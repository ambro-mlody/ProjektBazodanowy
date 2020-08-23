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
    class ChangePasswordViewModel : BaseViewModel
    {
		public ChangePasswordViewModel()
		{
			newPassword = "";
			ConfirmPassword = "";
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
				if(NewPassword == ConfirmPassword && NewPassword != "")
				{
					string password = Hashing.HashPassword(NewPassword);
					((App)Application.Current).MainUser.Password = password;
					await DBConnection.ChangeUserPasswordAsync(password);

					await Application.Current.MainPage.DisplayAlert("Gotowe!", "Hasło zostało amienione.!",
						"Ok");

					var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
					await navigation.PopAsync();
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Błąd!", "Hasła nie są takie same!",
						"Ok");
				}
			});
	}
}
