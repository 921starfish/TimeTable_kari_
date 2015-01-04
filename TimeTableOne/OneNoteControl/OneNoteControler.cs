using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.Live;
using OneNoteControl.Responses;


namespace OneNoteControl {

	public class OneNoteControler {
		#region singleton

		private static OneNoteControler _current;

		public static OneNoteControler Current {
			get {
				_current = _current ?? new OneNoteControler();
				return _current;
			}
		}

		private OneNoteControler() {
			_current = this;
		}

		#endregion


		#region Authraize

		private const string UserNotSignedIn = "You're not signed in.";

		private LiveAuthClient _authClient;

		public LiveAuthClient AuthClient {
			get {
				if (_authClient == null) {
					_authClient = new LiveAuthClient();
				}
				return _authClient;
			}
		}

		public bool IsAuthenticated {
			get {
				return _authClient.Session != null && !string.IsNullOrEmpty(_authClient.Session.AccessToken);
			}
		}

		public string SignInName;

		public bool IsSignedIn;

		public async Task<LiveLoginResult> SignIn() {

			LiveLoginResult loginResult = await AuthClient.InitializeAsync(Scopes);

			if (loginResult.Status != LiveConnectSessionStatus.Connected) {
				loginResult = await AuthClient.LoginAsync(Scopes);
			}
			UpdateAuthProperties(loginResult.Status);
			return loginResult;
		}

		public async Task SignOut() {
			LiveLoginResult loginResult = await AuthClient.InitializeAsync(Scopes);

			if (loginResult.Status != LiveConnectSessionStatus.NotConnected) {
				AuthClient.Logout();
			}
			UpdateAuthProperties(LiveConnectSessionStatus.NotConnected);
		}

		// サインインボタンを使わずにサインイン。
		// すでにサインインしたことあるのが前提
		public async Task<LiveLoginResult> SilentSignIn() {
			try {
				LiveLoginResult loginResult = await AuthClient.InitializeAsync(Scopes);
				UpdateAuthProperties(loginResult.Status);
				return loginResult;
			}
			catch (Exception e) {
				return null;
			}
		}

		// Authの更新の結果を格納
		private async void UpdateAuthProperties(LiveConnectSessionStatus loginStatus) {
			this.IsSignedIn = loginStatus == LiveConnectSessionStatus.Connected;
			if (this.IsSignedIn) {
				this.SignInName = await RetrieveName();
			}
			else {
				this.SignInName = UserNotSignedIn;
			}
		}

		// 名前の抽出
		private async Task<string> RetrieveName() {
			var lcConnect = new LiveConnectClient(AuthClient.Session);

			LiveOperationResult operationResult = await lcConnect.GetAsync("me");
			dynamic result = operationResult.Result;
			if (result != null) {
				return (string) result.name;
			}
			else {
				throw new InvalidOperationException();
			}
		}

		// トークンをリフレッシュしなければならない
		private async Task AttemptRefreshToken() {
			if (IsSignedIn) {
				LiveLoginResult loginWithRefreshTokenResult = await AuthClient.InitializeAsync(Scopes);
				UpdateAuthProperties(loginWithRefreshTokenResult.Status);
			}
			else {
				await SignIn();
			}
		}

		private static readonly string[] Scopes = new[] {"wl.signin", "wl.offline_access", "office.onenote_update"};

		#endregion


		// httpレスポンスを読みやすくする
		private static async Task<StandardResponse> TranslateResponse(HttpResponseMessage response) {
			StandardResponse standardResponse = null;
			//if (response.StatusCode == HttpStatusCode.Created) {
			//    dynamic responseObject = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
			//    standardResponse = new CreateSuccessResponse {
			//        StatusCode = response.StatusCode,
			//        OneNoteClientUrl = responseObject.links.oneNoteClientUrl.href,
			//        OneNoteWebUrl = responseObject.links.oneNoteWebUrl.href
			//    };
			//}
			//else if (response.StatusCode == HttpStatusCode.OK) {
			//    dynamic responseObject = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
			//    standardResponse = new GetSuccessResponse {
			//        StatusCode = response.StatusCode,
			//        Value = responseObject.value
			//    };
			//}
			//else {
			//    standardResponse = new StandardErrorResponse {
			//        StatusCode = response.StatusCode,
			//        Message = await response.Content.ReadAsStringAsync()
			//    };
			//}

			//IEnumerable<string> correlationValues;
			//if (response.Headers.TryGetValues("X-CorrelationId", out correlationValues)) {
			//    standardResponse.CorrelationId = correlationValues.FirstOrDefault();
			//}

			return standardResponse;
		}

