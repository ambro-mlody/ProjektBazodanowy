using GUI.Models;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static GUI.Views.AppMasterDetailPageMaster;

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

		public async Task StoreUserInDBAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("UpdateUserDataSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", int.Parse(((App)Application.Current).MainUser.Id)));
				storeUser.Parameters.Add(new SqlParameter("@email", Email));
				storeUser.Parameters.Add(new SqlParameter("@firstName", FirstName));
				storeUser.Parameters.Add(new SqlParameter("@lastName", LastName));
				storeUser.Parameters.Add(new SqlParameter("@phoneNumber", PhoneNumber));

				if(((App)Application.Current).MainUser.Address.Id != "")
				{
					storeUser.Parameters.Add(new SqlParameter("@addressId", int.Parse(((App)Application.Current).MainUser.Address.Id)));
				}
				else
				{
					int id = await CreateAddressDataAsyc();
					storeUser.Parameters.Add(new SqlParameter("@addressId", id));
				}

				storeUser.Parameters.Add(new SqlParameter("@city", City));
				storeUser.Parameters.Add(new SqlParameter("@street", Street));
				storeUser.Parameters.Add(new SqlParameter("@houseNumber", HouseNumber));
				storeUser.Parameters.Add(new SqlParameter("@localNumber", LocalNumber));
				storeUser.Parameters.Add(new SqlParameter("@postCode", PostCode));

				await storeUser.ExecuteNonQueryAsync();
			}
		}

		private async Task<int> CreateAddressDataAsyc()
		{

			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand createAddress = new SqlCommand("CreateAddressSP", conn);

				createAddress.CommandType = CommandType.StoredProcedure;

				createAddress.Parameters.Add(new SqlParameter("@city", City));
				createAddress.Parameters.Add(new SqlParameter("@street", Street));
				createAddress.Parameters.Add(new SqlParameter("@houseNumber", HouseNumber));
				createAddress.Parameters.Add(new SqlParameter("@localNumber", LocalNumber));
				createAddress.Parameters.Add(new SqlParameter("@postCode", PostCode));

				using (SqlDataReader insertedAddressId = await createAddress.ExecuteReaderAsync())
				{
					insertedAddressId.Read();
					return int.Parse(insertedAddressId.GetValue(0).ToString());
				}
			}
		}

		private async Task DeleteUserFromDBAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("DeleteUserSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", int.Parse(((App)Application.Current).MainUser.Id)));

				await storeUser.ExecuteNonQueryAsync();
			}
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

		public ICommand CHangePasswordCommand => new Command(
			async () =>
			{
				await Application.Current.MainPage.DisplayAlert("Zmiana!", "Tu będzie zmiana hasła.",
						"Tak", "Nie");
			});

		public ICommand DeleteAccountCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Usuwanie!", "Czy jeses pewien, ze chcesz usunąć konto?",
						"Tak", "Nie");

				if (res)
				{
					await DeleteUserFromDBAsync();

					setUpLogInPage();
				}
				
			});

		public ICommand LogOutCommand => new Command(
			async () =>
			{
				bool res = await Application.Current.MainPage.DisplayAlert("Wylogoywanie!", "Czy jeses pewien, ze chcesz się wylogować?",
						"Tak", "Nie");

				if(res)
				{
					await StoreUserInDBAsync();
					setUpLogInPage();
				}
				

			});

		private void setUpLogInPage()
		{
			((App)Application.Current).MainUser = new User();

			((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
															new LogInPage
															{
																BindingContext = new LogInViewModel(),
																Title = "Moje Konto"
															});

			var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
			view_model.MenuItems[1].TargetType = typeof(LogInPage);
			view_model.MenuItems[1].Title = "Moje Konto";
		}
	}
}
