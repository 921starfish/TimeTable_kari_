using Newtonsoft.Json;


namespace OneNoteControl.Responses {

	public class GetNotebooksResponse {
		[JsonProperty(PropertyName = "@odata.context")] public string context;

		public GetNotebooksResponseValueUnit[] value;
	}


	public class GetNotebooksResponseValueUnit {
		public bool isDefault;

		public string userRole;

		public bool isShared;

		public string sectionsUrl;

		public string sectionGroupsUrl;

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