using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorClassLib.Services;

public interface IEnvironmentService
{
    public bool IsWebApp { get; }
}

