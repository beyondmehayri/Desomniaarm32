using Autofac;
using MadWizard.Desomnia.Network.HyperV;
using MadWizard.Desomnia.Network.Knocking;
using MadWizard.Desomnia.Network.Services.Knocking;

namespace MadWizard.Desomnia.Network.FirewallKnockOperator
{
    public class PluginModule : Desomnia.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Receiver>()
                .Named<IKnockDetector>("fko")
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<Sender>()
                .Named<IKnockMethod>("fko")
                .AsImplementedInterfaces()
                .SingleInstance();

            // select default auth key type
            builder.ComponentRegistryBuilder.Registered += (sender, args) =>
            {
                if (args.ComponentRegistration.IsLimitedTo<KnockStanza>())
                    args.ComponentRegistration.PipelineBuilding += (_, pipeline) =>
                        pipeline.Use(new DigestTypeSelector());
            };
        }
    }
}
