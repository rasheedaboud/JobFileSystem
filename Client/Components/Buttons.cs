using Syncfusion.Blazor.Navigations;

namespace JobFileSystem.Client.Components
{
    public static class Buttons
    {
        public static string Add => "Add";
        public static string Edit => "Edit";
        public static string Cancel => "Cancel";
        public static string Update => "Update";
        public static string Details => "Details";
        public static string ExcelExport => "Excel Export";
        public static string Delete => "Delete";
        public static string Download => "Download";
        public static string Complete => "Complete";
        public static string Print => "Print";
        public static string Refresh => "Refresh";
    }
    public class Toolbar
    {

        private List<object> _toolBar;

        public Toolbar()
        {
            _toolBar = new();
        }


        public Toolbar Add()
        {
            _toolBar.Add(Buttons.Add);
            return this;
        }
        public Toolbar Cancel()
        {
            _toolBar.Add(Buttons.Cancel);
            return this;
        }
        public Toolbar Update()
        {
            _toolBar.Add(Buttons.Update);
            return this;
        }
        public Toolbar DefaultEdit()
        {
            _toolBar.Add(Buttons.Edit);
            return this;
        }
        public Toolbar Edit()
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Edit, Text = Buttons.Edit, PrefixIcon = "e-edit" });
            return this;
        }
        public Toolbar DefaultDelete()
        {
            _toolBar.Add(Buttons.Delete);
            return this;
        }
        public Toolbar Delete()
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Delete, Text = Buttons.Delete, PrefixIcon = "e-delete" });
            return this;
        }
        public Toolbar Download()
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Download, Text = Buttons.Download, PrefixIcon = "e-download" });
            return this;
        }
        public Toolbar DefaultRefresh()
        {
            _toolBar.Add(Buttons.Refresh);
            return this;
        }
        public Toolbar Refresh(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Refresh, Text = Buttons.Refresh, PrefixIcon = icon ?? "e-refresh" });
            return this;
        }
        public Toolbar MarkComplete(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Complete, Text = Buttons.Complete, PrefixIcon = icon ?? "e-check" });
            return this;
        }
        public Toolbar ExcelExport(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.ExcelExport, Text = Buttons.ExcelExport, PrefixIcon = icon ?? "e-excelexport" });
            return this;
        }
        public Toolbar Print(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Print, Text = Buttons.Print, PrefixIcon = icon ?? "e-print" });
            return this;
        }
        public Toolbar PrintDisabled(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Print, Text = Buttons.Print, PrefixIcon = icon ?? "e-print",Disabled=true });
            return this;
        }
        public Toolbar Details(string icon = null)
        {
            _toolBar.Add(new ItemModel() { Id = Buttons.Details, Text = Buttons.Details, PrefixIcon = icon ?? "e-unorder-list" });
            return this;
        }
        public List<object> Build() => _toolBar ?? new List<object>();
    }
}
