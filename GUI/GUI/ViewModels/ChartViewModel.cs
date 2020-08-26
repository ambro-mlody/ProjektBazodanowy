using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using GUI.Models;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
	/// <summary>
	/// Obsługa koszyka.
	/// </summary>
	class ChartViewModel : BaseViewModel
	{
		private ObservableCollection<PizzaInChart> pizzas;

		/// <summary>
		/// Kolekcja pizz dodanych do koszyka.
		/// </summary>
		public ObservableCollection<PizzaInChart> Pizzas
		{
			get { return pizzas; }
			set
			{
				pizzas = value;
				OnPropertyChanged();
			}
		}

		private double price;

		/// <summary>
		/// Cena za cały koszyk.
		/// </summary>
		public double Price
		{
			get { return price; }
			set 
			{
				price = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Dane użytkownika (zamówienia).
		/// </summary>
		public User user { get; set; }

		/// <summary>
		/// Przypisanie wartości znajdujących sie w MainUser.
		/// </summary>
		public ChartViewModel()
		{
			user = new User
			{
				UserChart = ((App)Application.Current).MainUser.UserChart,
				EmailAddress = ((App)Application.Current).MainUser.EmailAddress,
				FirstName = ((App)Application.Current).MainUser.FirstName,
				LastName = ((App)Application.Current).MainUser.LastName,
				PhoneNumber = ((App)Application.Current).MainUser.PhoneNumber,
				Address = new Address
				{
					City = ((App)Application.Current).MainUser.Address.City,
					PostCode = ((App)Application.Current).MainUser.Address.PostCode,
					Street = ((App)Application.Current).MainUser.Address.Street,
					HouseNumber = ((App)Application.Current).MainUser.Address.HouseNumber,
					LocalNumber = ((App)Application.Current).MainUser.Address.LocalNumber
				}	
			};

			Price = 0;
			Pizzas = user.UserChart.Pizzas;
			foreach (var item in Pizzas)
			{
				Price += item.TotalCost;
			}

			
		}

		/// <summary>
		/// Obsługa przycisku zmniejszania ilości danego typu pizzy w koszyku.
		/// </summary>
		public ICommand MinusAmmountCommand => new Command<PizzaInChart>(
			async (PizzaInChart pizza) =>
			{

				if (pizza.Amount > 1)
				{
					int id = Pizzas.IndexOf(pizza);
					pizza.Amount--;
					pizza.TotalCost -= pizza.CostOfOne;
					Pizzas[id] = pizza;
					Price -= pizza.CostOfOne;
				}
				else
				{
					if(Pizzas.Count == 1)
					{
						var res = await Application.Current.MainPage.DisplayAlert("Uwaga!", "Koszyk zostanie usuniety. Czy chcesz kontynuować?",
						"Kontynuuj", "Anuluj");
						if (res)
						{
							Price -= pizza.CostOfOne;
							Pizzas.Remove(pizza);
							var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
							await navigation.PopAsync();
						}
					}
					else
					{
						Price -= pizza.CostOfOne;
						Pizzas.Remove(pizza);
					}
				}	
			});

		/// <summary>
		/// Obsługa przycisku zwiększania ilości danego typu pizzy w koszyku.
		/// </summary>
		public ICommand PlusAmmountCommand => new Command<PizzaInChart>(
			(PizzaInChart pizza) =>
			{
				int id = Pizzas.IndexOf(pizza);
				pizza.Amount++;
				pizza.TotalCost += pizza.CostOfOne;
				Pizzas[id] = pizza;
				Price += pizza.CostOfOne;
			});

		/// <summary>
		/// Obsługa przycisku potwierdzenia zamówienia (pobranie poprawności wypełnionych pól).
		/// </summary>
		public ICommand OrderCommand => new Command<bool>(
			async (bool canExecute) =>
			{
				if(Pizzas.Count == 0)
				{
					await Application.Current.MainPage.DisplayAlert("", "Koszyk nie może być pusty!", "OK");
				}
				else if(canExecute)
				{
					user.UserChart.Price = Price;
					await DBConnection.StoreOrderInfoAsync(user);

					Pizzas.Clear();
					if(!user.Loged)
					{
						user = new User();
					}

					await Application.Current.MainPage.DisplayAlert("", "Zamówienie przyjęte do realizacji", "OK");
					var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
					await navigation.PopAsync();
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("", "Nie wszystkie pola zostały poprawnie wypełnione!", "OK");
				}
				
			});
	}
}
