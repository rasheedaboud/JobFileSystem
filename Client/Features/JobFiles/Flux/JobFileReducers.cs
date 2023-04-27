using Fluxor;
using JobFileSystem.Shared.JobFiles;
using System.Collections.Generic;

namespace Client.Features.JobFiles
{
    public static class JobFileReducers
    {

        [ReducerMethod]
        public static JobFileState GetAction(JobFileState state, GetItemsAction<JobFileDto> action) =>
            state with { IsLoading = true };

        [ReducerMethod]
        public static JobFileState GetSuccessAction(JobFileState state, GetItemsSuccessAction<JobFileDto> action) =>
             state with { IsLoading = false, Items = action.Items };

        [ReducerMethod]
        public static JobFileState SelectuccessAction(JobFileState state, SelectItemAction<JobFileDto> action) =>
             state with { SelectedItem = action.Item };

        [ReducerMethod]
        public static JobFileState DeSelectuccessAction(JobFileState state, DeSelectItemAction<JobFileDto> action)
                => state with { SelectedItem = new() };

        [ReducerMethod]
        public static JobFileState AddAreaAction(JobFileState state, AddItemAction<JobFileDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static JobFileState AdduccessAction(JobFileState state, AddItemSuccessAction<JobFileDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static JobFileState DeleteAction(JobFileState state, DeleteItemAction<JobFileDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static JobFileState DeleteActionSucess(JobFileState state, DeleteItemSuccessAction<JobFileDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };


        [ReducerMethod]
        public static JobFileState EditAreaAction(JobFileState state, EditItemAction<JobFileDto> _) =>
             state with { IsLoading = true };

        [ReducerMethod]
        public static JobFileState EdituccessAction(JobFileState state, EditItemSuccessAction<JobFileDto> action) =>
             state with { IsLoading = false, SelectedItem = new() };

        [ReducerMethod]
        public static JobFileState FailureAction(JobFileState state, FailureAction _) =>
                state with { IsLoading = false };
    }
}
