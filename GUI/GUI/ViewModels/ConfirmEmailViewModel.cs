using GUI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    class ConfirmEmailViewModel : BaseViewModel
    {
		public ConfirmEmailViewModel(string code)
		{
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

		public ICommand ConfirmEmailCommand => new Command(
			async () =>
			{
				if (Code == GeneratedCode)
				{
					string password = Hashing.HashPassword(((App)Application.Current).MainUser.Password);
					((App)Application.Current).MainUser.Password = password;

					await Application.Current.MainPage.DisplayAlert("Gotowe!", "Twoje konto zostało utwożone! Masz teraz dostęp do wszystkich funkcji :>",
							"Ok");

					await DBConnection.CreateUserInDBAsync();

					LoginAccounPageChanges.ShowAccountPageInMenuSetup();

					await DBConnection.GetUserDataAsync(int.Parse(((App)Application.Current).MainUser.Id));

					LoginAccounPageChanges.GoToAccountPage();
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Błąd!", "Kod nie jest poprawny!",
							"Ok");
				}
			});
	}
}
