using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace OneNoteControl.Responses {

	public abstract class StandardResponse {
		public HttpStatusCode StatusCode { get; set; }

		public string CorrelationId { get; set; }

		public string Content { get; set; }

		public static Task<JsonResponse<T>> FetchJsonResponse<T>(HttpRequestMessage message, HttpClient client)
			where T : new() {
			return JsonResponse<T>.FetchResponse<T>(message, client);
		}

		public static Task<JsonResponse<T>> FetchJsonResponse<T>(HttpMethod method, string uri, HttpClient client)
			where T : new() {
				return FetchJsonResponse<T>(new HttpRequestMessage(method, uri), client);
		}
		public static Task<JsonResponse<T>> FetchJsonResponse<T>(HttpMethod method, string uri, string content, HttpClient client)
		   where T : new() {
			return FetchJsonResponse<T>(new HttpRequestMessage(method, uri) {
				Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json")
			}, client);
		}

		public static Task<HtmlResponse> FetchHtmlResponse(HttpRequestMessage message, HttpClient client) {
			return HtmlResponse.FetchHtmlResponse(message, client);
		}

		protected static async void GenerateStandardResponse(StandardResponse result, HttpResponseMessage response) {
			result.Content = await response.Content.ReadAsStringAsync();
			result.StatusCode = response.StatusCode;
			IEnumerable<string> correlationValues;
			if (response.Headers.TryGetValues("X-CorrelationId", out correlationValues)) {
				result.CorrelationId = correlationValues.FirstOrDefault();
			}
		}
	}


	public class HtmlResponse : StandardResponse {
		public string HtmlBody { get; set; }

		public static async Task<HtmlResponse> FetchHtmlResponse(HttpRequestMessage message, HttpClient client) {
			HttpResponseMessage response = await client.SendAsync(message);
			var result = new HtmlResponse();
			GenerateStandardResponse(result, response);
			result.HtmlBody = await response.Content.ReadAsStringAsync();
			return result;
		}
	}


	public class JsonResponse<T> : StandardResponse where T : new() {
		public T ResponseData { get; set; }

		public static async Task<JsonResponse<T>> FetchResponse<T>(HttpRequestMessage request, HttpClient client)
			where T : new() {
			HttpResponseMessage response = await client.SendAsync(request);
			var result = new JsonResponse<T>();
			GenerateStandardResponse(result, response);
			JsonSerializer serializer = new JsonSerializer();
			result.ResponseData =
				serializer.Deserialize<T>(
					new JsonTextReader(new StringReader(await response.Content.ReadAsStringAsync())));
			return result;
		}


	}


	public class LinksUnit {
		public LinksURIUnit oneNoteClientUrl;

		public LinksURIUnit oneNoteWebUrl;
	}


	public class LinksURIUnit {
		public string href;
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


}