using Unity.Mathematics;

namespace ET
{
    public static class ConfigTypeConvertor
    {
        public static float2 ToFloat2(_float2 v)
        {
            return new float2(v.X, v.Y);
        }

        public static float3 ToFloat3(_float3 v)
        {
            return new float3(v.X, v.Y, v.Z);
        }

        public static float4 ToFloat4(_float4 v)
        {
            return new float4(v.X, v.Y, v.Z, v.W);
        }
        
        public static int2 ToInt2(_int2 v)
        {
            return new int2(v.X, v.Y);
        }

        public static int3 ToInt3(_int3 v)
        {
            return new int3(v.X, v.Y, v.Z);
        }

        public static int4 ToInt4(_int4 v)
        {
            return new int4(v.X, v.Y, v.Z, v.W);
        }
    }
}