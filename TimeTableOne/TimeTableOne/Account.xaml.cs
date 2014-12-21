using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Live;
using TimeTableOne.Utils;


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

		private static Account _current;

		public static Account Current {
			get {
				return _current;
			}
		}

		public Account() {
			this.InitializeComponent();
			_current = this;
			this.UpdateState();
		}

		private async void SignInClick(object sender, RoutedEventArgs e) {
			try {
				await OneNoteControl.Current.SignIn();
				this.UpdateState();
			}
			catch (LiveConnectException exception) {
				Debug.WriteLine(exception.ToString());
			}
		}

		private async void SignOutClick(object sender, RoutedEventArgs e) {
			try {
				await OneNoteControl.Current.SignOut();
				this.UpdateState();
			}
			catch (LiveConnectException exception) {
				Debug.WriteLine(exception.ToString());
			}
		}

		public void UpdateState() {
			try {
				this.SignInName = OneNoteControl.Current.SignInName;
				this.IsSignedIn = OneNoteControl.Current.IsSignedIn;
				if (this.IsSignedIn) {
					signOutBtn.Visibility = (OneNoteControl.Current.AuthClient.CanLogout
						? Visibility.Visible
						: Visibility.Collapsed);
					signInBtn.Visibility = Visibility.Collapsed;
				}
				else {
					signInBtn.Visibility = Visibility.Visible;
					signOutBtn.Visibility = Visibility.Collapsed;
				}
			}
			catch (LiveConnectException exception) {
				Debug.WriteLine(exception.ToString());
			}
		}
	}

}