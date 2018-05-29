// ***********************************************************************
// Assembly         : DynamicXml.Scanner
// Author           : Milo.Wical
// Created          : 04-15-2018
//
// Last Modified By : Milo.Wical
// Last Modified On : 04-15-2018
// ***********************************************************************
// <copyright file="IStateContainer.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using DynamicXml.Scanner.DFA.State;

namespace DynamicXml.Scanner.DFA.Container
{
    using System;
    using Edge;
    using Lexeme;

    /// <summary>
    /// Interface IStateContainer
    /// </summary>
    public interface IStateContainer
    {
        /// <summary>
        /// Gets the <see cref="IState"/> with the specified state name.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>IState.</returns>
        IState this[string stateName] { get; }

        /// <summary>
        /// Registers the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="name">The name.</param>
        void Register(IState state, string name);
    }
}