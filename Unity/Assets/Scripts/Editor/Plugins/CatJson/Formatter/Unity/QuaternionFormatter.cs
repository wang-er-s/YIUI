using System;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Quaternion类型的Json格式化器
    /// </summary>
    public class QuaternionFormatter : BaseJsonFormatter<Quaternion>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Quaternion value, Type type, Type realType, int depth)
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
        public override Quaternion ParseJson(JsonParser parser, Type type, Type realType)
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
            return new Quaternion(x,y,z,w);
        }
    }
}