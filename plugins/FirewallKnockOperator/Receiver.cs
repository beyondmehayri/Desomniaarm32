using MadWizard.Desomnia.Network.Knocking;
using MadWizard.Desomnia.Network.Knocking.Secrets;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using System.Security.Cryptography;
using System.Text;

namespace MadWizard.Desomnia.Network.FirewallKnockOperator
{
    internal class Receiver : Base, IKnockDetector
    {
        public required ILogger<Sender> Logger { private get; init; }

        IEnumerable<KnockEvent> IKnockDetector.Examine(IPPacket packet, SharedSecret secret)
        {
            if (packet.PayloadPacket is TransportPacket transport)
            {
                Packet data;
                try
                {
                    var bytes = TransformPayload(transport.PayloadData);

                    if (secret.Auth is HMAC auth)
                    {
                        int length = auth.HashSize / 8;

                        byte[] hmac = bytes[^length..]; bytes = bytes[..^length]; // chop off HMAC by its length

                        if (!hmac.SequenceEqual(CalculateHMAC(EncodeBase64(bytes), auth)))
                        {
                            throw new CryptographicException("HMAC verification FAILED.");
                        }
                    }

                    byte[] salt = [.. bytes.Skip(SALT_PREFIX_BYTES.Length).Take(SALT_LENGTH)];
                    byte[] ciphertext = [.. bytes.Skip(SALT_PREFIX_BYTES.Length + SALT_LENGTH)];

                    (byte[] key, byte[] iv) = DeriveKeyIV(secret.Key, salt, secret.Key.Length);

                    string plaintext = Encoding.ASCII.GetString(DecryptAES(ciphertext, key, iv));

                    // TODO: verify HMAC if secret.AuthKey is set
                    // TODO: verify plaintext

                    data = new Packet(plaintext);
                }
                catch (Exception ex)
                {
                    Logger.LogDebug(ex, "Failed to decrypt/analyze FKO packet");

                    yield break;
                }

                yield return new KnockEvent
                {
                    Time = data.Timestamp.DateTime,
                    SourceAddress = packet.SourceAddress, // data.SourceAddress, // TODO: implement public IP resolution
                    TargetPort = data.TargetPort,
                };
            }
        }

        static byte[] TransformPayload(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes);

            if (!str.StartsWith(SALT_PREFIX_BASE64))
            {
                str = SALT_PREFIX_BASE64 + str;
            }

            return DecodeBase64(str);
        }

        static byte[] DecryptAES(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var dec = aes.CreateDecryptor(key, iv))
            {
                return dec.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
            }
        }
    }
}
