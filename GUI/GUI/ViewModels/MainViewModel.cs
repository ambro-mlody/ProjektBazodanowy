using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DItalia.Models;
using DItalia.Views;
using Xamarin.Forms;

namespace DItalia.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        ObservableCollection<PizzaItem> pizzaItems;

        public ObservableCollection<PizzaItem> PizzaItems 
        {
            get
            {
                return pizzaItems;
            } 
            set
            {
                pizzaItems = value;
                OnPropertyChanged();
            }
        }

        private PizzaItem selectedPizza;

        public PizzaItem SelectedPizzaItem
        {
            get { return selectedPizza; }
            set 
            { 
                selectedPizza = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {

        }

        /// <summary>
        /// Testowe pizze.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<PizzaItem> makeSomeTestPizzas()
        {
            return new ObservableCollection<PizzaItem>
            {
                new PizzaItem {Id = 0, Name = "Pizza1", Description = "Description1", Price = 11.99,
                    PreperationTime = 40, Image = "search.png", Ingredients = new List<string>{ "cos1", "cos2", "cos3", "tes1", "test2", "test3" } },
                new PizzaItem {Id = 1, Name = "Pizza2", Description = "Description2", Price = 12.99,
                    PreperationTime = 45, Image = "search.png", Ingredients = new List<string>{ "cos4", "cos5", "cos6" } },
                new PizzaItem {Id = 2, Name = "Pizza3", Description = "Description3", Price = 13.99,
                    PreperationTime = 50, Image = "search.png", Ingredients = new List<string>{ "cos7", "cos8", "cos9" } },
                new PizzaItem {Id = 3, Name = "Pizza4", Description = "Description4", Price = 14.99,
                    PreperationTime = 55, Image = "search.png", Ingredients = new List<string>{ "cos10", "cos11", "cos12" }},
                new PizzaItem {Id = 4, Name = "Pizza5", Description = "Description5", Price = 15.99,
                    PreperationTime = 60, Image = "search.png", Ingredients = new List<string>{ "cos13", "cos14", "cos15" }}
            };
        }

        public ICommand SelectionCommand => new Command(DisplaySelectedPizza);

        private void DisplaySelectedPizza()
        {
            if(SelectedPizzaItem != null)
            {
                var viewModel = new PizzaDetailViewModel { SelectedPizzaItem = selectedPizza, PizzaItems = pizzaItems, 
                    CurrentPrice = SelectedPizzaItem.Price, TotalAmount = SelectedPizzaItem.Price, price = SelectedPizzaItem.Price };
                var detailsPage = new PizzaDetailPage { BindingContext = viewModel };
                detailsPage.Title = SelectedPizzaItem.Name;

                var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                navigation.PushAsync(detailsPage, true);
            }
        }

        public ICommand ShowChartCommand => new Command(
            async () =>
            {
                
                var viewModel = new ChartViewModel();
                var chartPage = new ChartPage { BindingContext = viewModel };
                chartPage.Title = "Koszyk";
                var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                await navigation.PushAsync(chartPage, true);
            });
    }
}
