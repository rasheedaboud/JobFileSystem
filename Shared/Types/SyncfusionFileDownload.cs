using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JobFileSystem.Shared.Types
{
    public class Datum
    {
        [JsonPropertyName("path")]
        public object Path { get; set; }

        [JsonPropertyName("action")]
        public object Action { get; set; }

        [JsonPropertyName("newName")]
        public object NewName { get; set; }

        [JsonPropertyName("names")]
        public object Names { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("previousName")]
        public object PreviousName { get; set; }

        [JsonPropertyName("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("hasChild")]
        public bool HasChild { get; set; }

        [JsonPropertyName("isFile")]
        public bool IsFile { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public object Id { get; set; }

        [JsonPropertyName("filterPath")]
        public string FilterPath { get; set; }

        [JsonPropertyName("filterId")]
        public object FilterId { get; set; }

        [JsonPropertyName("parentId")]
        public object ParentId { get; set; }

        [JsonPropertyName("targetPath")]
        public object TargetPath { get; set; }

        [JsonPropertyName("renameFiles")]
        public object RenameFiles { get; set; }

        [JsonPropertyName("caseSensitive")]
        public bool CaseSensitive { get; set; }

        [JsonPropertyName("searchString")]
        public object SearchString { get; set; }

        [JsonPropertyName("showHiddenItems")]
        public bool ShowHiddenItems { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }

        [JsonPropertyName("targetData")]
        public object TargetData { get; set; }

        [JsonPropertyName("permission")]
        public object Permission { get; set; }
    }

    public class SyncfusionFileDownload
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("names")]
        public string[] Names { get; set; }

        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }
    }
}
