using System;
using Unity.Mathematics;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Vector4类型的Json格式化器
    /// </summary>
    public class Vector4Formatter : BaseJsonFormatter<Vector4>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Vector4 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append(", ");
            parser.Append($"\"w\":{value.w.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override Vector4 ParseJson(JsonParser parser, Type type, Type realType)
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
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float w = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new Vector4(x,y,z,w);
        }
    }
    
    public class Float4Formatter : BaseJsonFormatter<float4>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, float4 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append(", ");
            parser.Append($"\"w\":{value.w.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override float4 ParseJson(JsonParser parser, Type type, Type realType)
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
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float w = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new float4(x,y,z,w);
        }
    }
    
    public class Int4Formatter : BaseJsonFormatter<int4>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, int4 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append(", ");
            parser.Append($"\"z\":{value.z.ToString()}");
            parser.Append(", ");
            parser.Append($"\"w\":{value.w.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override int4 ParseJson(JsonParser parser, Type type, Type realType)
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
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int w = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new int4(x,y,z,w);
        }
    }
}