		private string notebookID = "TimeTableOne";

		private string pageSectionName = "Quick Notes";

		private string DEFAULT_SECTION_NAME = "Quick Notes";

		public string clientLink;

		private static string GetDate() {
			return DateTime.Now.ToString("o");
		}

		private static readonly string PagesEndPoint = "https://www.onenote.com/api/v1.0/pages";

		// 送るノート名の変更
		public Uri GetPagesEndpoint(string specifiedSectionName) {
			string sectionNameToUse;
			if (specifiedSectionName != "") {
				sectionNameToUse = specifiedSectionName;
			}
			else {
				sectionNameToUse = DEFAULT_SECTION_NAME;
			}
			return new Uri(PagesEndPoint + "/?sectionName=" + sectionNameToUse);
		}

		// 新規ページ作成の制御
		private async Task CreatePage() {
			StandardResponse response = await CreateEmptyPage(pageSectionName);
			if (response.StatusCode == HttpStatusCode.Created) {
				var successResponse = (CreateSuccessResponse) response;
				clientLink = successResponse.OneNoteClientUrl ?? "No URI";
			}
			else {
				clientLink = string.Empty;
			}
		}

		// 新規ページ作成本体
		// ページにデフォルトで入れておく情報を指定できる
		public async Task<StandardResponse> CreateEmptyPage(string noteID) {
			var client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (IsAuthenticated) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Bearer",
					_authClient.Session.AccessToken);
			}

			string simpleHtml = "<html>" +
			                    "<head>" +
			                    "</head>" +
			                    "<body>" +
			                    "</body>" +
			                    "</html>";

			var createMessage = new HttpRequestMessage(
				HttpMethod.Post,
				new Uri("https://www.onenote.com/api/v1.0/notebooks/" + noteID + "/sections")) {
					Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html")
				};

			HttpResponseMessage response = await client.SendAsync(createMessage);

