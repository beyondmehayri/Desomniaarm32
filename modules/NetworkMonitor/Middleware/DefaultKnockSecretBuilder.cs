using Autofac;
using Autofac.Core.Resolving.Pipeline;
using MadWizard.Desomnia.Network.Configuration.Knocking;
using MadWizard.Desomnia.Network.Knocking.Secrets;

namespace MadWizard.Desomnia.Network.Middleware
{
    public sealed class DefaultKnockSecretBuilder : IResolveMiddleware
    {
        public PipelinePhase Phase => PipelinePhase.ParameterSelection;

        public void Execute(ResolveRequestContext context, Action<ResolveRequestContext> next)
        {
            var data = context.FirstParameterOfType<SharedSecretData>()!;
            
            if (context.FirstParameterOfType<SharedSecret>() is null)
            {
                var secret = BuildSharedSecret(data);

                context.ChangeParameters([..context.Parameters, TypedParameter.From(secret)]);
            }

            next(context);
        }

        private static SharedSecret BuildSharedSecret(SharedSecretData data)
        {
            byte[]? key = null;
            byte[]? authKey = null;

            DigestType authType = default;

            string defaultEncoding = data.Encoding ?? "UTF-8";

            if (data.Key is KeyData keyData)
            {
                key = SharedSecret.TryConvert(keyData.Text, keyData.Encoding ?? defaultEncoding);

                if (data.AuthKey is AuthKeyData authKeyData)
                {
                    authKey = SharedSecret.TryConvert(authKeyData.Text, authKeyData.Encoding ?? defaultEncoding);
                    authType = authKeyData.Type;
                }
            }
            else
            {
                key = SharedSecret.TryConvert(data.Text, defaultEncoding);
            }

            return new SharedSecret(key ?? throw new Exception($"Invalid SecretKey = '{data.Label}'"), authKey, authType);
        }
    }
}