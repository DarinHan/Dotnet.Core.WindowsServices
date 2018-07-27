using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace Dotnet.Core.WindowsHostService
{
    public static class WinHostServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var winHostService = new WinHostService(host);
            ServiceBase.Run(winHostService);
        }
    }
}
