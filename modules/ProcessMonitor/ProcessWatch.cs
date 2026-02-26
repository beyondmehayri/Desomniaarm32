using MadWizard.Desomnia.Process.Configuration;
using MadWizard.Desomnia.Process.Manager;


namespace MadWizard.Desomnia.Process
{
    public abstract class ProcessWatch(ProcessWatchInfo info) : Resource
    {
        private DateTime _lastMeasureTime;
        private TimeSpan _lastProcessorTime;

        protected string Name => info.Name;

        protected abstract IEnumerable<IProcess> EnumerateProcesses();

        protected virtual ProcessUsage CreateUsageToken(double usage) => new(Name, usage);
        protected virtual ProcessUsage CreateUsageToken(TimeSpan time) => new(Name, time);

        protected IEnumerable<IProcess> SelectProcesses()
        {
            IEnumerable<IProcess> processes = EnumerateProcesses();

            var parents = new HashSet<IProcess>();
            foreach (var process in processes)
            {
                if (info.Pattern.Matches(process.Name).Count == 0)
                    continue;

                if (parents.Add(process))
                    yield return process;
            }

            if (info.WatchChildren)
                foreach (var process in processes)
                {
                    if (parents.Contains(process))
                        continue;

                    foreach (var parent in parents)
                        if (process.HasParent(parent))
                            yield return process;
                }
        }

        private double MeasureUsage(out TimeSpan time)
        {
            DateTime measureTime = DateTime.UtcNow;

            var processorTime = SelectProcesses().Aggregate(TimeSpan.Zero, (time, process) => time + process.NativeProcess.TotalProcessorTime);

            try
            {
                time = (processorTime - _lastProcessorTime);
                var timeElapsed = (measureTime - _lastMeasureTime);

                return time.TotalMilliseconds / (Environment.ProcessorCount * timeElapsed.TotalMilliseconds);
            }
            finally
            {
                _lastProcessorTime = processorTime;
                _lastMeasureTime = measureTime;
            }
        }

        protected override IEnumerable<UsageToken> InspectResource(TimeSpan interval)
        {
            var usage = MeasureUsage(out TimeSpan time);

            if (info.MinCPU.AbsoluteTime is TimeSpan minTime)
            {
                if (time > minTime)
                {
                    yield return CreateUsageToken(time);
                }
            }
            else if (info.MinCPU.RelativeUsage is double minUsage)
            {
                if (usage > minUsage)
                {
                    yield return CreateUsageToken(usage);
                }
            }
        }

        [ActionHandler("stop")]
        internal void HandleActionStop(TimeSpan timeout = default) // TODO implement passing of timeout
        {
            foreach (var process in SelectProcesses())
            {
                process.Stop(timeout);
            }
        }
    }
}
