using Fluxor;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Client.Features.Estimates.Flux.Actions;
using JobFileSystem.Shared.Estimates;
using JobFileSystem.Shared.LineItems;

namespace Client.Features.Estimates
{
    public static class EstimateReducers
    {

        [ReducerMethod]
        public static EstimateState AttachmentAddedSuccessAction(EstimateState state, AttachmentAddedSuccessAction<EstimateDto> action) =>
            state with { IsLoading = false, SelectedItem = action.SelectedItem };

        [ReducerMethod]
        public static EstimateState AttachmentAddedAction(EstimateState state, AttachmentAddedAction<EstimateDto> action)
         => state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState PrintEstimateAction(EstimateState state, PrintEstimateAction action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState PrintEstimateSuccessAction(EstimateState state, PrintEstimateSuccessAction action) =>
            state with { IsLoading = false };
        [ReducerMethod]
        public static EstimateState GetAction(EstimateState state, GetItemsAction<EstimateDto> action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState AddAttachment(EstimateState state, AttachmentAddedToEstimate action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState AddAttachmentSuccess(EstimateState state, AttachmentAddedToEstimateSuccess action) =>
            state with { IsLoading = false, SelectedItem = action.dto };

        [ReducerMethod]
        public static EstimateState GetSuccessAction(EstimateState state, GetItemsSuccessAction<EstimateDto> action) =>
             state with { IsLoading = false, Items = action.Items };

        [ReducerMethod]
        public static EstimateState AddLineItemSuccessAction(EstimateState state, AddItemSuccessAction<LineItemDto> action)
        {
            state.SelectedItem.LineItems.Add(action.Item);
            return state with { IsLoading = false };
        }

        [ReducerMethod]
        public static EstimateState UpdateLineItemSuccessAction(EstimateState state, EditItemSuccessAction<LineItemDto> action)
        {
            var item = state.SelectedItem;
            var items = item.LineItems.Where(x => x.Id != action.Item.Id).ToList();
            items.Add(action.Item);
            item.LineItems = items;
            return state with { IsLoading = false, SelectedItem = item };
        }

        [ReducerMethod]
        public static EstimateState SelectuccessAction(EstimateState state, SelectItemAction<EstimateDto> action) =>
             state with { SelectedItem = action.Item };

        [ReducerMethod]
        public static EstimateState DeSelectuccessAction(EstimateState state, DeSelectItemAction<EstimateDto> action)
                => state with { SelectedItem = null };

        [ReducerMethod]
        public static EstimateState AddEstimateAction(EstimateState state, AddItemAction<EstimateDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState AddEstimateSuccessAction(EstimateState state, AddItemSuccessAction<EstimateDto> action) =>
             state with { IsLoading = false, SelectedItem = action.Item };

        [ReducerMethod]
        public static EstimateState DeleteAction(EstimateState state, DeleteItemAction<EstimateDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState DeleteActionSucess(EstimateState state, DeleteItemSuccessAction<EstimateDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static EstimateState DeleteLineItemActionSucess(EstimateState state, DeleteItemSuccessAction<LineItemDto> action)
        {
            var selected = state.SelectedItem;

            var items = selected.LineItems.Where(x => x.Id != action.Id).ToList();

            selected.LineItems = items;

            return state with { IsLoading = false, SelectedItem = selected };
        }



        [ReducerMethod]
        public static EstimateState EditEstimateAction(EstimateState state, EditEstimate _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static EstimateState EditEstimateSuccessAction(EstimateState state, EditEstimateSuccess action) =>
             state with { IsLoading = false, SelectedItem = action.dto };

        [ReducerMethod]
        public static EstimateState FailureAction(EstimateState state, FailureAction _) =>
                state with { IsLoading = false };
    }
}
