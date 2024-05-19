using System;
using System.Runtime.CompilerServices;

namespace ET
{
    public static class EnumHelper
    {
        public static int EnumIndex<T>(int value)
        {
            int i = 0;
            foreach (object v in Enum.GetValues(typeof(T)))
            {
                if ((int)v == value)
                {
                    return i;
                }

                ++i;
            }

            return -1;
        }

        public static T FromString<T>(string str)
        {
            if (!Enum.IsDefined(typeof(T), str))
            {
                return default(T);
            }

            return (T)Enum.Parse(typeof(T), str);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
            unsafe
            {
                switch (sizeof(TEnum))
                {
                    case 1:
                        return (*(byte*)(&lhs) & *(byte*)(&rhs)) > 0;
                    case 2:
                        return (*(ushort*)(&lhs) & *(ushort*)(&rhs)) > 0;
                    case 4:
                        return (*(uint*)(&lhs) & *(uint*)(&rhs)) > 0;
                    case 8:
                        return (*(ulong*)(&lhs) & *(ulong*)(&rhs)) > 0;
                    default:
                        throw new Exception("Size does not match a known Enum backing type.");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong AddFlag<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
            unsafe
            {
                switch (sizeof(TEnum))
                {
                    case 1:
                        return (ulong)(*(byte*)(&lhs) | *(byte*)(&rhs));
                    case 2:
                        return (ulong)(*(ushort*)(&lhs) | *(ushort*)(&rhs));
                    case 4:
                        return (*(uint*)(&lhs) | *(uint*)(&rhs));
                    case 8:
                        return (*(ulong*)(&lhs) | *(ulong*)(&rhs));
                    default:
                        throw new Exception("Size does not match a known Enum backing type.");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong RemoveFlag<TEnum>(this TEnum lhs, TEnum rhs) where TEnum : unmanaged, Enum
        {
            unsafe
            {
                switch (sizeof(TEnum))
                {
                    case 1:
                        return (ulong)(*(byte*)(&lhs) & ~*(byte*)(&rhs));
                    case 2:
                        return (ulong)(*(ushort*)(&lhs) & ~*(ushort*)(&rhs));
                    case 4:
                        return *(uint*)(&lhs) & ~*(uint*)(&rhs);
                    case 8:
                        return (*(ulong*)(&lhs) & ~*(ulong*)(&rhs));
                    default:
                        throw new Exception("Size does not match a known Enum backing type.");
                }
            }
        }
    }
}