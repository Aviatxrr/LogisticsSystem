using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.Events;


namespace CustomBehaviors
{
    public class EnterPressedBindingBehavior : Behavior<TextBox>
    {
        static EnterPressedBindingBehavior()
        {
            TextProperty.Changed.Subscribe(e =>
            {
                ((EnterPressedBindingBehavior) e.Sender).OnBindingValueChanged();
            });
        }
        

        public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<EnterPressedBindingBehavior, string>(
            "Text", defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.KeyUp += OnLostFocus;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyUp -= OnLostFocus;
            base.OnDetaching();
        }
        
        private void OnLostFocus(object? sender, KeyEventArgs e)
        {
            if (AssociatedObject != null & e.Key == Key.Enter)
                Text = AssociatedObject.Text;
        }
        
        private void OnBindingValueChanged()
        {
            if (AssociatedObject != null)
                AssociatedObject.Text = Text;
        }
    }
}
