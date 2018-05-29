// ***********************************************************************
// Assembly         : DynamicXml.Scanner
// Author           : Milo.Wical
// Created          : 05-29-2018
//
// Last Modified By : Milo.Wical
// Last Modified On : 05-29-2018
// ***********************************************************************
// <copyright file="StringBufferReader.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicXml.Scanner.DFA.BufferReader
{
    using System.IO;
    using System.Text;

    /// <inheritdoc />
    /// <summary>
    /// Class StringBufferReader.
    /// </summary>
    /// <seealso cref="T:DynamicXml.Scanner.DFA.BufferReader.StreamBufferReader" />
    public class StringBufferReader : StreamBufferReader
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynamicXml.Scanner.DFA.BufferReader.StringBufferReader" /> class.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        public StringBufferReader(string inputString) :
            base(new MemoryStream(Encoding.UTF8.GetBytes(inputString)), 1)
        { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynamicXml.Scanner.DFA.BufferReader.StringBufferReader" /> class.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <param name="lookaheadBufferSize">Size of the lookahead buffer.</param>
        public StringBufferReader(string inputString, int lookaheadBufferSize) :
            base(new MemoryStream(Encoding.UTF8.GetBytes(inputString)), lookaheadBufferSize)
        { }
    }
}
