namespace ET.Client
{
    public static class LoginHelper
    {
        public static async ETTask Login(Scene root, string account, string password)
        {
            root.RemoveComponent<ClientSenderComponent>();
            
            ClientSenderComponent clientSenderComponent = root.AddComponent<ClientSenderComponent>();
            
            var response = await clientSenderComponent.LoginAsync(account, password);

            if (response.Error != ErrorCode.ERR_Success)
            {
                Log.Error($"login error :{response.Error}");
                return;
            }

            root.GetComponent<PlayerComponent>().MyId = response.PlayerId;
            
            await EventSystem.Instance.PublishAsync(root, new LoginFinish());
        }
    }
}