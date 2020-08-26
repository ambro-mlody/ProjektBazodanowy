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

						((App)Application.Current).MainUser.Password = password;

						await DBConnection.ChangeUserPasswordAsync(id, password);

						await Application.Current.MainPage.DisplayAlert("Gotowe!", "Hasło zostało zmienione!",
							"Ok");

						LoginAccounPageChanges.ShowAccountPageInMenuSetup();

						

						((App)Application.Current).MainUser = await DBConnection.GetUserDataAsync(id);

						LoginAccounPageChanges.GoToAccountPage();
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
