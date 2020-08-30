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
	/// <summary>
	/// Klasa obsługująca kartę zmiany hasła.
	/// </summary>
    class ChangePasswordViewModel : BaseViewModel
    {
		/// <summary>
		/// Ustawienie pól na puste.
		/// </summary>
		public ChangePasswordViewModel()
		{
			newPassword = "";
			ConfirmPassword = "";
		}

		private string newPassword;

		/// <summary>
		/// Nowe hasło wprowadzone przez użytkownika.
		/// </summary>
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

		/// <summary>
		/// Powtórzenie hasła.
		/// </summary>
		public string ConfirmPassword
		{
			get { return confirmPassword; }
			set 
			{
				confirmPassword = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Obsługa zmiany hasła (pola NewPassword oraz ConfirmPassword nie mogą być puste, oraz muszą być takie same).
		/// </summary>
		public ICommand ChangePasswordCommand => new Command(
			async () =>
			{ 
				if(NewPassword == ConfirmPassword && NewPassword != "")
				{
					string password = Hashing.HashPassword(NewPassword);
					((App)Application.Current).MainUser.Password = password;
					var id = int.Parse(((App)Application.Current).MainUser.Id);

					try
					{
						await DBConnection.ChangeUserPasswordAsync(id, password);
						await Application.Current.MainPage.DisplayAlert("Gotowe!", "Hasło zostało amienione.!",
						"Ok");
					}
					catch (Exception)
					{
						await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
							"oraz że mam włączonego laptopa :>",
						"Ok");
					}

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
