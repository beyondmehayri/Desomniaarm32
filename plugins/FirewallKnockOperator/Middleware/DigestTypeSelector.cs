using Autofac;
using Autofac.Core.Resolving.Pipeline;
using MadWizard.Desomnia.Network.Configuration.Knocking;
using MadWizard.Desomnia.Network.Knocking.Secrets;

namespace MadWizard.Desomnia.Network.HyperV
{
    public sealed class DigestTypeSelector : IResolveMiddleware
    {
        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            var data = context.FirstParameterOfType<SharedSecretData>();

            // TODO check knock method

            if (data?.AuthKey is AuthKeyData key)
            {
                if (key.Type == DigestType.Default)
                {
                    key.Type = DigestType.SHA256;
                }
            }

            next(context);
        }
    }
}
