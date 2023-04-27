using Client.Features.Estimates;
using Fluxor;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;

namespace Client.Features.LineItems
{
    public static class LineItemReducers
    {

        [ReducerMethod]
        public static LineItemState AttachmentsAddedToLineItem(LineItemState state, AttachmentAddedSuccessAction<EstimateDto> action)=>
            state with { IsLoading = false, Items = action.SelectedItem.LineItems };
                

        [ReducerMethod]
        public static LineItemState GetAction(LineItemState state, GetItemsAction<LineItemDto> action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static LineItemState GetSuccessAction(LineItemState state, GetItemsSuccessAction<LineItemDto> action) =>
             state with { IsLoading = false, Items = action.Items };

        [ReducerMethod]
        public static LineItemState SelectuccessAction(LineItemState state, SelectItemAction<LineItemDto> action) =>
             state with { SelectedItem = action.Item };

        [ReducerMethod]
        public static LineItemState DeSelectuccessAction(LineItemState state, DeSelectItemAction<LineItemDto> action)
                => state with { SelectedItem = null };

        [ReducerMethod]
        public static LineItemState AddItemAction(LineItemState state, AddItemAction<LineItemDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static LineItemState AddItemSuccessAction(LineItemState state, AddItemSuccessAction _ ) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static LineItemState DeleteItemAction(LineItemState state, DeleteItemAction<LineItemDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static LineItemState DeleteItemSuccessAction(LineItemState state, DeleteItemSuccessAction<LineItemDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };


        [ReducerMethod]
        public static LineItemState EditItemAction(LineItemState state, EditItemAction<LineItemDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static LineItemState EditItemSuccessAction(LineItemState state, EditItemSuccessAction _) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static LineItemState FailureAction(LineItemState state, FailureAction _) =>
                state with { IsLoading = false };
    }
}
