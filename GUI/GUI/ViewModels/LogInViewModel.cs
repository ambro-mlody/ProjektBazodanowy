using GUI.Models;
using GUI.Views;
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
using static GUI.Views.AppMasterDetailPageMaster;

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
			if(e.IsAuthenticated)
			{
				var accessToken = e.Account.Properties["access_token"];
				((App)Application.Current).MainUser.Loged = true;

				var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
				view_model.MenuItems[1].TargetType = typeof(AccountPage);
				view_model.MenuItems[1].Title = "Moje Konto";

				var profile = await getFacebookUserAsync(accessToken);

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
								userData.Read();
								
								User user = new User()
								{
									Loged = true,
									Id = userData.GetValue(0).ToString(),
									EmailAddress = userData.GetValue(1).ToString(),
									Password = userData.GetValue(2).ToString(),
									PhoneNumber = userData.GetValue(4).ToString(),
									FirstName = userData.GetValue(5).ToString(),
									LastName = userData.GetValue(6).ToString(),
									LogedByFacebook = userData.GetValue(7).ToString() == "T" ? true : false,
									Salt = userData.GetValue(8).ToString(),
								};

								var address = userData.GetValue(3).ToString();
								var Address = new Address();
								if (address.Length == 0)
								{
								}
								else
								{
									userData.Close();

									SqlCommand getUserAddress = new SqlCommand("GetUserAddressSP", conn);

									getUserAddress.CommandType = CommandType.StoredProcedure;

									getUserAddress.Parameters.Add(new SqlParameter("@addressId", int.Parse(address)));

									using (SqlDataReader userAddress = await getUserAddress.ExecuteReaderAsync())
									{
										userAddress.Read();
										Address = new Address()
										{
											City = userAddress.GetValue(1).ToString(),
											Street = userAddress.GetValue(2).ToString(),
											HouseNumber = userAddress.GetValue(3).ToString(),
											LocalNumber = userAddress.GetValue(4).ToString(),
											PostCode = userAddress.GetValue(5).ToString()
										};
									}
								}
								user.Address = Address;

								((App)Application.Current).MainUser = user;

								((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
																new AccountPage
																{
																	BindingContext = new AccountViewModel(),
																	Title = "Moje Konto"
																});
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

							((App)Application.Current).MainUser.EmailAddress = profile.email;
							((App)Application.Current).MainUser.FirstName = profile.first_name;
							((App)Application.Current).MainUser.LastName = profile.last_name;

							await storeFbUser.ExecuteNonQueryAsync();

							((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
																new AccountPage
																{
																	BindingContext = new AccountViewModel(),
																	Title = "Moje Konto"
																});
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

						using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
						{
							conn.Open();

							SqlCommand searchEmail = new SqlCommand("EmailExistsSP", conn);

							searchEmail.CommandType = CommandType.StoredProcedure;

							searchEmail.Parameters.Add(new SqlParameter("@email", email));

							using (SqlDataReader userEmail = await searchEmail.ExecuteReaderAsync())
							{
								if (userEmail.HasRows)
								{
									userEmail.Read();
									int userId = (int)userEmail.GetValue(0);
									userEmail.Close();
									SqlCommand getUserPassword = new SqlCommand("GetUserPasswordSP", conn);

									getUserPassword.CommandType = CommandType.StoredProcedure;

									getUserPassword.Parameters.Add(new SqlParameter("@userId", userId));

									using (SqlDataReader userPassword = await getUserPassword.ExecuteReaderAsync())
									{
										userPassword.Read();
										var Password = userPassword.GetValue(0).ToString();
										userPassword.Close();

										if(password == Password) // hashowanie
										{
											var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
											view_model.MenuItems[1].TargetType = typeof(AccountPage);
											view_model.MenuItems[1].Title = "Moje Konto";

											SqlCommand getUserData = new SqlCommand("GetUserInfoSP", conn);

											getUserData.CommandType = CommandType.StoredProcedure;

											getUserData.Parameters.Add(new SqlParameter("@userId", userId));

											using (SqlDataReader userData = await getUserData.ExecuteReaderAsync())
											{
												userData.Read();

												User user = new User()
												{
													Loged = true,
													Id = userData.GetValue(0).ToString(),
													EmailAddress = userData.GetValue(1).ToString(),
													Password = userData.GetValue(2).ToString(),
													PhoneNumber = userData.GetValue(4).ToString(),
													FirstName = userData.GetValue(5).ToString(),
													LastName = userData.GetValue(6).ToString(),
													LogedByFacebook = userData.GetValue(7).ToString() == "T" ? true : false,
													Salt = userData.GetValue(8).ToString(),
												};

												var address = userData.GetValue(3).ToString();
												var Address = new Address() { Id = address };
												if (address.Length == 0)
												{
												}
												else
												{
													userData.Close();

													SqlCommand getUserAddress = new SqlCommand("GetUserAddressSP", conn);

													getUserAddress.CommandType = CommandType.StoredProcedure;

													getUserAddress.Parameters.Add(new SqlParameter("@addressId", int.Parse(address)));

													using (SqlDataReader userAddress = await getUserAddress.ExecuteReaderAsync())
													{
														userAddress.Read();
														Address = new Address()
														{
															Id = userAddress.GetValue(0).ToString(),
															City = userAddress.GetValue(1).ToString(),
															Street = userAddress.GetValue(2).ToString(),
															HouseNumber = userAddress.GetValue(3).ToString(),
															LocalNumber = userAddress.GetValue(4).ToString(),
															PostCode = userAddress.GetValue(5).ToString()
														};
													}
												}
												user.Address = Address;

												((App)Application.Current).MainUser = user;

												((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
																new AccountPage
																{
																	BindingContext = new AccountViewModel(),
																	Title = "Moje Konto"
																});
											}
										}
										else
										{
											await Application.Current.MainPage.DisplayAlert("Uwaga!", "Niepoprawne Hasło.", "Ok");
											password = "";
										}
									}
								}
								else
								{
									await Application.Current.MainPage.DisplayAlert("Uwaga!", "Niepoprawny Email.",
									"Ok");
								}
							}
						}
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
