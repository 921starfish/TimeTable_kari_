using Newtonsoft.Json;

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

    public GetNoteBooksSuccessLinksUnit links;

    public string id;

    public string name;

    public string self;

    public string createdBy;

    public string lastModifiedBy;

    public string createdTime;

    public string lastModifiedTime;
}

public class GetNoteBooksSuccessLinksUnit
{
    public GetNoteBoolsSuccessURIUnit oneNoteClientUrl;

    public GetNoteBoolsSuccessURIUnit oneNoteWebUrl;
}

public class GetNoteBoolsSuccessURIUnit
{
    public string href;
}