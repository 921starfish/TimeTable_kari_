using Windows.UI.Xaml;
using Newtonsoft.Json;
using OneNoteControl.Responses;

namespace OneNoteControl.Responses
{
    public class PostNotebooksResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context;

        public bool isDafult;

        public string userRole;

        public bool isShared;

        public string sectionUrl;

        public string sectionsGroupsUrl;

        public LinksUnit links;

        public string id;

        public string name;

        public string self;

        public string createdBy;

        public string lastModifiedBy;

        public string createdTime;

        public string lastModifiedTime;
    }
}
