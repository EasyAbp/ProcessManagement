using System;
using JetBrains.Annotations;

namespace EasyAbp.ProcessManagement.Processes;

public interface IProcessState : IProcessStateBase
{
    /// <summary>
    /// Description for the state, optional. It supports both plain text and HTML.
    /// </summary>
    /// <example><![CDATA[<p>Data is loading...</p>]]></example>
    [CanBeNull]
    string StateDetailsText { get; }
}