using GUI.Models;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            BindingContext = new AccountViewModel();
        }

        protected async override void OnDisappearing()
        {
            if(((App)Application.Current).MainUser.Loged)
            {
                try
                {
                    await DBConnection.UpdateUserInDBAsync(((App)Application.Current).MainUser);
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Brak połączenia!", "Nie udało się połączyć z bazą danych. upewnij się, że masz połączenie z internetem," +
                                    "oraz, że mam włączonego laptopa :>", "Ok");
                }
            }

            base.OnDisappearing();
        }
    }
}