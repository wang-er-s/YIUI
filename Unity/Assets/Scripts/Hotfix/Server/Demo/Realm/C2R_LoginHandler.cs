using System;
using System.Net;


namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOf(typeof(AccountInfo))]
    public class C2R_LoginHandler : MessageSessionHandler<C2R_Login, R2C_Login>
    {
        protected override async ETTask Run(Session session, C2R_Login request, R2C_Login response)
        {
            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
            {
                CloseSession(session).Coroutine();
                response.Error = ErrorCode.Err_AccountError;
                return;
            }

            Scene root = session.Root();
            
            // 协程锁防止同一个账号在操作数据库过程中，进行其他操作
            using(await session.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.LoginAccount, request.Account.GetLongHashCode()))
            {
                var zoneDb = root.GetComponent<DBManagerComponent>().GetZoneDB(root.Zone());
                var accountInfos = await zoneDb.Query<AccountInfo>(c => c.AccountName == request.Account);
                if (accountInfos.Count <= 0)
                {
                    var accountInfosComponent = session.GetComponent<AccountInfosComponent>() ?? session.AddComponent<AccountInfosComponent>();
                    var accountInfo = accountInfosComponent.AddChild<AccountInfo>();
                    accountInfo.AccountName = request.Account;
                    accountInfo.Password = request.Password;
                    accountInfo.CreateTime = TimeInfo.Instance.ServerNow();
                    accountInfo.AccountType = AccountType.General;
                    await zoneDb.Save(accountInfo);
                }
                else
                {
                    var accountInfo = accountInfos[0];
                    if (accountInfo.Password != request.Password)
                    {
                        response.Error = ErrorCode.Err_AccountError;
                        CloseSession(session).Coroutine();
                        return;
                    }
                }
            }

            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(session.Zone(), request.Account);
            Log.Debug($"gate address: {config}");
            // 向gate请求一个key,客户端可以拿着这个key连接gate
            R2G_GetLoginKey r2GGetLoginKey = R2G_GetLoginKey.Create();
            r2GGetLoginKey.Account = request.Account;
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await session.Fiber().Root.GetComponent<MessageSender>().Call(config.ActorId, r2GGetLoginKey);

            response.Address = config.InnerIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;
            response.GateId = g2RGetLoginKey.GateId;

            CloseSession(session).Coroutine();
        }

        private async ETTask CloseSession(Session session)
        {
            await session.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            session.Dispose();
        }
    }
}
