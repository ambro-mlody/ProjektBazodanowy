using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace GUI.Behaviors
{
    /// <summary>
    /// Klasa odpowiadająca za walidację adresu Email wprowadzanego przez użytkownika.
    /// </summary>
    public class EmailAddressBehavior : BaseValidation
    {
        protected override void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var text = args.NewTextValue;
            try
            {
                MailAddress m = new MailAddress(text);
                IsValid = true;
            }
            catch
            {
                IsValid = false;
            }
        }
    }
}
