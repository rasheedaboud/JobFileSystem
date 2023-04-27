using Fluxor;
using JobFileSystem.Client.Features.Attachments.Actions;
using JobFileSystem.Shared.DTOs;

namespace Client.Features.MaterialTestReports
{
    public static class MaterialTestReportReducers
    {



        [ReducerMethod]
        public static MaterialTestReportState GetItemsAction(MaterialTestReportState state, GetItemsAction<MaterialTestReportDto> action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static MaterialTestReportState AttachmentAddedSuccessAction(MaterialTestReportState state, AttachmentAddedSuccessAction<MaterialTestReportDto> action) =>
            state with { IsLoading = false, SelectedItem = action.SelectedItem };

        [ReducerMethod]
        public static MaterialTestReportState AttachmentAddedAction(MaterialTestReportState state, AttachmentAddedAction<MaterialTestReportDto> action)
         => state with { IsLoading = true };

        [ReducerMethod]
        public static MaterialTestReportState GetItemsSuccessAction(MaterialTestReportState state, GetItemsSuccessAction<MaterialTestReportDto> action) =>
             state with { IsLoading = false, Items = action.Items };





        [ReducerMethod]
        public static MaterialTestReportState SelectuccessAction(MaterialTestReportState state, SelectItemAction<MaterialTestReportDto> action) =>
             state with { SelectedItem = action.Item };

        [ReducerMethod]
        public static MaterialTestReportState DeSelectuccessAction(MaterialTestReportState state, DeSelectItemAction<MaterialTestReportDto> action)
                => state with { SelectedItem = null };

        [ReducerMethod]
        public static MaterialTestReportState AddMaterialTestReportAction(MaterialTestReportState state, AddItemAction<MaterialTestReportDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static MaterialTestReportState AddMaterialTestReportSuccessAction(MaterialTestReportState state, AddItemSuccessAction<MaterialTestReportDto> action) =>
             state with { IsLoading = false, SelectedItem = action.Item };

        [ReducerMethod]
        public static MaterialTestReportState DeleteAction(MaterialTestReportState state, DeleteItemAction<MaterialTestReportDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static MaterialTestReportState DeleteActionSucess(MaterialTestReportState state, DeleteItemSuccessAction<MaterialTestReportDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };





        [ReducerMethod]
        public static MaterialTestReportState EditMaterialTestReportAction(MaterialTestReportState state, EditItemAction<MaterialTestReportDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static MaterialTestReportState EditMaterialTestReportSuccessAction(MaterialTestReportState state, EditItemSuccessAction<MaterialTestReportDto> action) =>
             state with { IsLoading = false, SelectedItem = action.Item };

        [ReducerMethod]
        public static MaterialTestReportState FailureAction(MaterialTestReportState state, FailureAction _) =>
                state with { IsLoading = false };

        [ReducerMethod]
        public static MaterialTestReportState SetErrorAction(MaterialTestReportState state, SetErrorAction _) =>
            state with { IsLoading = false };
    }
}
