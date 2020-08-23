using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GUI.Models;

namespace GUI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public AppMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new AppMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        public class AppMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AppMasterDetailPageMasterMenuItem> MenuItems { get; set; }

            public AppMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<AppMasterDetailPageMasterMenuItem>(new[]
                {
                    new AppMasterDetailPageMasterMenuItem { Id = 0, Title = "Strona główna", TargetType = typeof(MainPage), 
                        Image = "home.png" },
                    new AppMasterDetailPageMasterMenuItem { Id = 1, Title = "Moje Konto", TargetType = typeof(LogInPage),
                        Image = "account.png"},
                    new AppMasterDetailPageMasterMenuItem { Id = 2, Title = "Restauracje", TargetType = typeof(MainPage),
                        Image = "contact.png"}
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}