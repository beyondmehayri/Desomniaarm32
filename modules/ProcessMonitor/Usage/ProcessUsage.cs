namespace MadWizard.Desomnia.Process
{
    public class ProcessUsage(string name) : UsageToken
    {
        public string Name => name;

        public double? Usage { get; private init; }
        public TimeSpan? Time { get; private init; }

        public ProcessUsage(string name, double usage) : this(name)
        {
            this.Usage = usage;
        }

        public ProcessUsage(string name, TimeSpan time) : this(name)
        {
            this.Time = time;
        }

        public override string ToString()
        {
            var str = "{";

            str += $"{Name}";

            if (Usage is double usage)
                str += $":{usage*100:0.0}%";
            else if (Time is TimeSpan time)
                str += $":{time}";

            str += "}";

            return str;
        }
    }
}
