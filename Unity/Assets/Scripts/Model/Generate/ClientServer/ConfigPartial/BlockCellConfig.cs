using System.Collections.Generic;

namespace ET
{
    public sealed partial class BlockCellConfig
    {
        public List<BlockColor> ColorEnums;
        
        partial void PostInit()
        {
            ColorEnums = new List<BlockColor>();
            foreach (var color in Colors)
            {
                ColorEnums.Add((BlockColor)color);
            }
        }
    }
}