using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GUI.Behaviors
{
    public class BaseValidation : Behavior<Entry>
    {

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(NotEmptyBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidPropertyKey, value); }
        }


        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        protected virtual void OnEntryTextChanged(object sender, TextChangedEventArgs args) { }
    }
}
