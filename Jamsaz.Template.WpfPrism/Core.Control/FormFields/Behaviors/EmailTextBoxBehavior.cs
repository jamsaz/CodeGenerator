using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace $safeprojectname$.FormFields.Behaviors
{
    public class EmailTextBoxBehavior : Behavior<TextBox>
    {
        public EmailTextBoxBehavior()
        {
            _invalid = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;
            AssociatedObject.LostFocus += AssociatedObjectLoastFocus;

            DataObject.AddPastingHandler(AssociatedObject, Pasting);

        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;
            AssociatedObject.LostFocus -= AssociatedObjectLoastFocus;

            DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }

        private void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = (string)e.DataObject.GetData(typeof(string));

                if (IsValidInput(this.GetText(pastedText))) return;
                SystemSounds.Beep.Play();
                e.CancelCommand();
            }
            else
            {
                SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space) return;
            if (IsValidInput(this.GetText(" "))) return;
            SystemSounds.Beep.Play();
            e.Handled = true;
        }

        private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsValidInput(this.GetText(e.Text))) return;
            SystemSounds.Beep.Play();
            e.Handled = true;
        }

        private void AssociatedObjectLoastFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)e.OriginalSource;
            if (IsValidEmail(textbox.Text))
            {
                textbox.BorderBrush = new SolidColorBrush(Colors.Silver);
                textbox.IsValidText = true;
                return;
            }
            SystemSounds.Beep.Play();
            textbox.BorderBrush = new SolidColorBrush(Colors.Red);
            textbox.IsValidText = false;
            e.Handled = true;
        }

        private string GetText(string input)
        {
            var txt = AssociatedObject;

            var selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
                selectionStart = txt.Text.Length;

            var selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
                selectionLength = txt.Text.Length - selectionStart;

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            var caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
                caretIndex = realtext.Length;

            var newtext = realtext.Insert(caretIndex, input);

            return newtext;
        }

        private bool IsValidInput(string input)
        {
            var charachters = new List<char>
            {
                '/',
                ',',
                '"',
                ':',
                ';',
                '!',
                '#',
                '$',
                '%',
                '^',
                '&',
                '*',
                '(',
                ')',
                '-',
                '+',
                '=',
                '|',
                '\\',
                '~',
                '`',
                '\''
            };
            return !input.ToCharArray().Any(ch => charachters.Contains(ch));
        }

        bool _invalid;
        public bool IsValidEmail(string strIn)
        {
            _invalid = false;
            if (string.IsNullOrEmpty(strIn))
                return false;

            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (_invalid)
                return false;

            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            var idn = new IdnMapping();

            var domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
