using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Live;
using TimeTableOne.View.Pages.TablePage.Controls;


namespace TimeTableOne {

	public sealed partial class Account : SettingsFlyout {
		public static readonly DependencyProperty SignInNameProperty =
			DependencyProperty.Register(
				"SignInName",
				typeof (string),
				typeof (Account),
				new PropertyMetadata(null));

		public static readonly DependencyProperty IsSignedInProperty =
			DependencyProperty.Register(
				"IsSignedIn",
				typeof (bool),
				typeof (Account),
				new PropertyMetadata(null));

		public string SignInName {
			get {
				return (string) this.GetValue(SignInNameProperty);
			}
			set {
				this.SetValue(SignInNameProperty, value);
			}
		}

		public bool IsSignedIn {
			get {
				return (bool) this.GetValue(IsSignedInProperty);
			}
			set {
				this.SetValue(IsSignedInProperty, value);
			}
		}

		private AccountViewModel ViewModel {
			get {
				return DataContext as AccountViewModel;
			}
		}

		private static Account _current;

		public static Account Current {
			get {
				return _current;
			}
		}

		public Account() {
			this.InitializeComponent();
			this.DataContext = new AccountViewModel();
			_current = this;
			this.UpdateState();		
		}

		private async void SignInClick(object sender, RoutedEventArgs e) {
			try {
                await OneNoteControl.OneNoteControler.Current.SignIn();
				this.UpdateState();
			}
			catch (LiveConnectException exception) {
				
			}
		}

		private async void SignOutClick(object sender, RoutedEventArgs e) {
			try {
                await OneNoteControl.OneNoteControler.Current.SignOut();
				this.UpdateState();
			}
			catch (LiveConnectException exception) {
				
			}
		}

		public void UpdateState() {
			try {
                this.SignInName = OneNoteControl.OneNoteControler.Current.SignInName;
                this.IsSignedIn = OneNoteControl.OneNoteControler.Current.IsSignedIn;
				if (this.IsSignedIn) {
                    signOutBtn.Visibility = (OneNoteControl.OneNoteControler.Current.AuthClient.CanLogout
						? Visibility.Visible
						: Visibility.Collapsed);
					signInBtn.Visibility = Visibility.Collapsed;
					ViewModel.UserLoginState = "ログイン済み:" + SignInName;
				}
				else {
					signInBtn.Visibility = Visibility.Visible;
					signOutBtn.Visibility = Visibility.Collapsed;
					ViewModel.UserLoginState = "未ログイン";
				}
			}
			catch (LiveConnectException exception) {
				
			}
		}

		private async void Test1_Click(object sender, RoutedEventArgs e) {
			if (await OneNoteControl.OneNoteControler.Current.IsExistNotebook("Test")) {
				int a = 10;
			}
		}

		private void Test2_Click(object sender, RoutedEventArgs e) {

		}
	}


	public class AccountViewModel : BasicViewModel {

		public AccountViewModel() {
			this.UserLoginState = "未ログイン";
		}
		private string _userLoginState;

		public string UserLoginState {
			get {
				return _userLoginState;
			}
			set {
				if (value == _userLoginState) {
					return;
				}
				_userLoginState = value;
				OnPropertyChanged();
			}
		}
	}

}