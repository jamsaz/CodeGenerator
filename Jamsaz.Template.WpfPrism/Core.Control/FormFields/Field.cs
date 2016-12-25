using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Primitives;
using $safeprojectname$.FormFields.Behaviors;
using $safeprojectname$.FormFields.ListView;

namespace $safeprojectname$.FormFields
{
    public class Field : ContentControl
    {
        public Field()
        {
            var textBoxField = new TextBox();
            textBoxField.SetBinding(System.Windows.Controls.TextBox.TextProperty,
                new Binding("Value") { Source = this, Mode = BindingMode.TwoWay });
            Control = textBoxField;
            Content = textBoxField;
        }

        static Field()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Field), new FrameworkPropertyMetadata(typeof(Field)));
        }

        #region DependencyProperties

        #region LabelText

        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText", typeof(string), typeof(Field), new PropertyMetadata("Label:"));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        #endregion

        #region FiledType

        public static readonly DependencyProperty FieldTypeProperty = DependencyProperty.Register(
            "FieldType", typeof(FieldTypes), typeof(Field),
            new FrameworkPropertyMetadata(FieldTypes.TextBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public FieldTypes FieldType
        {
            get { return (FieldTypes)GetValue(FieldTypeProperty); }
            set { SetValue(FieldTypeProperty, value); }
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(Field),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Control

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
            "Control", typeof(object), typeof(Field), new PropertyMetadata(default(object)));

        public object Control
        {
            get { return (object)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        #endregion

        #region ListObjects Dependencies

        #region ItemSource

        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            "ItemSource", typeof(object), typeof(Field),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public object ItemSource
        {
            get { return (object)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        #endregion

        #region DisplayMemberPath

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
            "DisplayMemberPath", typeof(object), typeof(Field),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public object DisplayMemberPath
        {
            get { return (object)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        #region SelectedValuePath

        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(
            "SelectedValuePath", typeof(object), typeof(Field), new PropertyMetadata(default(object)));

        public object SelectedValuePath
        {
            get { return (object)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode", typeof(SelectionMode), typeof(Field),
            new FrameworkPropertyMetadata(SelectionMode.Multiple, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        #endregion

        #endregion

        #region TextObjects Dependencies

        #region Mask

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
            "Mask", typeof(string), typeof(Field), new PropertyMetadata(default(string)));

        public string Mask
        {
            get { return (string)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        #endregion

        #endregion

        #region ToogleObjects Dependencies

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(
            "GroupName", typeof(string), typeof(Field),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (sender, e) => { }));

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        #endregion

        #endregion

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            var owner = this;
            switch (FieldType)
            {
                case FieldTypes.TextBox:
                    var textBoxField = new TextBox { VerticalAlignment = VerticalAlignment.Center };
                    textBoxField.SetBinding(System.Windows.Controls.TextBox.TextProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = textBoxField;
                    owner.Content = textBoxField;
                    break;
                case FieldTypes.Checkbox:
                    var checkBoxField = new CheckBox { Content = "", VerticalAlignment = VerticalAlignment.Center };
                    checkBoxField.SetBinding(ToggleButton.IsCheckedProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = checkBoxField;
                    owner.Content = checkBoxField;
                    break;
                case FieldTypes.ListView:
                    var listViewField = new ListView.ListView { VerticalAlignment = VerticalAlignment.Center };
                    listViewField.SetBinding(ItemsControl.ItemsSourceProperty,
                        new Binding("ItemSource") { Source = owner, Mode = BindingMode.TwoWay });
                    listViewField.SetBinding(ItemsControl.DisplayMemberPathProperty,
                        new Binding("DisplayMemberPath") { Source = owner, Mode = BindingMode.TwoWay });
                    listViewField.SetBinding(Selector.SelectedValuePathProperty,
                        new Binding("SelectedValuePath") { Source = owner, Mode = BindingMode.TwoWay });
                    listViewField.SetBinding(ItemsControlSelector.SelectedItemProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    listViewField.SetBinding(ItemsControlSelector.SelectionModeProperty,
                        new Binding("SelectionMode") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = listViewField;
                    owner.Content = listViewField;
                    break;
                case FieldTypes.CheckBoxList:
                    var checkBoxListField = new CheckBoxList { VerticalAlignment = VerticalAlignment.Center };
                    checkBoxListField.SetBinding(RadioButtonList.ItemsSourceProperty,
                        new Binding("ItemSource") { Source = owner, Mode = BindingMode.TwoWay });
                    checkBoxListField.SetBinding(RadioButtonList.SelectedValuePathProperty,
                        new Binding("SelectedValuePath") { Source = owner, Mode = BindingMode.TwoWay });
                    checkBoxListField.SetBinding(RadioButtonList.SelectedItemProperty,
                        new Binding("SelectedItem") { Source = owner, Mode = BindingMode.TwoWay });
                    checkBoxListField.SetBinding(RadioButtonList.SelectedValueProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    checkBoxListField.SetBinding(RadioButtonList.SelectionModeProperty,
                        new Binding("SelectionMode") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = checkBoxListField;
                    owner.Content = checkBoxListField;
                    break;
                case FieldTypes.RadioBoxList:
                    var radioBoxListField = new RadioButtonList { VerticalAlignment = VerticalAlignment.Center };
                    radioBoxListField.SetBinding(RadioButtonList.ItemsSourceProperty,
                        new Binding("ItemSource") { Source = owner, Mode = BindingMode.TwoWay });
                    radioBoxListField.SetBinding(RadioButtonList.SelectedValuePathProperty,
                        new Binding("SelectedValuePath") { Source = owner, Mode = BindingMode.TwoWay });
                    radioBoxListField.SetBinding(RadioButtonList.SelectedItemProperty,
                        new Binding("SelectedItem") { Source = owner, Mode = BindingMode.TwoWay });
                    radioBoxListField.SetBinding(RadioButtonList.SelectedValueProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    radioBoxListField.SetBinding(RadioButtonList.SelectionModeProperty,
                        new Binding("SelectionMode") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = radioBoxListField;
                    owner.Content = radioBoxListField;
                    break;
                case FieldTypes.Radiobox:
                    var radioBoxField = new RadioButton { Content = "", VerticalAlignment = VerticalAlignment.Center };
                    radioBoxField.SetBinding(System.Windows.Controls.RadioButton.GroupNameProperty,
                        new Binding("GroupName") { Source = owner, Mode = BindingMode.TwoWay });
                    radioBoxField.SetBinding(ToggleButton.IsCheckedProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = radioBoxField;
                    owner.Content = radioBoxField;
                    break;
                case FieldTypes.Combobox:
                    var comboBoxField = new ComboBox.ComboBox { VerticalAlignment = VerticalAlignment.Center };
                    comboBoxField.SetBinding(ItemsControl.ItemsSourceProperty,
                        new Binding("ItemSource") { Source = owner, Mode = BindingMode.TwoWay });
                    comboBoxField.SetBinding(ItemsControl.DisplayMemberPathProperty,
                        new Binding("DisplayMemberPath") { Source = owner, Mode = BindingMode.TwoWay });
                    comboBoxField.SetBinding(Selector.SelectedValuePathProperty,
                        new Binding("SelectedValuePath") { Source = owner, Mode = BindingMode.TwoWay });
                    comboBoxField.SetBinding(Selector.SelectedItemProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = comboBoxField;
                    owner.Content = comboBoxField;
                    break;
                case FieldTypes.DateTimebox:
                    var datetimeBoxField = new DatePicker { VerticalAlignment = VerticalAlignment.Center };
                    datetimeBoxField.SetBinding(DatePicker.ValueProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = datetimeBoxField;
                    owner.Content = datetimeBoxField;
                    break;
                case FieldTypes.ColorPicker:
                    var colorField = new ColorPicker { VerticalAlignment = VerticalAlignment.Center };
                    colorField.SetBinding(RadColorPicker.SelectedColorProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = colorField;
                    owner.Content = colorField;
                    break;
                case FieldTypes.NumericBox:
                    var numericField = new TextBox { VerticalAlignment = VerticalAlignment.Center };
                    numericField.SetBinding(System.Windows.Controls.TextBox.TextProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    var behaviorNumericBox = new NumericTextBoxBehavior { InputMode = TextBoxInputMode.DecimalInput };
                    Interaction.GetBehaviors(numericField).Add(behaviorNumericBox);
                    owner.Control = numericField;
                    owner.Content = numericField;
                    break;
                case FieldTypes.EmailBox:
                    var emailField = new TextBox { VerticalAlignment = VerticalAlignment.Center };
                    emailField.SetBinding(System.Windows.Controls.TextBox.TextProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    var behaviorEmailBox = new EmailTextBoxBehavior();
                    Interaction.GetBehaviors(emailField).Add(behaviorEmailBox);
                    owner.Control = emailField;
                    owner.Content = emailField;
                    break;
                case FieldTypes.MaskTextBox:
                    var maskField = new RadMaskedTextInput { VerticalAlignment = VerticalAlignment.Center };
                    maskField.SetBinding(RadMaskedInputBase.TextProperty,
                        new Binding("Value") { Source = owner, Mode = BindingMode.TwoWay });
                    maskField.SetBinding(RadMaskedInputBase.MaskProperty,
                        new Binding("Mask") { Source = owner, Mode = BindingMode.TwoWay });
                    owner.Control = maskField;
                    owner.Content = maskField;
                    break;
            }

            base.OnApplyTemplate();
        }

        #endregion

    }
}
