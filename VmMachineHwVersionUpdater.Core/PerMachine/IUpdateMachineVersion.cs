﻿using System.Collections.Generic;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
    /// <inheritdoc cref="IUpsertVmxLine{TIn2}" />
    public interface IUpdateMachineVersion : IRunFor2<List<Machine>, int>, IUpsertVmxLine<int>
    {
    }
}