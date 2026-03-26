using System.Net;
using System.Net.NetworkInformation;

namespace MadWizard.Desomnia.Network.Address
{
    public struct AddressMapping
    {
        public IPAddress IPAddress { get; set; }
        public PhysicalAddress PhysicalAddress { get; set; }

        public AddressMapping(IPAddress ip, PhysicalAddress mac)
        {
            IPAddress = ip;
            PhysicalAddress = mac;
        }
    }
}