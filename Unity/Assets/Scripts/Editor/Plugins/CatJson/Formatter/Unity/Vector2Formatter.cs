using System;
using Unity.Mathematics;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Vector2类型的Json格式化器
    /// </summary>
    public class Vector2Formatter : BaseJsonFormatter<Vector2>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Vector2 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override Vector2 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new Vector2(x,y);
        }
    }

    public class Float2Formatter : BaseJsonFormatter<float2>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, float2 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override float2 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new float2(x,y);
        }
    }
    
    
    public class Int2Formatter : BaseJsonFormatter<int2>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, int2 value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"x\":{value.x.ToString()}");
            parser.Append(", ");
            parser.Append($"\"y\":{value.y.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override int2 ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int x = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            int y = parser.Lexer.GetNextTokenByType(TokenType.Number).AsInt();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new int2(x,y);
        }
    }
}