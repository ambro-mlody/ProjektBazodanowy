using GUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    class ForgotPasswordViewModel : BaseViewModel
    {

		public ForgotPasswordViewModel(string code)
		{
			newPassword = "";
			ConfirmPassword = "";
			GeneratedCode = code;
		}

		private string GeneratedCode;

		private string code;

		public string Code
		{
			get { return code; }
			set 
			{ 
				code = value;
				OnPropertyChanged();
			}
		}


		private string newPassword;

		public string NewPassword
		{
			get { return newPassword; }
			set
			{
				newPassword = value;
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

		public ICommand ChangePasswordCommand => new Command(
			async () =>
			{
				if(Code == GeneratedCode)
				{
					if (NewPassword == ConfirmPassword && NewPassword != "")
					{
						string password = Hashing.HashPassword(NewPassword);

						var id = int.Parse(((App)Application.Current).MainUser.Id);

						try
						{
							await DBConnection.ChangeUserPasswordAsync(id, password);

							((App)Application.Current).MainUser.Password = password;

							await Application.Current.MainPage.DisplayAlert("Gotowe!", "Twoje konto zostało utwożone! Masz teraz dostęp do wszystkich funkcji :>",
								"Ok");

							LoginAccounPageChanges.ShowAccountPageInMenuSetup();

							((App)Application.Current).MainUser = await DBConnection.GetUserDataAsync(id);

							LoginAccounPageChanges.GoToAccountPage();

						}
						catch (Exception)
						{
							await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
							"oraz że mam włączonego laptopa :>",
							"Ok");
						}

					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Błąd!", "Hasła nie są takie same!",
							"Ok");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Błąd!", "Kod nie jest poprawny!",
							"Ok");
				}
				
			});
	}
}
