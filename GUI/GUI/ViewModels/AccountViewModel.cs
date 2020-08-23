using GUI.Models;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    class AccountViewModel : BaseViewModel
    {

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

		public ICommand ChangePasswordCommand => new Command(
			async () =>
			{
				var viewModel = new ChangePasswordViewModel();
				var detailsPage = new ChangePasswordPage { BindingContext = viewModel };
				detailsPage.Title = "Zmien Haslo";

				var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
				await navigation.PushAsync(detailsPage, true);
			});

		public ICommand DeleteAccountCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Usuwanie!", "Czy jeses pewien, ze chcesz usunąć konto?",
						"Tak", "Nie");

				if (res)
				{
					await DBConnection.DeleteUserFromDBAsync();

					LoginAccounPageChanges.setUpLogInPage();

					LoginAccounPageChanges.GoToLoginPage();
				}
				
			});

		public ICommand LogOutCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Wylogoywanie!", "Czy jeses pewien, ze chcesz się wylogować?",
						"Tak", "Nie");

				if(res)
				{
					await DBConnection.UpdateUserInDBAsync();

					LoginAccounPageChanges.setUpLogInPage();

					LoginAccounPageChanges.GoToLoginPage();
				}
			});
	}
}
