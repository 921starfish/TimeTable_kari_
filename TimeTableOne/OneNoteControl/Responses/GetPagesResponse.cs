using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneNoteControl.Responses
{
    public class GetPagesResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context;

        public GetPagesResponseValueUnit[] value;
    }

    public class GetPagesResponseValueUnit
    {
        public string title;

        public string createdByAppId;

        public LinksUnit links;

        public string id;

        public string createdTime;
    }
}
