using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneNoteControl.Responses
{
    class PostSectionsResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context;

        public bool isDefault;

        public string pagesUrl;

        public string name;

        public string self;

        public string createdBy;

        public string lastModifiedBy;

        public string lastModifiedTime;

        public string id;

        public string createdTime;
    }
}
