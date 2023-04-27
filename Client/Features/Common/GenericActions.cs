using JobFileSystem.Shared.Attachments;
using JobFileSystem.Shared.Interfaces;
using System.Collections.Generic;

namespace Client.Features
{
    public record AttachmentAddedSuccessAction<T> where T : class, IId
    {
        public AttachmentAddedSuccessAction(T selectedItem)
        {
            SelectedItem = selectedItem;
        }

        public T SelectedItem { get; }
    }
    public record AttachmentAddedAction<T> where T : class, IId
    {
        public AttachmentAddedAction(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }

    public record SetSelectedItemAsNewAction<T> where T : class, IId
    {

    }
    public record SetSelectedItemAsNewSuccessAction<T> where T : class, IId
    {

    }
    public record DeSelectItemAction<T> where T : IId { }
    public record SelectItemAction<T> where T : IId
    {
        public SelectItemAction(T item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
        public T Item { get; }
    }

    public record DeleteItemAction<T> where T : IId
    {
        public DeleteItemAction(string id, string parentId = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            ParentId = parentId;
        }
        public string Id { get; }
        public string ParentId { get; }
    }

    public record DeleteItemSuccessAction<T>
    {
        public DeleteItemSuccessAction(bool success, string id = null)
        {
            Success = success;
            Id = id;
        }
        public bool Success { get; }
        public string Id { get; }
    }

    public record AddItemSuccessAction
    {

    }
    public record AddItemSuccessAction<T> where T : IId
    {
        public AddItemSuccessAction(T item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
        public T Item { get; }
    }

    public record AddItemAction<T> where T : IId
    {
        public AddItemAction(T item, string parentId = null)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            ParentId = parentId;
        }
        public T Item { get; }
        public string ParentId { get; }
    }
    public record EditItemSuccessAction
    {
    }
    public record EditItemSuccessAction<T> where T : IId
    {
        public EditItemSuccessAction(T item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }
        public T Item { get; }
    }

    public record EditItemAction<T> where T : IId
    {
        public EditItemAction(T item, string parentId = null)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            ParentId = parentId;
        }
        public T Item { get; }
        public string ParentId { get; }
    }

    public record GetItemsSuccessAction<T>
    {
        public GetItemsSuccessAction(IReadOnlyList<T> items)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
        public IReadOnlyList<T> Items { get; }
    }

    public record GetItemsAction<T>
    {
    }
}
