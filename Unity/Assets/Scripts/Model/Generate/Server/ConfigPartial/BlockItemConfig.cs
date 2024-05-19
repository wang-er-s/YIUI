namespace ET
{
    public sealed partial class BlockItemConfig
    {
        public BlockItemType BlockTypeEnum;

        partial void PostInit()
        {
            this.BlockTypeEnum = (BlockItemType)this.BlockType;
        }
    }
}