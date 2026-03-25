using MadWizard.Desomnia.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadWizard.Desomnia.Service.Duo.Configuration
{
    public class DuoInstanceInfo(string name)
    {
        public string Name { get; set; } = name;

        public NamedAction? OnDemand { get; set; }

        public DelayedAction? OnIdle { get; set; }

        public DelayedAction? OnLogin { get; set; }
        public DelayedAction? OnStart { get; set; }
        public DelayedAction? OnStop { get; set; }
        public DelayedAction? OnLogout { get; set; }

    }
}
