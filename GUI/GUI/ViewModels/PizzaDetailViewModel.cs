using GUI.Models;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GUI.ViewModels
{
    public enum Size
    {
        small = 24,
        normal = 32,
        large = 40
    }

    class PizzaDetailViewModel : BaseViewModel
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

        private double currentPrice;

        public double CurrentPrice
        {
            get { return currentPrice; }
            set 
            { 
                currentPrice = value;
                totalAmount = value * Amount;
                OnPropertyChanged();
            }
        }

        private int amount;

        public int Amount
        {
            get { return amount; }
            set 
            { 
                amount = value;
                OnPropertyChanged();
            }
        }

        private double totalAmount;

        public double TotalAmount
        {
            get { return totalAmount; }
            set 
            {
                totalAmount = value;
                OnPropertyChanged();
            }
        }

        private SizeStruct currentSize;

        public SizeStruct CurrentSize
        {
            get { return currentSize; }
            set 
            { 
                currentSize = value;
                OnPropertyChanged();
            }
        }

        public ICommand MinusPizzaCommand => new Command(
            () => 
            { 
                if(Amount > 1)
                {
                    Amount--;
                    TotalAmount = Amount * CurrentPrice;
                }
            });

        public ICommand PlusPizzaCommand => new Command(
            () =>
            {
                Amount++;
                TotalAmount = Amount * CurrentPrice;
            });

        public ICommand ChoseSizeCommand => new Command(
            () =>
            {
                var sizePage = new ChoseSizePage { BindingContext = this };
                sizePage.Title = "Rozmiar";

                var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                navigation.PushAsync(sizePage, true);
            });

        private double _price;

        public double price
        {
            get { return _price; }
            set 
            { 
                _price = value;
                OnPropertyChanged();
            }
        }

        public SizeStruct _size;

        public ICommand SizeChangedCommand => new Command<string>(
            (string arg) =>
            {
                int size = int.Parse(arg);
                _size.Size = size;
                if(size == (int)Size.small)
                {
                    _size.Name = "Mała";
                    price = SelectedPizzaItem.Price * .9;
                }
                else if(size == (int)Size.normal)
                {
                    _size.Name = "Średnia";
                    price = SelectedPizzaItem.Price;
                }
                else
                {
                    _size.Name = "Duża";
                    price = SelectedPizzaItem.Price * 1.1;
                }
            });

        public ICommand ConfirmChoiseCommand => new Command(
            () =>
            {
                CurrentPrice = price;
                CurrentSize = _size;
                TotalAmount = price * Amount;
                var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                navigation.PopAsync();

            });

        private int id = 0;

        public ICommand AddToChartCommand => new Command(
            async () =>
            {
                ((App)Application.Current).MainUser.UserChart.Pizzas.Add(new PizzaInChart
                {
                    Amount = Amount,
                    Pizza = SelectedPizzaItem,
                    Size = CurrentSize,
                    TotalCost = TotalAmount,
                    CostOfOne = CurrentPrice,
                    Id = id
                });

                id++;

                ((App)Application.Current).MainUser.UserChart.Price += TotalAmount;
                var res = await Application.Current.MainPage.DisplayAlert("", "Dodaliśmy produkt do koszyka", "Kontynuuj",
                    "Koszyk");
                if(res)
                {
                    var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                    await navigation.PopAsync();
                }
                else
                {
                    var navigation = ((MasterDetailPage)Application.Current.MainPage).Detail as NavigationPage;
                    await navigation.PopAsync();
                    var viewModel = new ChartViewModel();
                    var chartPage = new ChartPage { BindingContext = viewModel };
                    chartPage.Title = "Koszyk";

                    await navigation.PushAsync(chartPage, true);
                }
            });

        public PizzaDetailViewModel()
        {
            Amount = 1;
            CurrentSize = new SizeStruct { Size = (int)Size.normal, Name = "Średnia" };
            _size = CurrentSize;
        }

        
    }
}
