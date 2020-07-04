using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using GUI.Models;
using System.Collections.ObjectModel;

namespace GUI.ViewModels
{
	class ChartViewModel : BaseViewModel
	{
		private ObservableCollection<PizzaInChart> pizzas;

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

		public double Price
		{
			get { return price; }
			set 
			{
				price = value;
				OnPropertyChanged();
			}
		}

		public User user { get; set; }

		public ChartViewModel(ref User user)
		{
			Price = 0;
			Pizzas = user.UserChart.Pizzas;
			foreach (var item in Pizzas)
			{
				Price += item.TotalCost;
			}

			this.user = user;
		}

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

		public ICommand PlusAmmountCommand => new Command<PizzaInChart>(
			(PizzaInChart pizza) =>
			{
				int id = Pizzas.IndexOf(pizza);
				pizza.Amount++;
				pizza.TotalCost += pizza.CostOfOne;
				Pizzas[id] = pizza;
				Price += pizza.CostOfOne;
			});

		public ICommand OrderCommand => new Command<bool>(
			async (bool canExecute) =>
			{
				if(Pizzas.Count == 0)
				{
					await Application.Current.MainPage.DisplayAlert("", "Koszyk nie może być pusty!", "OK");
				}
				else if(canExecute)
				{
					Pizzas.Clear();
					if(!user.Loged)
					{
						user.FirstName = "";
						user.Address = new Address();
						user.EmailAddress = "";
						user.LastName = "";
						user.PhoneNumber = "";
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
