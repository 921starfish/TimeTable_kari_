using Newtonsoft.Json;


namespace OneNoteControl.Responses {

	internal class GetSectionGroupsResponse {
		[JsonProperty(PropertyName = "@odata.context")] public string context;

		public GetSectionGroupsResponseValueUnit[] value;
	}


	public class GetSectionGroupsResponseValueUnit {
		public string sectionUrl;

		public string sectionGroupsUrl;

		public string id;

		public string name;

		public string self;

		public string createdBy;

		public string lastModifiedBy;

		public string createdTime;

		public string lastModifiedTime;
	}

}