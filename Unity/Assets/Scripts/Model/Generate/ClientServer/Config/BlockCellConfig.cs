
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;

namespace ET
{
    [EnableClass]
    public sealed partial class BlockCellConfig : BeanBase
    {
        public BlockCellConfig(ByteBuf _buf)
        {
            ID = _buf.ReadInt();
            Hp = _buf.ReadInt();
            {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);PrefabPath = new System.Collections.Generic.List<string>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { string _e0;  _e0 = _buf.ReadString(); PrefabPath.Add(_e0);}}
            {int n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);Colors = new System.Collections.Generic.List<int>(n0);for(var i0 = 0 ; i0 < n0 ; i0++) { int _e0;  _e0 = _buf.ReadInt(); Colors.Add(_e0);}}

            PostInit();
        }

        public static BlockCellConfig DeserializeBlockCellConfig(ByteBuf _buf)
        {
            return new BlockCellConfig(_buf);
        }

        public readonly int ID;

        public readonly int Hp;

        public readonly System.Collections.Generic.List<string> PrefabPath;

        public readonly System.Collections.Generic.List<int> Colors;


        public const int __ID__ = 523809681;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef()
        {
            
            
            
            
        }

        public override string ToString()
        {
            return "{ "
            + "ID:" + ID + ","
            + "Hp:" + Hp + ","
            + "PrefabPath:" + Luban.StringUtil.CollectionToString(PrefabPath) + ","
            + "Colors:" + Luban.StringUtil.CollectionToString(Colors) + ","
            + "}";
        }

        partial void PostInit();
    }
}
