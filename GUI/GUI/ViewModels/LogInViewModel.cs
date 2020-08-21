using GUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
			});

		private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			if(e.IsAuthenticated)
			{
				var accessToken = e.Account.Properties["access_token"];
				((App)Application.Current).MainUser.Loged = true;

				var profile = await getFacebookUserAsync(accessToken);
				//((App)Application.Current).MainUser.EmailAddress = profile.email;

				using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
				{
					conn.Open();

					SqlCommand searchEmail = new SqlCommand("EmailExistsSP", conn);

					searchEmail.CommandType = CommandType.StoredProcedure;

					searchEmail.Parameters.Add(new SqlParameter("@email", profile.email));

					using (SqlDataReader userEmail = await searchEmail.ExecuteReaderAsync())
					{
						if (userEmail.HasRows)
						{
							userEmail.Read();
							int userId = (int)userEmail.GetValue(0);
							userEmail.Close();
							SqlCommand getUserData = new SqlCommand("GetUserInfoSP", conn);

							getUserData.CommandType = CommandType.StoredProcedure;

							getUserData.Parameters.Add(new SqlParameter("@userId", userId));

							using (SqlDataReader userData = await getUserData.ExecuteReaderAsync())
							{
								await Application.Current.MainPage.DisplayAlert("Działa?", $"Sprawdzam czy dziala. {userData.GetString(0)}, {userData.GetString(1)}, {userData.GetString(2)}, {userData.GetString(3)}",
								"Tak", "Nie");
							}
						}
						else
						{
							userEmail.Close();
							SqlCommand storeFbUser = new SqlCommand("StoreFacebookUserSP", conn);

							storeFbUser.CommandType = CommandType.StoredProcedure;

							storeFbUser.Parameters.Add(new SqlParameter("@email", profile.email));
							storeFbUser.Parameters.Add(new SqlParameter("@firstName", profile.first_name));
							storeFbUser.Parameters.Add(new SqlParameter("@lastName", profile.last_name));

							await storeFbUser.ExecuteNonQueryAsync();

							await Application.Current.MainPage.DisplayAlert("Działa?", "Sprawdzam czy dziala.",
								"Tak", "Nie");
						}
					}
				}
			}
		}

		public struct fbProfile
		{
			public string email;
			public string first_name;
			public string last_name;
		}

		public async Task<fbProfile> getFacebookUserAsync(string accessToken)
		{
			var client = new HttpClient();

			var userJson = await client.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={accessToken}");

			var profile = JsonConvert.DeserializeObject<fbProfile>(userJson);
			return profile;
		}

		public ICommand forgotPasswordCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Resetowanie hasla!", "Czy jeses pewien, ze chcesz zresetowac haslo?",
						"Tak", "Nie");

				if(res)
				{
					 
				}
			});

		public ICommand logInCommand => new Command(
			async () =>
			{
				
				if (email.Length != 0 && password.Length != 0)
				{
					try
					{
						MailAddress m = new MailAddress(email);
						await Application.Current.MainPage.DisplayAlert("Uwaga!", "Tu bedzie sprawdzanie czy istnieje uzytkownik o " +
							"takim emailu w bazie danych.",
							"Ok");
					}
					catch
					{
						await Application.Current.MainPage.DisplayAlert("Uwaga!", "Niepoprawny Email.",
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

		public LogInViewModel()
		{
			email = "";
			password = "";
		}

	}
}
