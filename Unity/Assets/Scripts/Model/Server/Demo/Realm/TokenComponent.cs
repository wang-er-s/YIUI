using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class TokenComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, string> Account2Token;
    }
}