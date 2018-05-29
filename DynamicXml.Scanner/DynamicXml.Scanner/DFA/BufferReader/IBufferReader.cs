// ***********************************************************************
// Assembly         : DynamicXml.Scanner
// Author           : Milo.Wical
// Created          : 05-29-2018
//
// Last Modified By : Milo.Wical
// Last Modified On : 05-29-2018
// ***********************************************************************
// <copyright file="IBufferReader.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicXml.Scanner.DFA.BufferReader
{
    /// <summary>
    /// Interface IBufferReader
    /// </summary>
    public interface IBufferReader
    {
        /// <summary>
        /// Gets a value indicating whether [end of stream].
        /// </summary>
        /// <value><c>true</c> if [end of stream]; otherwise, <c>false</c>.</value>
        bool EndOfStream { get; }

        /// <summary>
        /// Advances the buffer.
        /// </summary>
        void AdvanceBuffer();
    }
}
