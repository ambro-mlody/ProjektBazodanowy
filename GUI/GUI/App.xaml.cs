using GUI.Models;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GUI
{
    public partial class App : Application
    {
        public User MainUser;


        public App()
        {
            InitializeComponent();

            MainPage = new Views.AppMasterDetailPage();
            MainUser = new User { Loged = false };

            Device.SetFlags(new string[] { "RadioButton_Experimental" });

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
