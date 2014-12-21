using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Live;
using Newtonsoft.Json;
using Windows.System;


namespace TimeTableOne.Utils {

	public class OneNoteControl {

		private static OneNoteControl _current;

		public static OneNoteControl Current {
			get {
				return _current;
			}
		}

		public OneNoteControl() {
			_current = this;
		}

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

		private static readonly string[] Scopes = new[] {"wl.signin", "wl.offline_access", "Office.OneNote_Create"};

		private static readonly string PagesEndPoint = "https://www.onenote.com/api/v1.0/pages";

		private string pageSectionName = "Quick Notes";

		private string DEFAULT_SECTION_NAME = "Quick Notes";

		// 送るノート名の変更
		public Uri GetPagesEndpoint(string specifiedSectionName) {
			string sectionNameToUse;
			if (specifiedSectionName != null) {
				sectionNameToUse = specifiedSectionName;
			}
			else {
				sectionNameToUse = DEFAULT_SECTION_NAME;
			}
			return new Uri(PagesEndPoint + "/?sectionName=" + sectionNameToUse);
		}

		// 認証されているか
		public bool IsAuthenticated {
			get {
				return _authClient.Session != null && !string.IsNullOrEmpty(_authClient.Session.AccessToken);
			}
		}

		public string SignInName;

		public bool IsSignedIn;

		public string clientLink;

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
			catch (Exception) {
				return null;
			}
		}

		// Authの更新の結果を格納
		private async void UpdateAuthProperties(LiveConnectSessionStatus loginStatus) {
			IsSignedIn = loginStatus == LiveConnectSessionStatus.Connected;
			if (IsSignedIn) {
				SignInName = await RetrieveName();
			}
			else {
				SignInName = UserNotSignedIn;
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
		}

		// 新規ページ作成の制御
		private async Task CreatePage() {
			StandardResponse response = await CreateSimplePage(pageSectionName);
			Debug.WriteLine(
				((int) response.StatusCode).ToString() + ": " +
				response.StatusCode.ToString());
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
		public async Task<StandardResponse> CreateSimplePage(string sectionName) {

			var client = new HttpClient();

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (IsAuthenticated) {
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Bearer",
					_authClient.Session.AccessToken);
			}

			string date = GetDate();
			string simpleHtml = "<html>" +
			                    "<head>" +
			                    "<title>タイトルだよ</title>" +
			                    "<meta name=\"created\" content=\"" + date + "\" />" +
			                    "</head>" +
			                    "<body>" +
			                    "<p>いろいろできそう <i>ななめ</i> <b>太字</b></p>" +
			                    "<p>リンクとか <a href=\"http://www.microsoft.com\">マイクロソフトへ</a></p>" +
			                    "</body>" +
			                    "</html>";

			var createMessage = new HttpRequestMessage(HttpMethod.Post, GetPagesEndpoint(sectionName)) {
				Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html")
			};

			HttpResponseMessage response = await client.SendAsync(createMessage);

			return await TranslateResponse(response);
		}

		private static string GetDate() {
			return DateTime.Now.ToString("o");
		}

		// httpレスポンスを読みやすくする
		// マイクロソフトのStandardResponseを使用
		private static async Task<StandardResponse> TranslateResponse(HttpResponseMessage response) {
			StandardResponse standardResponse;
			if (response.StatusCode == HttpStatusCode.Created) {
				dynamic responseObject = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
				standardResponse = new CreateSuccessResponse {
					StatusCode = response.StatusCode,
					OneNoteClientUrl = responseObject.links.oneNoteClientUrl.href,
					OneNoteWebUrl = responseObject.links.oneNoteWebUrl.href
				};
			}
			else {
				standardResponse = new StandardErrorResponse {
					StatusCode = response.StatusCode,
					Message = await response.Content.ReadAsStringAsync()
				};
			}

			IEnumerable<string> correlationValues;
			if (response.Headers.TryGetValues("X-CorrelationId", out correlationValues)) {
				standardResponse.CorrelationId = correlationValues.FirstOrDefault();
			}

			return standardResponse;
		}


		public async void Open(string tableName) {
			pageSectionName = tableName;
			await AttemptRefreshToken();
			await CreatePage();
			await Launcher.LaunchUriAsync(new Uri(clientLink));
		}
	}

}