			return await TranslateResponse(response);
		}

		public async Task<StandardResponse> GetSectionInfo(string sectionName) {
			var client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (IsAuthenticated) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Bearer",
					_authClient.Session.AccessToken);
			}

			var createMessage = new HttpRequestMessage(
				HttpMethod.Get,
				new Uri("https://www.onenote.com/api/v1.0/notebooks/id/sections")) {

				};

			HttpResponseMessage response = await client.SendAsync(createMessage);

			return await TranslateResponse(response);
		}





		private HttpClient GenerateClient() {
			var client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (IsAuthenticated) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Bearer",
					_authClient.Session.AccessToken);
			}
			return client;
		}

		private async Task<JsonResponse<GetNotebooksResponse>> GetNotebooks() {
			return
				await
					StandardResponse.FetchJsonResponse<GetNotebooksResponse>(
						HttpMethod.Get,
						"https://www.onenote.com/api/v1.0/notebooks",
						GenerateClient());
		}

		private async Task<JsonResponse<GetSectionsResponse>> GetSections() {
			return
				await
					StandardResponse.FetchJsonResponse<GetSectionsResponse>(
						HttpMethod.Get,
						"https://www.onenote.com/api/v1.0/sections",
						GenerateClient());
		}

		private async Task<JsonResponse<GetSectionsResponse>> GetSections(string nextUrl) {
			return await StandardResponse.FetchJsonResponse<GetSectionsResponse>(HttpMethod.Get, nextUrl, GenerateClient());
		}

		private async Task<JsonResponse<GetPagesResponse>> GetPages() {
			return
				await
					StandardResponse.FetchJsonResponse<GetPagesResponse>(
						HttpMethod.Get,
						"https://www.onenote.com/api/beta/pages",
						GenerateClient());
		}

		private async Task<JsonResponse<GetPagesResponse>> GetPages(string nextUrl) {
			return await StandardResponse.FetchJsonResponse<GetPagesResponse>(HttpMethod.Get, nextUrl, GenerateClient());
		}

		private async Task<JsonResponse<PostNotebooksResponse>> PostNotebooks(string tableName) {
			string content = "{\"name\": \"" + tableName + "\"}";
			return
				await
					StandardResponse.FetchJsonResponse<PostNotebooksResponse>(
						HttpMethod.Post,
						"https://www.onenote.com/api/v1.0/notebooks",
						content,
						GenerateClient());
		}

		private async Task<JsonResponse<PostSectionsResponse>> PostSections(string nextUrl, string sectionName) {
			string content = "{\"name\": \"" + sectionName + "\"}";
			return
				await StandardResponse.FetchJsonResponse<PostSectionsResponse>(HttpMethod.Post, nextUrl, content, GenerateClient());
		}

		private async Task<JsonResponse<PostPagesResponse>> PostPages(string nextUrl) {
			string simpleHtml = "<html>" +
				"<head>" +
				"</head>" +
				"<body>" +
				"</body>" +
				"</html>";
			var Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html");
			return
				await StandardResponse.FetchJsonResponse<PostPagesResponse>(HttpMethod.Post, nextUrl, Content, GenerateClient());
		}

		private JsonResponse<GetNotebooksResponse> notebookResponse;
		private JsonResponse<GetSectionsResponse> sectionResponse;
		private JsonResponse<GetPagesResponse> pageResponse;

		private string childSectionUrl;

		private async Task OpenNotebook(string tableName) {
			notebookResponse = await GetNotebooks();
			if (notebookResponse.StatusCode == HttpStatusCode.OK) {
				foreach (var value in notebookResponse.ResponseData.value) {
					if (value.name == tableName) {
						childSectionUrl = value.sectionsUrl;
						sectionResponse = await GetSections(value.sectionsUrl);
						break;
					}
				}
			}
			else {
				Debug.WriteLine(notebookResponse.Content);
			}
		}

		private async Task OpenSection(string sectionName) {
			if (sectionResponse.StatusCode == HttpStatusCode.OK) {
				foreach (var value in sectionResponse.ResponseData.value) {
					if (value.name == sectionName) {
						pageResponse = await GetPages("https://www.onenote.com/api/beta/sections/" + value.id + "/pages");
						break;
					}
				}
				if (pageResponse.StatusCode == HttpStatusCode.OK) {
					clientLink = pageResponse.ResponseData.value[0].links.oneNoteClientUrl.href;
				}
				else {
					Debug.WriteLine(pageResponse.Content);
				}
			}
			else {
				Debug.WriteLine(sectionResponse.Content);
			}
		}

		private async Task CreateSection(string sectionName) {
			var response = await PostSections(childSectionUrl, sectionName);
			if (response.StatusCode == HttpStatusCode.Conflict) {
				await OpenSection(sectionName);
			}
			else {
				var pageResponse = await PostPages(response.ResponseData.self + "/pages");
				clientLink= pageResponse.ResponseData.links.oneNoteClientUrl.href;
			}
		}

		public async Task CreateNotebook(string tableName) {
			var response = await PostNotebooks(tableName);
		}

		public async void OpenNotes(string tableName) {
			pageSectionName = tableName;
			await AttemptRefreshToken();
			await OpenNotebook(tableName);
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}

		public async void OpenNewSection(string tableName, string sectionName) {
			pageSectionName = tableName;
			await AttemptRefreshToken();
			await OpenNotebook(tableName);
			await CreateSection(sectionName);
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}

		public async void OpenRecentlySection(string tableName, string sectionName) {
			pageSectionName = tableName;
			await AttemptRefreshToken();
			await OpenNotebook(tableName);
			await OpenSection(sectionName);
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}

		public async Task<bool> IsExistNotebook(string tableName) {
			await AttemptRefreshToken();
			notebookResponse = await GetNotebooks();
			if (notebookResponse.StatusCode == HttpStatusCode.OK) {
				foreach (var value in notebookResponse.ResponseData.value) {
					if (value.name == tableName) {
						return true;
					}
				}
			}
			else {
				return false;
			}
			return false;
		}

		public async Task<bool> ExistsSection(string tableName,string sectionName) {
            // await AttemptRefreshToken();
            // TODO ↑直前にIsExistNotebookがなくても呼べるように。
			notebookResponse = await GetNotebooks();
			if (notebookResponse.StatusCode == HttpStatusCode.OK) {
				foreach (var value in notebookResponse.ResponseData.value) {
					if (value.name == tableName) {
						sectionResponse = await GetSections(value.sectionsUrl);
						if (sectionResponse.StatusCode == HttpStatusCode.OK) {
							foreach (var svalue in sectionResponse.ResponseData.value) {
								if (svalue.name == sectionName) {
									return true;
								}
							}
						}
					}
				}
			}
			else {
				return false;
			}
			return false;
		}


		public async void Open(string tableName) {
			pageSectionName = tableName;
			await AttemptRefreshToken();
			await CreatePage();
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}

		private async Task GetSectionName() {
			StandardResponse response = await GetSectionInfo(pageSectionName);
			if (response.StatusCode == HttpStatusCode.Created) {
				var successResponse = (CreateSuccessResponse) response;
				clientLink = successResponse.OneNoteClientUrl ?? "No URI";
			}
			else {
				clientLink = string.Empty;
			}
		}
	}

}