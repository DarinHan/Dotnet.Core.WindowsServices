using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dotnet.Core.WindowsHostService
{
    public class WinHostService: WebHostService
    {
        private ILoggerFactory _logFactory;
        private ILogger _logger;
        private Task task;
        private CancellationTokenSource source;

        public WinHostService(IWebHost host) : base(host)
        {
            _logFactory = host.Services
                .GetRequiredService<ILoggerFactory>();
            _logger = _logFactory.CreateLogger(nameof(WinHostService));
        }
        protected override void OnStarting(string[] args)
        {
            try
            {
                source = new CancellationTokenSource();

                task = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        SyncServices.RunService(_logger);
                        Thread.Sleep(30 * 60 * 1000);
                    }
                }, source.Token);
            }
            catch (Exception ex)
            {
                source.Cancel();
                _logger.LogError(ex, "OnStarting error");
            }
            _logger.LogDebug("OnStarting method called.");
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            _logger.LogDebug("OnStarted method called.");
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            source.Cancel();
            _logger.LogDebug("OnStopping method called.");
            base.OnStopping();
        }

    }
}
