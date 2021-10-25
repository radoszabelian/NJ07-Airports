﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports.Commands
{
    public interface ICommand
    {
        string GetDescription();
        void Start();
    }
}