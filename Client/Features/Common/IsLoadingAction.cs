namespace Client.Features
{
    public record IsLoadingAction
    {
        public IsLoadingAction(bool isLoading)
        {
            IsLoading = isLoading;
        }
        public bool IsLoading { get; }
    }
}
