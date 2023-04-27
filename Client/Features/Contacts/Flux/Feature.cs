using Fluxor;

namespace Client.Features.Contacts
{
    public class Feature : Feature<ContactState>
    {
        public override string GetName() => nameof(ContactState);
        protected override ContactState GetInitialState() => new();
    }
}
