using System;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [MessageHandler(SceneType.NetClient)]
    public class Main2NetClient_LoginHandler: MessageHandler<Scene, Main2NetClient_Login, NetClient2Main_Login>
    {
        protected override async ETTask Run(Scene root, Main2NetClient_Login request, NetClient2Main_Login response)
        {
            string account = request.Account;
            string password = request.Password;
            // 创建一个ETModel层的Session
            root.RemoveComponent<RouterAddressComponent>();
            // 获取路由跟realmDispatcher地址
            RouterAddressComponent routerAddressComponent =
                    root.AddComponent<RouterAddressComponent, string, int>(ConstValue.RouterHttpHost, ConstValue.RouterHttpPort);
            await routerAddressComponent.Init();
            root.AddComponent<NetComponent, AddressFamily, NetworkProtocol>(routerAddressComponent.RouterManagerIPAddress.AddressFamily, NetworkProtocol.UDP);
            root.GetComponent<FiberParentComponent>().ParentFiberId = request.OwnerFiberId;

            NetComponent netComponent = root.GetComponent<NetComponent>();
            
            IPEndPoint realmAddress = routerAddressComponent.GetRealmAddress(account);

            R2C_LoginAccount r2CLogin = null;
            Session session = null;
            try
            {
                session = await netComponent.CreateRouterSession(realmAddress, account, password);
                C2R_LoginAccount c2RLogin = C2R_LoginAccount.Create();
                c2RLogin.AccountName = account;
                c2RLogin.Password = password;
                r2CLogin = (R2C_LoginAccount)await session.Call(c2RLogin);
                if (r2CLogin.Error != ErrorCode.ERR_Success)
                {
                    response.Error = r2CLogin.Error;
                    response.Message = r2CLogin.Message;
                    session?.Dispose();
                    return;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                session?.Dispose();
                response.Error = ErrorCode.Err_NetworkError;
                return;
            }

            response.Token = r2CLogin.Token;
            root.AddComponent<SessionComponent>().Session = session;
        }
    }
}