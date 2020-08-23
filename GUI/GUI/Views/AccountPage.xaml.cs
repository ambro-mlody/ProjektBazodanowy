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
                await DBConnection.UpdateUserInDBAsync();
            }

            base.OnDisappearing();
        }
    }
}