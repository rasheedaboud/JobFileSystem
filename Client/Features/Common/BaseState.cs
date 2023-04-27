using JobFileSystem.Client.Features.Attachments;
using JobFileSystem.Shared.Interfaces;

namespace JobFileSystem.Client.Features
{
    public record BaseState<T>  where T : new()
    {
        public bool IsLoading { get; init; } = false;
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
        public T SelectedItem { get; init; } 
        public bool HasError { get; init; } = false;
        public string ErrorMessage { get; init; }
    }
}
