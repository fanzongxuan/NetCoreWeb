﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Core.Interface
{
    public interface IStartupTask
    {
        void Execute();

        int Order { get; }
    }
}
