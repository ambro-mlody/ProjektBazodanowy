﻿using DItalia.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DItalia.ViewModels
{
	/// <summary>
	/// Obsługa karty potwoerdzenia adresu email.
	/// </summary>
    class ConfirmEmailViewModel : BaseViewModel
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="code">Kod wysłany na email uzytkownika.</param>
		public ConfirmEmailViewModel(string code)
		{
			GeneratedCode = code;
		}

		private string GeneratedCode;

		private string code;

		/// <summary>
		/// Kod wprowadzany przez użytkownika.
		/// </summary>
		public string Code
		{
			get { return code; }
			set
			{
				code = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Obsługa przycisku potwierdzenia emaila.
		/// </summary>
		public ICommand ConfirmEmailCommand => new Command(
			async () =>
			{
				if (Code == GeneratedCode)
				{
					string password = Hashing.HashPassword(((App)Application.Current).MainUser.Password);
					((App)Application.Current).MainUser.Password = password;
					var email = ((App)Application.Current).MainUser.EmailAddress;

					try
					{
						int userId = await DBConnection.CreateUserInDBAsync(email, password);

						await Application.Current.MainPage.DisplayAlert("Gotowe!", "Twoje konto zostało utwożone! Masz teraz dostęp do wszystkich funkcji :>",
							"Ok");

						LoginAccounPageChanges.ShowAccountPageInMenuSetup();

						((App)Application.Current).MainUser = await DBConnection.GetUserDataAsync(userId);

						((App)Application.Current).MainUser.Id = userId.ToString();

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
					await Application.Current.MainPage.DisplayAlert("Błąd!", "Kod nie jest poprawny!",
							"Ok");
				}
			});
	}
}
