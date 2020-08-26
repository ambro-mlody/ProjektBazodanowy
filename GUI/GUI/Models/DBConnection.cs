using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GUI.Models
{
	/// <summary>
	/// Klasa odpowiedzialna za łączenie się z bazą danych.
	/// </summary>
    public static class DBConnection
    {
		/// <summary>
		/// Dane serwera, bazy oraz konta.
		/// </summary>
        public static string ConnectionString { get; set; } = @"Data Source=192.168.8.101\MSSQLSERVEWELL;Initial Catalog=pizzeriaDB;User ID=PizzeriaLogin;Password=Pizzeria";

		/// <summary>
		/// Asynchroniczna metoda zwracająca Id użytkownika na podstawie emailu.
		/// </summary>
		/// <param name="email">Email który chcemy znależć w bazie.</param>
		/// <returns>Jeżeli taki adres email znajduje się w bazie: Id użytkownika. W przeciwnym wypadku: -1.</returns>
		public static async Task<int> GetUserIdFromEmailAsync(string email)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand searchEmail = new SqlCommand("EmailExistsSP", conn);

				searchEmail.CommandType = CommandType.StoredProcedure;

				searchEmail.Parameters.Add(new SqlParameter("@email", email));

				using (SqlDataReader userEmail = await searchEmail.ExecuteReaderAsync())
				{
					if (userEmail.HasRows)
					{
						userEmail.Read();
						return (int)userEmail.GetValue(0);
					}
					else
					{
						return -1;
					}
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda tworząca nowego użytkownika w bazie danych.
		/// </summary>
		/// <param name="email">Adres email podany przez użytkownika.</param>
		/// <param name="password">Hasło(hash) użytkownika.</param>
		/// <returns>Id nowo utowrzonego rekordu w bazie.</returns>
		public static async Task<int> CreateUserInDBAsync(string email, string password)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand createUser = new SqlCommand("CreateUserSP", conn);

				createUser.CommandType = CommandType.StoredProcedure;

				createUser.Parameters.Add(new SqlParameter("@email", email));
				createUser.Parameters.Add(new SqlParameter("@password", password));

				using (SqlDataReader userId = await createUser.ExecuteReaderAsync())
				{
					userId.Read();

					return int.Parse(userId.GetValue(0).ToString());
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda zwracająca nowy obiekt User zawierający wsszystkie znajudujące się w bazie dane użytkownika o podanym Id.
		/// </summary>
		/// <param name="userId">Id użytkownika.</param>
		/// <returns>Nowy obiekt klasy User.</returns>
		public static async Task<User> GetUserDataAsync(int userId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getUserData = new SqlCommand("GetUserInfoSP", conn);

				getUserData.CommandType = CommandType.StoredProcedure;

				getUserData.Parameters.Add(new SqlParameter("@userId", userId));

				using (SqlDataReader userData = await getUserData.ExecuteReaderAsync())
				{
					userData.Read();

					User user = new User()
					{
						Loged = true,
						Id = userData.GetValue(0).ToString(),
						EmailAddress = userData.GetValue(1).ToString(),
						Password = userData.GetValue(2).ToString(),
						PhoneNumber = userData.GetValue(4).ToString(),
						FirstName = userData.GetValue(5).ToString(),
						LastName = userData.GetValue(6).ToString(),
						LogedByFacebook = userData.GetValue(7).ToString() == "T" ? true : false
					};

					var address = userData.GetValue(3).ToString();
					var Address = new Address { Id = address };

					if (address != "")
					{
						Address = await GetUserAddressAsync(int.Parse(address));
					}

					user.Address = Address;

					return user;
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda zwracająca adres o podanym Id.
		/// </summary>
		/// <param name="addressId">Id adresu w bazie.</param>
		/// <returns>Nowy obiekt typu Address.</returns>
		public static async Task<Address> GetUserAddressAsync(int addressId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getUserAddress = new SqlCommand("GetUserAddressSP", conn);

				getUserAddress.CommandType = CommandType.StoredProcedure;

				getUserAddress.Parameters.Add(new SqlParameter("@addressId", addressId));

				using (SqlDataReader userAddress = await getUserAddress.ExecuteReaderAsync())
				{
					userAddress.Read();
					return new Address()
					{
						City = userAddress.GetValue(1).ToString(),
						Street = userAddress.GetValue(2).ToString(),
						HouseNumber = userAddress.GetValue(3).ToString(),
						LocalNumber = userAddress.GetValue(4).ToString(),
						PostCode = userAddress.GetValue(5).ToString()
					};
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda dodająca do bazy użytkownika na podstawie danych z facebooka.
		/// </summary>
		/// <param name="profile">Profil użytkownika.</param>
		/// <returns>Id dodanego rekordu</returns>
		public static async Task<int> StoreFacebookUserAsync(FBProfile profile)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeFbUser = new SqlCommand("StoreFacebookUserSP", conn);

				storeFbUser.CommandType = CommandType.StoredProcedure;

				storeFbUser.Parameters.Add(new SqlParameter("@email", profile.email));
				storeFbUser.Parameters.Add(new SqlParameter("@firstName", profile.first_name));
				storeFbUser.Parameters.Add(new SqlParameter("@lastName", profile.last_name));

				using (SqlDataReader userId = await storeFbUser.ExecuteReaderAsync())
				{
					userId.Read();

					return int.Parse(userId.GetString(0));
				}

			}
		}

		/// <summary>
		/// Asynchroniczna metoda pobierająca hsło użytkownika o danym Id.
		/// </summary>
		/// <param name="userId">Id użytkownika.</param>
		/// <returns>Hasło użytkownika.</returns>
		public static async Task<string> GetUserPasswordAsync(int userId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getUserPassword = new SqlCommand("GetUserPasswordSP", conn);

				getUserPassword.CommandType = CommandType.StoredProcedure;

				getUserPassword.Parameters.Add(new SqlParameter("@userId", userId));

				using (SqlDataReader userPassword = await getUserPassword.ExecuteReaderAsync())
				{
					userPassword.Read();
					return userPassword.GetValue(0).ToString();
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda zmieniająca hasło użytkownika o podanym id.
		/// </summary>
		/// <param name="userId">Id użytkownika.</param>
		/// <param name="password">nowe hasło.</param>
		/// <returns></returns>
		public static async Task ChangeUserPasswordAsync(int userId, string password)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand changePassword = new SqlCommand("ChangePasswordSP", conn);

				changePassword.CommandType = CommandType.StoredProcedure;

				changePassword.Parameters.Add(new SqlParameter("@id", userId));
				changePassword.Parameters.Add(new SqlParameter("@password", password));

				await changePassword.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Asynchroniczna metoda zmieniająca dane użytkownika w bazie.
		/// </summary>
		/// <param name="user">Nowe dane.</param>
		/// <returns></returns>
		public static async Task UpdateUserInDBAsync(User user)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("UpdateUserDataSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", int.Parse(user.Id)));
				storeUser.Parameters.Add(new SqlParameter("@email", user.EmailAddress));
				storeUser.Parameters.Add(new SqlParameter("@firstName", user.FirstName));
				storeUser.Parameters.Add(new SqlParameter("@lastName", user.LastName));
				storeUser.Parameters.Add(new SqlParameter("@phoneNumber", user.PhoneNumber));

				if (((App)Application.Current).MainUser.Address.Id != "")
				{
					storeUser.Parameters.Add(new SqlParameter("@addressId", int.Parse(user.Address.Id)));
				}
				else
				{
					int id = await CreateAddressDataAsync(user.Address);
					((App)Application.Current).MainUser.Id = id.ToString();
					storeUser.Parameters.Add(new SqlParameter("@addressId", id));
				}

				storeUser.Parameters.Add(new SqlParameter("@city", user.Address.City));
				storeUser.Parameters.Add(new SqlParameter("@street", user.Address.Street));
				storeUser.Parameters.Add(new SqlParameter("@houseNumber", user.Address.HouseNumber));
				storeUser.Parameters.Add(new SqlParameter("@localNumber", user.Address.LocalNumber));
				storeUser.Parameters.Add(new SqlParameter("@postCode", user.Address.PostCode));

				await storeUser.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Asynchroniczna metoda tworząca nowy rekord adresu w bazie.
		/// </summary>
		/// <param name="address">Dane adresowe.</param>
		/// <returns>Id nowo utworzonego rekordu.</returns>
		public static async Task<int> CreateAddressDataAsync(Address address)
		{

			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand createAddress = new SqlCommand("CreateAddressSP", conn);

				createAddress.CommandType = CommandType.StoredProcedure;

				createAddress.Parameters.Add(new SqlParameter("@city", address.City));
				createAddress.Parameters.Add(new SqlParameter("@street", address.Street));
				createAddress.Parameters.Add(new SqlParameter("@houseNumber", address.HouseNumber));
				createAddress.Parameters.Add(new SqlParameter("@localNumber", address.LocalNumber));
				createAddress.Parameters.Add(new SqlParameter("@postCode", address.PostCode));

				using (SqlDataReader insertedAddressId = await createAddress.ExecuteReaderAsync())
				{
					insertedAddressId.Read();
					var id = insertedAddressId.GetValue(0).ToString();
					return int.Parse(id);
				}
			}
		}

		/// <summary>
		/// Asynchroniczna metoda usuwająca użytkownika o podanym Id z bazy danych.
		/// </summary>
		/// <param name="userId">Id użytkownika do usunięcia</param>
		/// <returns></returns>
		public static async Task DeleteUserFromDBAsync(int userId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("DeleteUserSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", userId));

				await storeUser.ExecuteNonQueryAsync();
			}
		}

		/// <summary>
		/// Asynchroniczna metoda pobierająca daneo pizzach z bazy danych.
		/// </summary>
		/// <returns>Nowa kolekcja obiektów typu PizzaItem zawierająca wszystkie pizze znajdujące się w bazie.</returns>
		public static async Task<ObservableCollection<PizzaItem>> GetPizzasFromDBAsync()
		{
			var pizzas = new ObservableCollection<PizzaItem>();

			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getPizzas = new SqlCommand("GetPizzasSP", conn);

				getPizzas.CommandType = CommandType.StoredProcedure;

				using (SqlDataReader pizzasReader = await getPizzas.ExecuteReaderAsync())
				{
					while(pizzasReader.Read())
					{
						int pizzaId = int.Parse(pizzasReader.GetValue(0).ToString());
						int imageId = int.Parse(pizzasReader.GetValue(5).ToString());

						var pizza = new PizzaItem
						{
							Id = pizzaId,
							Name = pizzasReader.GetValue(1).ToString(),
							Description = pizzasReader.GetValue(2).ToString(),
							Price = double.Parse(pizzasReader.GetValue(3).ToString()),
							PreperationTime = int.Parse(pizzasReader.GetValue(4).ToString()),
							Ingredients = await GetIngredientsInPizzaAsync(pizzaId),
							Image = await GetImageAsync(imageId)
						};

						pizzas.Add(pizza);
					}

				}
			}
			return pizzas;

		}

		/// <summary>
		/// Asynchroniczna funkcja zwracająca obraz pizzy.
		/// </summary>
		/// <param name="imageId">id obrazu(nie pizzy!) w bazie.</param>
		/// <returns>Obiekt typu ImageSource zawierający obraz.</returns>
		public static async Task<ImageSource> GetImageAsync(int imageId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getImage = new SqlCommand("GetImageSP", conn);

				getImage.CommandType = CommandType.StoredProcedure;

				getImage.Parameters.Add(new SqlParameter("@imageId", imageId));

				var img = await getImage.ExecuteScalarAsync();

				return ImageSource.FromStream(() => new MemoryStream((byte[])img));
			}
		}	

		/// <summary>
		/// Asynchroniczna metoda zwracająca wszystkie składniki przypisane do pizzy o podanym Id.
		/// </summary>
		/// <param name="pizzaId">Id pizzy.</param>
		/// <returns>Lista składników (string).</returns>
		public static async Task<List<string>> GetIngredientsInPizzaAsync(int pizzaId)
		{
			var ingredients = new List<string>();

			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand getIngredients = new SqlCommand("GetIngredientsSP", conn);

				getIngredients.CommandType = CommandType.StoredProcedure;

				getIngredients.Parameters.Add(new SqlParameter("@pizzaId", pizzaId));

				using (SqlDataReader ingredientsReader = await getIngredients.ExecuteReaderAsync())
				{
					while (ingredientsReader.Read())
					{
						ingredients.Add(ingredientsReader.GetValue(1).ToString());
					}
				}
			}

			return ingredients;
		}

		/// <summary>
		/// Asynchroniczna metoda zapisująca informacje o zamówieniu w bazie danych.
		/// </summary>
		/// <param name="user">Informacje o zamówieniu w postaci obiektu klasy User.</param>
		/// <returns></returns>
		public static async Task StoreOrderInfoAsync(User user)
		{
			int orderId;
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeOrder = new SqlCommand("StoreOrderInfoSP", conn);

				storeOrder.CommandType = CommandType.StoredProcedure;

				storeOrder.Parameters.Add(new SqlParameter("@email", user.EmailAddress));
				storeOrder.Parameters.Add(new SqlParameter("@firstName", user.FirstName));
				storeOrder.Parameters.Add(new SqlParameter("@lastName", user.LastName));
				storeOrder.Parameters.Add(new SqlParameter("@phoneNumber", user.PhoneNumber));
				storeOrder.Parameters.Add(new SqlParameter("@city", user.Address.City));
				storeOrder.Parameters.Add(new SqlParameter("@street", user.Address.Street));
				storeOrder.Parameters.Add(new SqlParameter("@houseNumber", user.Address.HouseNumber));
				storeOrder.Parameters.Add(new SqlParameter("@localNumber", user.Address.LocalNumber));
				storeOrder.Parameters.Add(new SqlParameter("@postCode", user.Address.PostCode));
				storeOrder.Parameters.Add(new SqlParameter("@cost", user.UserChart.Price));

				orderId = (int)await storeOrder.ExecuteScalarAsync();

			}

			await StorePizzasInOrderAsync(user.UserChart.Pizzas, orderId);
		}

		/// <summary>
		/// Asynchroniczna metoda zapisujaca w bazie informacje o wszystkich pizzach znajdujących się w zamówieniu.
		/// </summary>
		/// <param name="pizzas">Kolekcja pizz w zamówieniu.</param>
		/// <param name="orderId">Id zamówienia.</param>
		/// <returns></returns>
		public static async Task StorePizzasInOrderAsync(ObservableCollection<PizzaInChart> pizzas, int orderId)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				foreach(var pizza in pizzas)
				{
					SqlCommand storePizza = new SqlCommand("StorePizzaInOrderSP", conn);

					storePizza.CommandType = CommandType.StoredProcedure;

					storePizza.Parameters.Add(new SqlParameter("@pizzaId", pizza.Pizza.Id));
					storePizza.Parameters.Add(new SqlParameter("@ammount", pizza.Amount));
					storePizza.Parameters.Add(new SqlParameter("@priceOfOne", pizza.CostOfOne));
					storePizza.Parameters.Add(new SqlParameter("@size", pizza.Size.Size));
					storePizza.Parameters.Add(new SqlParameter("@orderId", orderId));

					await storePizza.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
