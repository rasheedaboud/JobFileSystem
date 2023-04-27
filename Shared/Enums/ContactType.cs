using Ardalis.SmartEnum;

namespace JobFileSystem.Shared.Enums
{
    public abstract class ContactType : SmartEnum<ContactType>
    {
        public static readonly ContactType Client   = new ClientContact();
        public static readonly ContactType Supplier = new SupplierContact();

        private const string _client    = "Client";
        private const string _supplier  = "Supplier";



        private ContactType(string name, int value) : base(name, value)
        {
        }
        private sealed class ClientContact : ContactType
        {
            public ClientContact() : base(_client, 1) { }

        }

        private sealed class SupplierContact : ContactType
        {
            public SupplierContact() : base(_supplier, 2) { }

        }
    }
}
