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
            self.Account2Session = new Dictionary<long, long>();
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.AccountSessionsComponent self)
        {
            self.Account2Session.Clear();
        }

        private static void Add(this AccountSessionsComponent self, long accountId, long sessionInsId)
        {
            self.Account2Session[accountId] = sessionInsId;
        }

        private static void Remove(this AccountSessionsComponent self, long accountId)
        {
            self.Account2Session.Remove(accountId);
        }

        private static long Get(this AccountSessionsComponent self, long accountId)
        {
            self.Account2Session.TryGetValue(accountId, out var result);
            return result;
        }
    }
}