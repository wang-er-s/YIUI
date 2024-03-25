namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(ET.Server.AccountInfo))]
    public class C2R_LoginAccountHandler : MessageSessionHandler<C2R_LoginAccount, R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.Err_RequestRepeatedly;
                session.Disconnect().Coroutine();
            }
            
            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                session.Disconnect().Coroutine();
                response.Error = ErrorCode.Err_AccountError;
                session.Disconnect().Coroutine();
                return;
            }

            Scene root = session.Root();

            // 防止同一个session多次登录
            using (session.AddComponent<SessionLockingComponent>())
            {
                // 协程锁防止同一个账号在操作数据库过程中，进行其他操作
                using (await session.Root().GetComponent<CoroutineLockComponent>()
                               .Wait(CoroutineLockType.LoginAccount, request.AccountName.GetLongHashCode()))
                {
                    var zoneDb = root.GetComponent<DBManagerComponent>().GetZoneDB(root.Zone());
                    var accountInfos = await zoneDb.Query<AccountInfo>(c => c.AccountName == request.AccountName);
                    if (accountInfos.Count > 0)
                    {
                        var accountInfo = accountInfos[0];
                        if (accountInfo.Password != request.Password)
                        {
                            response.Error = ErrorCode.Err_AccountError;
                            session.Disconnect().Coroutine();
                            accountInfo.Dispose();
                            return;
                        }

                        if (accountInfo.AccountType == AccountType.BlackList)
                        {
                            response.Error = ErrorCode.Err_AccountInBlackList;
                            session.Disconnect().Coroutine();
                            accountInfo.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        var accountInfo = session.AddChild<AccountInfo>();
                        accountInfo.AccountName = request.AccountName;
                        accountInfo.Password = request.Password;
                        accountInfo.CreateTime = TimeInfo.Instance.ServerNow();
                        accountInfo.AccountType = AccountType.General;
                        await zoneDb.Save(accountInfo);
                    }

                    R2L_LoginAccountRequest r2LLoginAccountRequest = R2L_LoginAccountRequest.Create();
                    r2LLoginAccountRequest.Account = request.AccountName;

                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenter;
                    var loginAccountResponse = await session.Fiber().Root.GetComponent<MessageSender>().Call(loginCenterConfig.ActorId, r2LLoginAccountRequest);
                    if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = loginAccountResponse.Error;
                        session.Disconnect().Coroutine();
                        return;
                    }

                    Session oldSession = session.Root().GetComponent<AccountSessionsComponent>().Get(request.AccountName);
                    if (oldSession != null)
                    {
                        oldSession.Send(R2C_Dissconnect.Create());
                        oldSession.Disconnect().Coroutine();
                    }
                    
                    session.Root().GetComponent<AccountSessionsComponent>().Add(request.AccountName, session);
                    // session.AddComponent<accountch>()

                    string token = TimeInfo.Instance.ServerNow().ToString() + RandomGenerator.RandomNumber(int.MinValue, int.MaxValue);
                    session.Root().GetComponent<TokenComponent>().Remove(request.AccountName);
                    session.Root().GetComponent<TokenComponent>().Add(request.AccountName, token);

                    response.Token = token;
                }
            }
        }
    }
}