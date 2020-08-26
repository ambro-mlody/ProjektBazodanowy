using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace GUI.Behaviors
{
    /// <summary>
    /// Klasa sprawdzająca, by pole wprowadzone przez użytkownika nie było puste.
    /// </summary>
    public class NotEmptyBehavior : BaseValidation
    {
        override protected void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var textValue = args.NewTextValue;
            IsValid = textValue.Length == 0 ? false : true;
        }
    }
}
