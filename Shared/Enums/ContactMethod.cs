using Ardalis.SmartEnum;

namespace JobFileSystem.Shared.Enums
{
    public abstract class ContactMethod : SmartEnum<ContactMethod>
    {
        public static readonly ContactMethod Email = new EmailContact();
        public static readonly ContactMethod Phone = new PhoneContact();

        private const string _email = "Email";
        private const string _phone = "Phone";



        private ContactMethod(string name, int value) : base(name, value)
        {
        }
        private sealed class EmailContact : ContactMethod
        {
            public EmailContact() : base(_email, 1) { }

        }

        private sealed class PhoneContact : ContactMethod
        {
            public PhoneContact() : base(_phone, 2) { }

        }
    }
}
