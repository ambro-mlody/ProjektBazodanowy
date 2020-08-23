using GUI.Models;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    class SignUpViewModel : BaseViewModel
    {
		public SignUpViewModel()
		{
			Email = "";
			Password = "";
			ConfirmPassword = "";
		}

		private string email;

		public string Email
		{
			get { return email; }
			set
			{
				email = value;
				OnPropertyChanged();
			}
		}


		private string password;

		public string Password
		{
			get { return password; }
			set
			{
				password = value;
				OnPropertyChanged();
			}
		}

		private string confirmPassword;

		public string ConfirmPassword
		{
			get { return confirmPassword; }
			set
			{
				confirmPassword = value;
				OnPropertyChanged();
			}
		}

		public ICommand SignUpCommand => new Command(
			async () =>
			{
				if(Email != "")
				{
					if (Password == ConfirmPassword && Password != "")
					{
						try
						{
							MailAddress mail = new MailAddress(email);

							((App)Application.Current).MainUser.EmailAddress = Email;

							int userId = await DBConnection.GetUserIdFromEmailAsync();

							if(userId == -1)
							{
								((App)Application.Current).MainUser.Password = Password;

								var code = EmailSender.GenerateCode();

								EmailSender sender = new EmailSender(
									mail,
									"Rejestracja",
									$"Twój kod do rejestracji: {code}");

								await sender.SendMailAsync();

								await Application.Current.MainPage.DisplayAlert("Uwaga!", "Kod do rejestracji został wysłany na twojego emaila.",
								"Ok");

								var viewModel = new ConfirmEmailViewModel(code);
								var detailsPage = new ConfirmEmailPage { BindingContext = viewModel };
								detailsPage.Title = "Potwierdz Email";

								var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
								await navigation.PushAsync(detailsPage, true);
							}
							else
							{
								await Application.Current.MainPage.DisplayAlert("Błąd!", "Ten Email jest już zajęty",
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
						await Application.Current.MainPage.DisplayAlert("Błąd!", "Hasła nie są jednakowe!",
							"Ok");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Błąd!", "Pole Email nie może być puste!",
							"Ok");
				}
			});
	}
}
