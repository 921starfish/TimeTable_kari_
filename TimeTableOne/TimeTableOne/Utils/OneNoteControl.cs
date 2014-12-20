using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Live;
using System.Net.Http;
using System.Net.Http.Headers;


namespace TimeTableOne.Utils {

	internal static class OneNoteControl {
		static OneNoteControl() {
			InitializePage();
		}

		//スコープの定義
		private static readonly string[] scopes = new string[] {"wl.signin", "wl.basic", "Office.OneNote_Create"};

		//必要な変数
		private static LiveAuthClient authClient;
		private static LiveConnectClient liveClient;

		//OneNoteのURI
		private static readonly Uri PagesEndPoint = new Uri("https://www.onenote.com/api/v1.0/pages?sectionName=Time%20Table&20One%20Test");

		//ページがロードされた時に再ログイン
		private static async void InitializePage() {
		    try
		    {
		        authClient = new LiveAuthClient();
		        LiveLoginResult loginResult =authClient.InitializeAsync(scopes).GetAwaiter().GetResult();

		        if (loginResult.Status == LiveConnectSessionStatus.Connected)
		        {
		            liveClient = new LiveConnectClient(loginResult.Session);
		        }
		    }
		    catch (LiveAuthException authExp)
		    {
		        Debug.WriteLine(authExp.ToString());
		    }
		    catch (Exception e)
		    {
                Debug.WriteLine(e.ToString());
		    }
		}

		private static async void login() {
			try {
				LiveLoginResult loginResult = await authClient.LoginAsync(scopes);

				//ログインの確認
				if (loginResult.Status == LiveConnectSessionStatus.Connected) {

					liveClient = new LiveConnectClient(loginResult.Session);
					Debug.WriteLine("logged in");
				}

			}
				//とりあえずデバッグログ
			catch (LiveAuthException authExp) {
				Debug.WriteLine(authExp.ToString());
			}
		}

		//適当にHTMLページの作成、OneNoteに出力
		private static async Task CreatePage() {
			try {
				var client = new HttpClient();

				//JSONが返ってくる
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				//認証されていない時
				if (IsAuthenticated) {
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
						"Bearer",
						authClient.Session.AccessToken);
				}


				string date = GetDate();
				string simpleHtml = "<html>" +
				                    "<head>" +
				                    "<title>A simple page created from basic HTML-formatted text on Windows 8.</title>" +
				                    "<meta name=\"created\" content=\"" + date + "\" />" +
				                    "</head>" +
				                    "<body>" +
									"<p>This is a page that Time Table One create test page <i>formatted</i> <b>text</b></p>" +
				                    "</body>" +
				                    "</html>";

				var createMessage = new HttpRequestMessage(HttpMethod.Post, PagesEndPoint) {
					Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html")
				};

				HttpResponseMessage response = await client.SendAsync(createMessage);

				Debug.WriteLine(response.ToString());
			}

			catch (Exception e) {
				Debug.WriteLine(e.ToString());
			}

		}

		private static string GetDate() {
			return DateTime.Now.ToString("o");
		}

		/// <summary>
		/// 認証されているかどうか
		/// </summary>
		public static bool IsAuthenticated {
			get {
				return authClient.Session != null && !string.IsNullOrEmpty(authClient.Session.AccessToken);
			}
		}

		public static async void Open(string tableName) {
		 //	login();
			await CreatePage();
		}

		public static string CreatNewNote(string tableName) {
			return "";
		}
	}

}