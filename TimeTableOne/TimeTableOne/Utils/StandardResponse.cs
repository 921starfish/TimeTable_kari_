using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TimeTableOne.Utils {

	/// <summary>
	/// Base class representing a simplified response from a service call 
	/// </summary>
	public abstract class StandardResponse {
		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// Per call identifier that can be logged to diagnose issues with Microsoft support
		/// </summary>
		public string CorrelationId { get; set; }

        public static Task<StandardResponse<T>> GetResponse<T>(HttpRequestMessage message, HttpClient client) where T : new()
        {
            
	        return StandardResponse<T>.GetResponse<T>(message, client);
	    }
	}

    public class StandardResponse<T>where T:new()
    {
        public HttpStatusCode StatusCode { get; set; }

        public string CorrelationId { get; set; }

        public T ResponseData { get; set; }

        public async static Task<StandardResponse<T>> GetResponse<T>(HttpRequestMessage request,HttpClient client)where T:new()
        {
            HttpResponseMessage response = await client.SendAsync(request);
            var result=new StandardResponse<T>();
            result.StatusCode = response.StatusCode;
            IEnumerable<string> correlationValues;
            if (response.Headers.TryGetValues("X-CorrelationId", out correlationValues))
            {
                result.CorrelationId = correlationValues.FirstOrDefault();
            }
            JsonSerializer serializer=new JsonSerializer();
            result.ResponseData =
                serializer.Deserialize<T>(new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
            return result;
        }
    }


	/// <summary>
	/// Class representing standard error from the service
	/// </summary>
	public class StandardErrorResponse : StandardResponse {
		/// <summary>
		/// Error message - intended for developer, not end user
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public StandardErrorResponse() {
			this.StatusCode = HttpStatusCode.InternalServerError;
		}
	}


	/// <summary>
	/// Class representing a successful create call from the service
	/// </summary>
	public class CreateSuccessResponse : StandardResponse {
		/// <summary>
		/// URL to launch OneNote rich client
		/// </summary>
		public string OneNoteClientUrl { get; set; }

		/// <summary>
		/// URL to launch OneNote web experience
		/// </summary>
		public string OneNoteWebUrl { get; set; }
	}



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

}