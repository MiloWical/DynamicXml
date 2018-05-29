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
    using Edge;
    using State;

    #endregion

    /// <inheritdoc />
    /// <summary>
    /// Class DfaStateContainer.
    /// </summary>
    /// <seealso cref="T:DynamicXml.Scanner.DFA.Container.IStateContainer" />
    public class DfaStateContainer : IStateContainer
    {
        /// <summary>
        /// The state dictionary
        /// </summary>
        private static readonly ConcurrentDictionary<string, IState> StateDictionary = new ConcurrentDictionary<string, IState>();

        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="T:DynamicXml.Scanner.DFA.State.IState" /> with the specified state type.
        /// </summary>
        /// <param name="stateType">Type of the state.</param>
        /// <returns>IState.</returns>
        public IState this[string stateType] => StateDictionary[stateType];

        /// <inheritdoc />
        /// <summary>
        /// Registers the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="name">The name.</param>
        public void Register(IState state, string name = null)
        {
            if (name == null)
                StateDictionary[nameof(state)] = state;
            else
                StateDictionary[name] = state;
        }
    }
}