using Autofac;
using MadWizard.Desomnia.Power.Manager;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MadWizard.Desomnia.Power.Watch
{
    public class StartupWatch : IStartable
    {
        public required ILogger<StartupWatch> Logger { private get; set; }

        void IStartable.Start()
        {
            Logger.LogInformation("Startup");
        }
    }
}
