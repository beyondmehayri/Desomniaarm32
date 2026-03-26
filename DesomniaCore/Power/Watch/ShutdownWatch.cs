using Autofac;
using MadWizard.Desomnia.Power.Manager;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MadWizard.Desomnia.Power.Watch
{
    public class ShutdownWatch : IStartable, IDisposable
    {
        public required ILogger<ShutdownWatch> Logger { private get; set; }

        void IStartable.Start()
        {

        }

        void IDisposable.Dispose()
        {
            Logger.LogInformation("Shutdown");
        }
    }
}
