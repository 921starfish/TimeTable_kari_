using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TimeTableOne.Data;
// add the assembly for the Settings flyout
using Windows.UI.ApplicationSettings;
using System.Threading.Tasks;
using Microsoft.Live;
using TimeTableOne.Utils;


// 空のアプリケーション テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234227 を参照してください

namespace TimeTableOne
{
    /// <summary>
    /// 既定の Application クラスに対してアプリケーション独自の動作を実装します。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        /// 最初の行であり、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために呼び出されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動要求とプロセスの詳細を表示します。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
            // ウィンドウがアクティブであることだけを確認してください
            if (rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                rootFrame = new Frame();
                // 既定の言語を設定します
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 以前中断したアプリケーションから状態を読み込みます。
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // ナビゲーションの履歴スタックが復元されていない場合、最初のページに移動します。
                // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                // 作成します
                rootFrame.Navigate(typeof(TimeTableOne.View.Pages.DayPage.DayPage), e.Arguments);
            }
            // 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate();


			SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。アプリケーションの状態は、
        /// アプリケーションが終了されるのか、メモリの内容がそのままで再開されるのか
        /// わからない状態で保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private  void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
            deferral.Complete();
        }



		protected void onWindowCreated(WindowCreatedEventArgs args) {
			SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
		}

		void onCommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs e) {
			SettingsCommand privacyCommand = new SettingsCommand("privacy", "Privacy", (handler) => {
				Privacy privacyFlyout = new Privacy();
				privacyFlyout.Show();
			});
			e.Request.ApplicationCommands.Add(privacyCommand);
			SettingsCommand accountCommand = new SettingsCommand(
				"account",
				"Account",
				(handler) => {
					Account accountFlyout = new Account();
					accountFlyout.Show();
				});
			e.Request.ApplicationCommands.Add(accountCommand);

            // 星野が追加
            e.Request.ApplicationCommands.Add(new SettingsCommand(
                "Background Setting", "背景を変更", (handler) => ShowCustomSettingFlyout()));
            //ここまで
		}

        public void ShowCustomSettingFlyout()
        {
            TopBackgroundSelect backgroundSettingFlyout = new TopBackgroundSelect();
            backgroundSettingFlyout.Show();
        }

		public static async Task updateUserName(TextBlock userName, Boolean signIn) {
			try {
				// Open Live Connect SDK client.
				LiveAuthClient LCAuth = new LiveAuthClient();
				LiveLoginResult LCLoginResult = await LCAuth.InitializeAsync();
				try {
					LiveLoginResult loginResult = null;
					if (signIn) {
						// Sign in to the user's Microsoft account with the required scope.
						//  
						//  This call will display the Microsoft account sign-in screen if 
						//   the user is not already signed in to their Microsoft account 
						//   through Windows 8.
						// 
						//  This call will also display the consent dialog, if the user has 
						//   has not already given consent to this app to access the data 
						//   described by the scope.
						// 
						//  Change the parameter of LoginAsync to include the scopes 
						//   required by your app.
						loginResult = await LCAuth.LoginAsync(new string[] { "wl.basic" });
					}
					else {
						// If we don't want the user to sign in, continue with the current 
						//  sign-in state.
						loginResult = LCLoginResult;
					}
					if (loginResult.Status == LiveConnectSessionStatus.Connected) {
						// Create a client session to get the profile data.
						LiveConnectClient connect = new LiveConnectClient(LCAuth.Session);

						// Get the profile info of the user.
						LiveOperationResult operationResult = await connect.GetAsync("me");
						dynamic result = operationResult.Result;
						if (result != null) {
							// Update the text of the object passed in to the method. 
							userName.Text = string.Join(" ", "Hello", result.name, "!");
						}
						else {
							// Handle the case where the user name was not returned. 
						}
					}
					else {
						// The user hasn't signed in so display this text 
						//  in place of his or her name.
						userName.Text = "You're not signed in.";
					}
				}
				catch (LiveAuthException exception) {
					// Handle the exception. 
				}
			}
			catch (LiveAuthException exception) {
				// Handle the exception. 
			}
			catch (LiveConnectException exception) {
				// Handle the exception. 
			}
		}

	    private OneNoteControl oneNoteControl = new OneNoteControl();
    }
}
