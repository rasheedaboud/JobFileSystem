using Fluxor;
using JobFileSystem.Shared.Contacts;

namespace Client.Features.Contacts
{
    public static class ContactReducers
    {

        [ReducerMethod]
        public static ContactState GetAction(ContactState state, GetItemsAction<ContactDto> action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static ContactState GetSuccessAction(ContactState state, GetItemsSuccessAction<ContactDto> action) =>
             state with { IsLoading = false, Items = action.Items };

        [ReducerMethod]
        public static ContactState SelectuccessAction(ContactState state, SelectItemAction<ContactDto> action) =>
             state with { SelectedItem = action.Item };

        [ReducerMethod]
        public static ContactState DeSelectuccessAction(ContactState state, DeSelectItemAction<ContactDto> action)
                => state with { SelectedItem = new() };

        [ReducerMethod]
        public static ContactState AddContactAction(ContactState state, AddItemAction<ContactDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static ContactState AdduccessAction(ContactState state, AddItemSuccessAction<ContactDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static ContactState DeleteAction(ContactState state, DeleteItemAction<ContactDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static ContactState DeleteActionSucess(ContactState state, DeleteItemSuccessAction<ContactDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };


        [ReducerMethod]
        public static ContactState EditContactAction(ContactState state, EditItemAction<ContactDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static ContactState EdituccessAction(ContactState state, EditItemSuccessAction<ContactDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static ContactState FailureAction(ContactState state, FailureAction _) =>
                state with { IsLoading = false };
    }
}
