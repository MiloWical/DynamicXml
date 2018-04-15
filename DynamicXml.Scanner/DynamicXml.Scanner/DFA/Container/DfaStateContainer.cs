// ***********************************************************************
// Assembly         : DynamicXml.Scanner
// Author           : Milo.Wical
// Created          : 04-15-2018
//
// Last Modified By : Milo.Wical
// Last Modified On : 04-15-2018
// ***********************************************************************
// <copyright file="DfaStateContainer.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DynamicXml.Scanner.DFA.Container
{
    #region Usings

    using System;
    using System.Collections.Concurrent;
    using State;

    #endregion

    /// <summary>
    /// Class DfaStateContainer.
    /// </summary>
    /// <seealso cref="DynamicXml.Scanner.DFA.Container.IStateContainer" />
    public class DfaStateContainer : IStateContainer
    {
        private static readonly ConcurrentDictionary<string, IState> StateDictionary = new ConcurrentDictionary<string, IState>();

        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="T:DynamicXml.Scanner.DFA.State.IState" /> with the specified state name.
        /// </summary>
        /// <param name="stateName">Name of the state.</param>
        /// <returns>IState.</returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public IState this[string stateName] => StateDictionary[stateName];

        /// <inheritdoc />
        /// <summary>
        /// Registers the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public void Register(IState state, string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException("egistrations of states must have a non-empty, non-ull  name.", nameof(name));

            StateDictionary[name] = state;
        }
    }
}