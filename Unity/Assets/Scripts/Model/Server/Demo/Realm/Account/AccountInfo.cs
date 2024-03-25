namespace ET.Server
{
    public enum AccountType
    {
        General,
        BlackList,
        GM,
    }

    [ChildOf(typeof(Session))]
    public class AccountInfo : Entity, IAwake
    {
        public string AccountName;
        public string Password;
        public long CreateTime;
        public AccountType AccountType;
    }
}
