using GUI.Models;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GUI.Views
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
            try
            {
                var viewModel = (MainViewModel)BindingContext;
                viewModel.PizzaItems = await DBConnection.GetPizzasFromDBAsync();
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. Upewnij się, że masz połączenie z internetem, " +
                                    "oraz że mam włączonego laptopa :>", "Ok");
            }

            base.OnAppearing();
        }
    }
}
