using System;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
/// <inheritdoc cref="IDisposable" />
public interface IUpsertVmxLine<in T> : IRunFor2<string, T>, IDisposable
{
}