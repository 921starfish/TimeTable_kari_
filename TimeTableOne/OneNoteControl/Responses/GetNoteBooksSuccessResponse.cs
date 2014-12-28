using Newtonsoft.Json;
using OneNoteControl.Responses;

public class GetNoteBooksSuccessResponse
{
    [JsonProperty(PropertyName = "@odata.context")]
    public string context;

    public GetNoteBooksSuccessResponseValueUnit[] value;
}

public class GetNoteBooksSuccessResponseValueUnit
{
    public bool isDefault;

    public string userRole;

    public bool isShared;

    public string sectionsUrl;

    public string sectionGroupsUrl;

    public GetLinksUnit links;

    public string id;

    public string name;

    public string self;

    public string createdBy;

    public string lastModifiedBy;

    public string createdTime;

    public string lastModifiedTime;
}