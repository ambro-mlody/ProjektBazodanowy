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

		public ChartViewModel(ref User user)
		{
			Pizzas = user.UserChart.Pizzas;
		}

		public ICommand MinusAmmountCommand => new Command<PizzaInChart>(
			(PizzaInChart pizza) =>
			{
				int id = Pizzas.IndexOf(pizza);
				pizza.Amount--;
				if(pizza.Amount != 0)
				{
					pizza.TotalCost -= pizza.CostOfOne;
					Pizzas[id] = pizza;
				}
				else
				{
					Pizzas.Remove(pizza);
				}
			});

		public ICommand PlusAmmountCommand => new Command<PizzaInChart>(
			(PizzaInChart pizza) =>
			{
				int id = Pizzas.IndexOf(pizza);
				pizza.Amount++;
				pizza.TotalCost += pizza.CostOfOne;
				Pizzas[id] = pizza;
				
			});
	}
}
