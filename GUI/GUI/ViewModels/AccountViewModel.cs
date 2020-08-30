using GUI.Models;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
	/// <summary>
	/// ViewModel obsługujący kartę konta użytkownika.
	/// </summary>
    class AccountViewModel : BaseViewModel
    {
		/// <summary>
		/// Konstruktor pobirający dane z MainUser.
		/// </summary>
		public AccountViewModel()
		{
			Email = ((App)Application.Current).MainUser.EmailAddress;
			FirstName = ((App)Application.Current).MainUser.FirstName;
			LastName = ((App)Application.Current).MainUser.LastName;
			PhoneNumber = ((App)Application.Current).MainUser.PhoneNumber;
			City = ((App)Application.Current).MainUser.Address.City;
			PostCode = ((App)Application.Current).MainUser.Address.PostCode;
			Street = ((App)Application.Current).MainUser.Address.Street;
			HouseNumber = ((App)Application.Current).MainUser.Address.HouseNumber;
			LocalNumber = ((App)Application.Current).MainUser.Address.LocalNumber;
		}

		private string email;

		/// <summary>
		/// Email użytkownika.
		/// </summary>
		public string Email
		{
			get { return email; }
			set 
			{ 
				email = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.EmailAddress = email;
			}
		}

		private string firstName;

		/// <summary>
		/// Imię.
		/// </summary>
		public string FirstName
		{
			get { return firstName; }
			set 
			{ 
				firstName = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.FirstName = firstName;
			}
		}

		private string lastName;

		/// <summary>
		/// Nazwisko.
		/// </summary>
		public string LastName
		{
			get { return lastName; }
			set 
			{ 
				lastName = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.LastName = lastName;
			}
		}

		private string phoneNumber;

		/// <summary>
		/// Numer telefonu.
		/// </summary>
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set 
			{ 
				phoneNumber = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.PhoneNumber = phoneNumber;
			}
		}

		private string city;

		/// <summary>
		/// Miasto.
		/// </summary>
		public string City
		{
			get { return city; }
			set 
			{ 
				city = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.Address.City = city;
			}
		}

		private string postCode;

		/// <summary>
		/// Kod pocztowy.
		/// </summary>
		public string PostCode
		{
			get { return postCode; }
			set 
			{ 
				postCode = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.Address.PostCode = postCode;
			}
		}

		private string street;

		/// <summary>
		/// Ulica.
		/// </summary>
		public string Street
		{
			get { return street; }
			set 
			{ 
				street = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.Address.Street = street;
			}
		}

		private string houseNumber;

		/// <summary>
		/// Nume domu.
		/// </summary>
		public string HouseNumber
		{
			get { return houseNumber; }
			set 
			{ 
				houseNumber = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.Address.HouseNumber = houseNumber;
			}
		}

		private string localNumber;

		/// <summary>
		/// Numer mieszkania.
		/// </summary>
		public string LocalNumber
		{
			get { return localNumber; }
			set 
			{ 
				localNumber = value;
				OnPropertyChanged();
				((App)Application.Current).MainUser.Address.LocalNumber = localNumber;
			}
		}

		/// <summary>
		/// Obsługa przycisku zmiany hasła (otwarcie karty zmiany hasła).
		/// </summary>
		public ICommand ChangePasswordCommand => new Command(
			async () =>
			{
				var viewModel = new ChangePasswordViewModel();
				var detailsPage = new ChangePasswordPage { BindingContext = viewModel };
				detailsPage.Title = "Zmien Haslo";

				var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
				await navigation.PushAsync(detailsPage, true);
			});

		/// <summary>
		/// Obsługa przycisku usuwania konta użytkownika (wyświetlenie ostrzeżenia, a następnie usuniecie użytkownika z bazy).
		/// </summary>
		public ICommand DeleteAccountCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Usuwanie!", "Czy jeses pewien, ze chcesz usunąć konto?",
						"Tak", "Nie");

				if (res)
				{
					var id = int.Parse(((App)Application.Current).MainUser.Id);
					try
					{
						await DBConnection.DeleteUserFromDBAsync(id);
					}
					catch (Exception)
					{
						await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
							"oraz że mam włączonego laptopa :>",
						"Ok");
					}

					LoginAccounPageChanges.setUpLogInPage();

					LoginAccounPageChanges.GoToLoginPage();
				}
				
			});

		/// <summary>
		/// Obsługa wylogowania użytkownika.
		/// </summary>
		public ICommand LogOutCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Wylogoywanie!", "Czy jeses pewien, ze chcesz się wylogować?",
						"Tak", "Nie");

				if(res)
				{
					try
					{
						await DBConnection.UpdateUserInDBAsync(((App)Application.Current).MainUser);
					}
					catch (Exception)
					{
						await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
							"oraz że mam włączonego laptopa :>",
							"Ok");
					}

					LoginAccounPageChanges.setUpLogInPage();

					LoginAccounPageChanges.GoToLoginPage();
				}
			});
	}
}
