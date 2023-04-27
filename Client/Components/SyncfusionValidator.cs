using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Grids;
using System.Net.Http.Json;
namespace JobFileSystem.Client.Features
{
    public class SyncfusionValidator : ComponentBase
    {
        [Inject] protected HttpClient _httpClient { get; set; }

        [Parameter]
        public ValidatorTemplateContext context { get; set; }

        [Parameter]
        public string ValidationUrl { get; set; }
        public bool HasValidationErrors { get; set; }   

        public async Task ValidateAsync() => await HandleValidation();

        private ValidationMessageStore messageStore;

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }


        protected override void OnInitialized()
        {

            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomValidation)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(CustomValidation)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            messageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += async (s, e) =>
                await ValidateRequested(s, e);
            CurrentEditContext.OnFieldChanged += async (s, e) =>
                await ValidateField(s, e);

        }

        protected async Task HandleValidation()
        {

            messageStore.Clear();
            HasValidationErrors = false;
            var response = await _httpClient.PostAsJsonAsync(ValidationUrl, CurrentEditContext.Model, default);

            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

                foreach (var err in errors)
                {

                    messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
                    context.ShowValidationMessage(err.Key, false, string.Concat(err.Value));
                }
                HasValidationErrors = true;
            }
            CurrentEditContext.NotifyValidationStateChanged();
        }

        protected async Task ValidateField(object editContext, FieldChangedEventArgs fieldChangedEventArgs)
        {
            messageStore.Clear(fieldChangedEventArgs.FieldIdentifier);
            await HandleValidation();
        }

        private async Task ValidateRequested(object editContext, ValidationRequestedEventArgs validationEventArgs)
        {
            messageStore.Clear();
            await HandleValidation();
        }

    }
}
