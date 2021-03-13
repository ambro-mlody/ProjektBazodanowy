using DItalia.Models;
using DItalia.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DItalia.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            if(((App)Application.Current).FirstConnection)
            {
                try
                {
                    ((App)Application.Current).Pizzas = await DBConnection.GetPizzasFromDBAsync();
                    var viewModel = (MainViewModel)BindingContext;
                    viewModel.PizzaItems = ((App)Application.Current).Pizzas;
                    ((App)Application.Current).FirstConnection = false;
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
                                "oraz że mam włączonego laptopa :>",
                        "Ok");
                }
            }
            else
            {
                var viewModel = (MainViewModel)BindingContext;
                viewModel.PizzaItems = ((App)Application.Current).Pizzas;
            }

            base.OnAppearing();
        }
    }
}
