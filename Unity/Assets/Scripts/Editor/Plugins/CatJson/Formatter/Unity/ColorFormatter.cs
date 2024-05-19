using System;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Color类型的Json格式化器
    /// </summary>
    public class ColorFormatter : BaseJsonFormatter<Color>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Color value, Type type, Type realType, int depth)
        {
            parser.Append('{');
            parser.Append($"\"r\":{value.r.ToString()}");
            parser.Append(", ");
            parser.Append($"\"g\":{value.g.ToString()}");
            parser.Append(", ");
            parser.Append($"\"b\":{value.b.ToString()}");
            parser.Append(", ");
            parser.Append($"\"a\":{value.a.ToString()}");
            parser.Append('}');
        }

        /// <inheritdoc />
        public override Color ParseJson(JsonParser parser, Type type, Type realType)
        {
            parser.Lexer.GetNextTokenByType(TokenType.LeftBrace);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float r = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float g = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float b = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.Comma);
            parser.Lexer.GetNextTokenByType(TokenType.String);
            parser.Lexer.GetNextTokenByType(TokenType.Colon);
            float a = parser.Lexer.GetNextTokenByType(TokenType.Number).AsFloat();
            parser.Lexer.GetNextTokenByType(TokenType.RightBrace);
            return new Color(r, g, b, a);
        }
    }
}