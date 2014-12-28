﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
				return (string)result.name;
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
		}

		private static readonly string[] Scopes = new[] { "wl.signin", "wl.offline_access", "Office.OneNote_Create" };

        #endregion

        // httpレスポンスを読みやすくする
		private static async Task<StandardResponse> TranslateResponse(HttpResponseMessage response) {
            StandardResponse standardResponse=null;
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

			var createMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("https://www.onenote.com/api/v1.0/notebooks/" + noteID + "/sections")) {
				Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html")
			};

			HttpResponseMessage response = await client.SendAsync(createMessage);

			return await TranslateResponse(response);
		}

		private async Task CreateSection() {
			StandardResponse response = await CreateEmptyPage(notebookID);
			if (response.StatusCode == HttpStatusCode.Created) {
				var successResponse = (CreateSuccessResponse)response;
				clientLink = successResponse.OneNoteClientUrl ?? "No URI";
			}
			else {
				clientLink = string.Empty;
			}
		}

		public async Task<StandardResponse> GetSectionInfo(string sectionName) {
			var client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (IsAuthenticated) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Bearer",
					_authClient.Session.AccessToken);
			}

			var createMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("https://www.onenote.com/api/v1.0/notebooks/id/sections")) {

			};

			HttpResponseMessage response = await client.SendAsync(createMessage);

			return await TranslateResponse(response);
		}





	    private HttpClient GenerateClient()
	    {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (IsAuthenticated)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    _authClient.Session.AccessToken);
            }
	        return client;
	    }

	    private async Task<JsonResponse<GetNotebooksResponse>> GetNotebooks()
        {
            return await StandardResponse.FetchJsonResponse<GetNotebooksResponse>(HttpMethod.Get,"https://www.onenote.com/api/v1.0/notebooks", GenerateClient());
        }

	    private async Task<JsonResponse<GetSectionsResponse>> GetSections()
	    {
            return await StandardResponse.FetchJsonResponse<GetSectionsResponse>(HttpMethod.Get, "https://www.onenote.com/api/v1.0/sections", GenerateClient());
	    }

        private async Task<JsonResponse<GetSectionsResponse>> GetSections(string nextUrl)
        {
            return await StandardResponse.FetchJsonResponse<GetSectionsResponse>(HttpMethod.Get, nextUrl, GenerateClient());
        }

        private async Task OpenNotebook(string tableName)
        {
            var response = await GetNotebooks();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                foreach (var value in response.ResponseData.value)
                {
                    if (value.name == tableName)
                    {
                       var sectionResponse = await GetSections(value.sectionsUrl);
                        break;
                    }
                }
                Debug.WriteLine(response.Content);
            }
            else
            {
              Debug.WriteLine(response.Content);
            }
        }

        public async void OpenNotes(string tableName)
        {
            pageSectionName = tableName;
            await AttemptRefreshToken();
            await OpenNotebook(tableName);
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
				var successResponse = (CreateSuccessResponse)response;
				clientLink = successResponse.OneNoteClientUrl ?? "No URI";
			}
			else {
				clientLink = string.Empty;
			}
		}

		public async void OpenNewSection(string tableNameID) {
			notebookID = tableNameID;
			await AttemptRefreshToken();
			await CreateSection();
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}

		public async void OpenResentSection(string tableNameID) {
			pageSectionName = tableNameID;
			await AttemptRefreshToken();
			await GetSectionName();
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}
	}

}