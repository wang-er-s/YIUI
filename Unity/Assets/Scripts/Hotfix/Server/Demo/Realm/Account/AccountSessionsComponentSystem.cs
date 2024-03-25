using System.Collections.Generic;

namespace ET.Server
{

    [EntitySystemOf(typeof(AccountSessionsComponent))]
    [FriendOf(typeof(AccountSessionsComponent))]
    public static partial class AccountSessionsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.AccountSessionsComponent self)
        {
            self.Account2Session = new Dictionary<string, EntityRef<Session>>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.AccountSessionsComponent self)
        {
            self.Account2Session.Clear();
        }

        public static void Add(this AccountSessionsComponent self, string accountName, Session session)
        {
            self.Account2Session[accountName] = session;
        }

        public static void Remove(this AccountSessionsComponent self, string accountName)
        {
            self.Account2Session.Remove(accountName);
        }

        public static Session Get(this AccountSessionsComponent self, string accountName)
        {
            self.Account2Session.TryGetValue(accountName, out var result);
            return result;
        }
    }
}