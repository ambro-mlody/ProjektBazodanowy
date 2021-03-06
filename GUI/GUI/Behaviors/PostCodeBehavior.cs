﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace DItalia.Behaviors
{

    /// <summary>
    /// Klasa sprawdzająca poprawność kodu pocztowego.
    /// </summary>
    public class PostCodeBehavior : BaseValidation
    {
        protected override void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var text = args.NewTextValue;
            IsValid = Regex.IsMatch(text, @"^[0-9]{2}-[0-9]{3}$");
        }
    }
}
