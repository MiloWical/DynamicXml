using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicXml.Scanner.LexemeReader
{
    using Lexeme;

    public class DfaLexemeReader : ILexemeReader
    {
        public Lexeme GetNextLexemeFromBuffer(LexemeType specifiedLexeme = LexemeType.Unspecified)
        {
            throw new NotImplementedException();
        }
    }
}
