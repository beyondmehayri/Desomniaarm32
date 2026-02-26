using Autofac;
using MadWizard.Desomnia.Process.Configuration;
using MadWizard.Desomnia.Process.Manager;
using Microsoft.Extensions.Logging;


namespace MadWizard.Desomnia.Process
{
    public class ProcessMonitor(ProcessMonitorConfig config, IProcessManager manager) : ResourceMonitor<ProcessWatch>, IStartable
    {
        public required ILogger<ProcessMonitor> Logger { get; set; }

        void IStartable.Start()
        {
            foreach (var info in config.Process)
            {
                StartTracking(new SystemProcessWatch(manager, info));
            }

            Logger.LogDebug("Startup complete");
        }

        private class SystemProcessWatch(IProcessManager manager, ProcessWatchInfo info) : ProcessWatch(info)
        {
            protected override IEnumerable<IProcess> EnumerateProcesses() => manager;
        }
    }
}
