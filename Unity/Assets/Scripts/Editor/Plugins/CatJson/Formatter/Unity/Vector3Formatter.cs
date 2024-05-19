using System;
using Unity.Mathematics;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Vector3类型的Json格式化器
    /// </summary>
    public class Vector3Formatter : BaseJsonFormatter<Vector3>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Vector3 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override Vector3 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float z = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new Vector3(x, y, z);
        }
    }

    public class Float3Formatter : BaseJsonFormatter<float3>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, float3 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override float3 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float z = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new float3(x, y, z);
        }
    }


    public class Int3Formatter : BaseJsonFormatter<int3>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, int3 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override int3 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int z = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new int3(x, y, z);
        }
    }
}