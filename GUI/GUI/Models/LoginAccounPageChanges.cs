using GUI.ViewModels;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static GUI.Views.AppMasterDetailPageMaster;

namespace GUI.Models
{
    public static class LoginAccounPageChanges
    {
		public static void ShowAccountPageInMenuSetup()
		{
			var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
			view_model.MenuItems[1].TargetType = typeof(AccountPage);
			view_model.MenuItems[1].Title = "Konto";
		}

		public static void GoToAccountPage()
		{
			((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
																new AccountPage
																{
																	BindingContext = new AccountViewModel(),
																	Title = "Konto"
																});
		}

		public static void GoToLoginPage()
		{
			((App)Application.Current).MainUser = new User();

			((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
															new LogInPage
															{
																BindingContext = new LogInViewModel(),
																Title = "Konto"
															});
		}

		public static void setUpLogInPage()
		{
			var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
			view_model.MenuItems[1].TargetType = typeof(LogInPage);
			view_model.MenuItems[1].Title = "Konto";
		}
	}
}
