using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(TokenComponent))]
    [FriendOf(typeof(ET.Server.TokenComponent))]
    public static partial class TokenComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.TokenComponent self)
        {
            self.Account2Token = new Dictionary<string, string>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.TokenComponent self)
        {
            self.Account2Token.Clear();
        }

        public static void Add(this TokenComponent self, string accountName, string token)
        {
            self.Account2Token[accountName] = token;
        }

        public static void Remove(this TokenComponent self, string accountName)
        {
            self.Account2Token.Remove(accountName);
        }

        public static string Get(this TokenComponent self, string accountName)
        {
            self.Account2Token.TryGetValue(accountName, out var result);
            return result;
        }
    }
}