using MadWizard.Desomnia.Process;
using MadWizard.Desomnia.Process.Manager;
using MadWizard.Desomnia.Session.Configuration;
using MadWizard.Desomnia.Session.Manager;

namespace MadWizard.Desomnia.Session
{
    public class SessionProcessWatch : ProcessWatch
    {
        [EventContext]
        public ISession Session { get; private init; }

        public SessionProcessWatch(SessionWatch watch, SessionProcessWatchInfo info) : base(info)
        {
            Session = watch.Session;

            // TODO support SessionIdle Events?

            //watch.Idle += (sender, args) => TriggerAction(info.OnSessionIdle);
            //watch.Busy += (sender, args) => ResetActionTimer(info.OnSessionIdle);
        }

        protected override IEnumerable<IProcess> EnumerateProcesses() => Session.Processes;

        protected override SessionProcessUsage CreateUsageToken(double usage) => new(Name, usage) { UserName = Session.UserName };
        protected override SessionProcessUsage CreateUsageToken(TimeSpan time) => new(Name, time) { UserName = Session.UserName };
    }
}
