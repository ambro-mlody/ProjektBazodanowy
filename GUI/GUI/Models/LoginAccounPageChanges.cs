using GUI.ViewModels;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static GUI.Views.AppMasterDetailPageMaster;

namespace GUI.Models 
{
	/// <summary>
	/// Statyczna klasa zawierająca metody odpowiedzialne za logikę zmian międdzy kartą użytkownika, a ekranem logowania.
	/// </summary>
    public static class LoginAccounPageChanges
    {
		/// <summary>
		/// Metoda ustawiająca kartę konta użytkownika w menu bocznym.
		/// </summary>
		public static void ShowAccountPageInMenuSetup()
		{
			var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
			view_model.MenuItems[1].TargetType = typeof(AccountPage);
			view_model.MenuItems[1].Title = "Konto";
		}

		/// <summary>
		/// Metoda zmieniająca obecnie wyświetlaną kartę na kartę konta użytkownika.
		/// </summary>
		public static void GoToAccountPage()
		{
			((MasterDetailPage)Application.Current.MainPage).Detail = new NavigationPage(
																new AccountPage
																{
																	BindingContext = new AccountViewModel(),
																	Title = "Konto"
																});
		}

		/// <summary>
		/// Metoda zmieniająca obecnie wyświetlaną kartę na kartę logowania.
		/// </summary>
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

		/// <summary>
		/// Metoda ustawiająca kartę logowania w menu bocznym.
		/// </summary>
		public static void setUpLogInPage()
		{
			var view_model = (AppMasterDetailPageMasterViewModel)((MasterDetailPage)Application.Current.MainPage).Master.BindingContext;
			view_model.MenuItems[1].TargetType = typeof(LogInPage);
			view_model.MenuItems[1].Title = "Konto";
		}
	}
}
