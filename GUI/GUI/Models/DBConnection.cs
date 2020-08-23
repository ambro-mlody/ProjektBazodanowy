﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GUI.Models
{
    public static class DBConnection
    {
        public static string ConnectionString { get; set; } = @"Data Source=192.168.8.101\MSSQLSERVEWELL;Initial Catalog=pizzeriaDB;User ID=PizzeriaLogin;Password=Pizzeria";

		public static async Task<int> GetUserIdFromEmailAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand searchEmail = new SqlCommand("EmailExistsSP", conn);

				searchEmail.CommandType = CommandType.StoredProcedure;

				searchEmail.Parameters.Add(new SqlParameter("@email", ((App)Application.Current).MainUser.EmailAddress));

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

		public static async Task CreateUserInDBAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand createUser = new SqlCommand("CreateUserSP", conn);

				createUser.CommandType = CommandType.StoredProcedure;

				createUser.Parameters.Add(new SqlParameter("@email", ((App)Application.Current).MainUser.EmailAddress));
				createUser.Parameters.Add(new SqlParameter("@password", ((App)Application.Current).MainUser.Password));

				await createUser.ExecuteNonQueryAsync();
			}
		}

		public static async Task GetUserDataAsync(int userId)
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

					((App)Application.Current).MainUser = user;
				}
			}
		}

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

		public static async Task StoreFacebookUserAsync(FBProfile profile)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeFbUser = new SqlCommand("StoreFacebookUserSP", conn);

				storeFbUser.CommandType = CommandType.StoredProcedure;

				storeFbUser.Parameters.Add(new SqlParameter("@email", profile.email));
				storeFbUser.Parameters.Add(new SqlParameter("@firstName", profile.first_name));
				storeFbUser.Parameters.Add(new SqlParameter("@lastName", profile.last_name));

				((App)Application.Current).MainUser.EmailAddress = profile.email;
				((App)Application.Current).MainUser.FirstName = profile.first_name;
				((App)Application.Current).MainUser.LastName = profile.last_name;

				await storeFbUser.ExecuteNonQueryAsync();
			}
		}

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

		public static async Task ChangeUserPasswordAsync(string password)
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand changePassword = new SqlCommand("ChangePasswordSP", conn);

				changePassword.CommandType = CommandType.StoredProcedure;

				changePassword.Parameters.Add(new SqlParameter("@id", int.Parse(((App)Application.Current).MainUser.Id)));
				changePassword.Parameters.Add(new SqlParameter("@password", password));

				await changePassword.ExecuteNonQueryAsync();
			}
		}

		public static async Task UpdateUserInDBAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("UpdateUserDataSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", int.Parse(((App)Application.Current).MainUser.Id)));
				storeUser.Parameters.Add(new SqlParameter("@email", ((App)Application.Current).MainUser.EmailAddress));
				storeUser.Parameters.Add(new SqlParameter("@firstName", ((App)Application.Current).MainUser.FirstName));
				storeUser.Parameters.Add(new SqlParameter("@lastName", ((App)Application.Current).MainUser.LastName));
				storeUser.Parameters.Add(new SqlParameter("@phoneNumber", ((App)Application.Current).MainUser.PhoneNumber));

				if (((App)Application.Current).MainUser.Address.Id != "")
				{
					storeUser.Parameters.Add(new SqlParameter("@addressId", int.Parse(((App)Application.Current).MainUser.Address.Id)));
				}
				else
				{
					int id = await CreateAddressDataAsyc();
					storeUser.Parameters.Add(new SqlParameter("@addressId", id));
				}

				storeUser.Parameters.Add(new SqlParameter("@city", ((App)Application.Current).MainUser.Address.City));
				storeUser.Parameters.Add(new SqlParameter("@street", ((App)Application.Current).MainUser.Address.Street));
				storeUser.Parameters.Add(new SqlParameter("@houseNumber", ((App)Application.Current).MainUser.Address.HouseNumber));
				storeUser.Parameters.Add(new SqlParameter("@localNumber", ((App)Application.Current).MainUser.Address.LocalNumber));
				storeUser.Parameters.Add(new SqlParameter("@postCode", ((App)Application.Current).MainUser.Address.PostCode));

				await storeUser.ExecuteNonQueryAsync();
			}
		}

		public static async Task<int> CreateAddressDataAsyc()
		{

			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand createAddress = new SqlCommand("CreateAddressSP", conn);

				createAddress.CommandType = CommandType.StoredProcedure;

				createAddress.Parameters.Add(new SqlParameter("@city", ((App)Application.Current).MainUser.Address.City));
				createAddress.Parameters.Add(new SqlParameter("@street", ((App)Application.Current).MainUser.Address.Street));
				createAddress.Parameters.Add(new SqlParameter("@houseNumber", ((App)Application.Current).MainUser.Address.HouseNumber));
				createAddress.Parameters.Add(new SqlParameter("@localNumber", ((App)Application.Current).MainUser.Address.LocalNumber));
				createAddress.Parameters.Add(new SqlParameter("@postCode", ((App)Application.Current).MainUser.Address.PostCode));

				using (SqlDataReader insertedAddressId = await createAddress.ExecuteReaderAsync())
				{
					insertedAddressId.Read();
					var id = insertedAddressId.GetValue(0).ToString();
					((App)Application.Current).MainUser.Address.Id = id;
					return int.Parse(id);
				}
			}
		}

		public static async Task DeleteUserFromDBAsync()
		{
			using (SqlConnection conn = new SqlConnection(DBConnection.ConnectionString))
			{
				conn.Open();

				SqlCommand storeUser = new SqlCommand("DeleteUserSP", conn);

				storeUser.CommandType = CommandType.StoredProcedure;

				storeUser.Parameters.Add(new SqlParameter("@id", int.Parse(((App)Application.Current).MainUser.Id)));
				storeUser.Parameters.Add(new SqlParameter("@addressId", int.Parse(((App)Application.Current).MainUser.Address.Id)));

				await storeUser.ExecuteNonQueryAsync();
			}
		}
	}
}