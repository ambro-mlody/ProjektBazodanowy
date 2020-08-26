using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace GUI.Behaviors
{
    /// <summary>
    /// Klasa sprawdzająca poprawność numeru telefonu.
    /// </summary>
    class PhoneNumberBehavior : BaseValidation
    {
        protected override void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var textValue = args.NewTextValue;
            IsValid = Regex.IsMatch(textValue, @"^[0-9]+$");
        }
    }
}
