// ***********************************************************************
// Assembly         : DynamicXml.Scanner
// Author           : Milo.Wical
// Created          : 05-29-2018
//
// Last Modified By : Milo.Wical
// Last Modified On : 05-29-2018
// ***********************************************************************
// <copyright file="StreamBufferReader.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicXml.Scanner.DFA.BufferReader
{
    using System;
    using System.IO;

    /// <inheritdoc />
    /// <summary>
    /// Class StreamBufferReader.
    /// </summary>
    /// <seealso cref="T:DynamicXml.Scanner.DFA.BufferReader.IBufferReader" />
    public class StreamBufferReader : IBufferReader
    {
        /// <summary>
        /// The reader
        /// </summary>
        private readonly StreamReader _reader;

        /// <summary>
        /// The buffer
        /// </summary>
        private char[] _buffer;

        /// <inheritdoc />
        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <value>The buffer.</value>
        /// <remarks>This function creates a copy of the
        /// buffer so that any changes applied to it in the 
        /// client application won't alter the state of
        /// the buffer in the buffer reader.</remarks>
        public char[] Buffer => (char[]) _buffer?.Clone();

        /// <inheritdoc />
        /// <summary>
        /// Gets a value indicating whether [end of stream].
        /// </summary>
        /// <value><c>true</c> if [end of stream]; otherwise, <c>false</c>.</value>
        public bool EndOfStream { get; private set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynamicXml.Scanner.DFA.BufferReader.StreamBufferReader" /> class.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public StreamBufferReader(Stream inputStream) :
            this(inputStream, 1)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamBufferReader"/> class.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        /// <param name="lookaheadBufferSize">Size of the lookahead buffer.</param>
        /// <exception cref="ArgumentNullException">inputStream</exception>
        /// <exception cref="ArgumentException">
        /// Cannot read from the supplied stream.
        /// or
        /// illegal buffer size
        /// </exception>
        public StreamBufferReader(Stream inputStream, int lookaheadBufferSize)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (!inputStream.CanRead) throw new ArgumentException("Cannot read from the supplied stream.");
            if (lookaheadBufferSize < 1)
                throw new ArgumentException(
                    $"Illegal lookahead buffer size: {lookaheadBufferSize}; must be greater than or equal to 1.");

            _reader = new StreamReader(inputStream);
            _buffer = new char[lookaheadBufferSize];
            EndOfStream = false;

            AdvanceBuffer(); //Preload the buffer for the first lexeme lookup.
        }

        /// <inheritdoc />
        /// <summary>
        /// Advances the buffer.
        /// </summary>
        public void AdvanceBuffer()
        {
            if (EndOfStream) return;

            var charactersRead = _reader.Read(_buffer, 0, _buffer.Length);

            if (charactersRead > 0) return;

            EndOfStream = true;
            _reader.Close();
            _reader.Dispose();
            _buffer = null;
        }
    }
}
