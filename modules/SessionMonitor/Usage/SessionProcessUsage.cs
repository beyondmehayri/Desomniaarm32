using MadWizard.Desomnia.Process;

namespace MadWizard.Desomnia.Session
{
    public class SessionProcessUsage : ProcessUsage
    {
        public SessionProcessUsage(string name, double usage) : base(name, usage) { }
        public SessionProcessUsage(string name, TimeSpan time) : base(name, time) { }

        public required string UserName { get; init; }

        public override string ToString()
        {
            var str = base.ToString();

            str = str.Replace("{", "{" + $@"{UserName}\");

            return str;
        }
    }
}
