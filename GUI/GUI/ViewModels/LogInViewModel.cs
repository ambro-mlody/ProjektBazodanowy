using GUI.Models;
using GUI.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    class LogInViewModel : BaseViewModel
    {
		private string _email;

		public string email
		{
			get 
			{
				return _email;
			}
			set 
			{
				_email = value;
				OnPropertyChanged();
			}
		}

		private string _password;

		public string password
		{
			get 
			{ 
				return _password;
			}
			set 
			{ 
				_password = value;
				OnPropertyChanged();
			}
		}

		public ICommand logInFacebookCommand => new Command(
			() =>
			{
				var auth = new OAuth2Authenticator(
					"3194277703942557",
					"email",
					new Uri("https://www.facebook.com/dialog/oauth/"),
					new Uri("https://www.facebook.com/connect/login_success.html"));

				var presenter = new OAuthLoginPresenter();
				presenter.Login(auth);
				auth.Completed += OnAuthenticationCompleted;
				auth.Error += OnAuthenticationError;
				auth.AllowCancel = true;
			});

		private void OnAuthenticationError(object sender, AuthenticatorErrorEventArgs e)
		{
			OAuth2Authenticator auth2 = (OAuth2Authenticator)sender;
			auth2.ShowErrors = false;
			auth2.OnCancelled();
		}

		private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			if (e.IsAuthenticated)
			{
				var accessToken = e.Account.Properties["access_token"];
				((App)Application.Current).MainUser.Loged = true;

				LoginAccounPageChanges.ShowAccountPageInMenuSetup();

				var profile = await getFacebookUserAsync(accessToken);

				((App)Application.Current).MainUser.EmailAddress = profile.email;

				int userId = await DBConnection.GetUserIdFromEmailAsync();

				if (userId != -1)
				{
					await DBConnection.GetUserDataAsync(userId);

					LoginAccounPageChanges.GoToAccountPage();
				}
				else
				{
					await DBConnection.StoreFacebookUserAsync(profile);

					LoginAccounPageChanges.GoToAccountPage();
				}
			}
		}

		public async Task<FBProfile> getFacebookUserAsync(string accessToken)
		{
			var client = new HttpClient();

			var userJson = await client.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={accessToken}");

			var profile = JsonConvert.DeserializeObject<FBProfile>(userJson);
			return profile;
		}

		

		public ICommand logInCommand => new Command(
			async () =>
			{
				if (email.Length != 0 && password.Length != 0)
				{
					try
					{
						MailAddress mail = new MailAddress(email);

						((App)Application.Current).MainUser.EmailAddress = email;
					}
					catch
					{
						await Application.Current.MainPage.DisplayAlert("Uwaga!", "Niepoprawny Email.",
							"Ok");
					}

					int userId = await DBConnection.GetUserIdFromEmailAsync();

					if(userId != -1)
					{
						string userPassword = await DBConnection.GetUserPasswordAsync(userId);

						if (Hashing.ValidatePassword(password, userPassword))
						{
							LoginAccounPageChanges.ShowAccountPageInMenuSetup();

							await DBConnection.GetUserDataAsync(userId);

							LoginAccounPageChanges.GoToAccountPage();
						}
						else
						{
							await Application.Current.MainPage.DisplayAlert("Uwaga!", "Niepoprawne Hasło.", "Ok");
							password = "";
						}
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Błąd!", "Podany email nie jest powiązany z żadnym kontem!",
																				"Ok");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Uwaga!", "Pola Email oraz Haslo nie moga byc puste.",
						"Ok");

					password = "";
				}
			});

		public ICommand signUpCommand => new Command(
			async () =>
			{
				var viewModel = new SignUpViewModel();
				var detailsPage = new SignUpPage { BindingContext = viewModel };
				detailsPage.Title = "Zarejestruj sie";

				var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
				await navigation.PushAsync(detailsPage, true);
			});

		public ICommand forgotPasswordCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Resetowanie hasla!", "Czy jeses pewien, ze chcesz zresetowac haslo?",
						"Tak", "Nie");

				if (res)
				{
					if(email != "")
					{
						try
						{
							MailAddress mail = new MailAddress(email);

							((App)Application.Current).MainUser.EmailAddress = email;

							int userId = await DBConnection.GetUserIdFromEmailAsync();

							if(userId != -1)
							{
								((App)Application.Current).MainUser.Id = userId.ToString();

								var code = EmailSender.GenerateCode();

								EmailSender sender = new EmailSender(
									mail,
									"Zmiana Hasła",
									$"Twój kod do zmiany hasła: {code}");

								await sender.SendMailAsync();

								await Application.Current.MainPage.DisplayAlert("Uwaga!", "Kod do zmiany hasła został wysłany na twojego emaila.",
								"Ok");

								var viewModel = new ForgotPasswordViewModel(code);
								var detailsPage = new ForgotPasswordPage { BindingContext = viewModel };
								detailsPage.Title = "Zmiana Hasla";

								var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
								await navigation.PushAsync(detailsPage, true);
							}
							else
							{
								await Application.Current.MainPage.DisplayAlert("Błąd!", "Podany email nie jest powiązany z żadnym kontem!",
																				"Ok");
							}
							
						}
						catch
						{
							await Application.Current.MainPage.DisplayAlert("Błąd!", "Email nie jest poprawny!",
							"Ok");
						}
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Błąd!", "Email nie może być pusty!",
						"Ok");
					}
				}
			});

		public LogInViewModel()
		{
			email = "";
			password = "";
		}

	}
}
