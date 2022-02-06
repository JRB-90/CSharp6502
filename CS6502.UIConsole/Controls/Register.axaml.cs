using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CS6502.Core;

namespace CS6502.UIConsole.Controls
{
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
            valueString = "";
        }

        public static readonly StyledProperty<string> RegisterNameProperty =
            AvaloniaProperty.Register<Register, string>(nameof(RegisterName));

        public static readonly DirectProperty<Register, string> ValueStringProperty =
            AvaloniaProperty.RegisterDirect<Register, string>(
                nameof(ValueString),
                o => o.ValueString,
                (o, v) => o.ValueString = v
            );

        public string RegisterName
        {
            get => GetValue(RegisterNameProperty);
            set => SetValue(RegisterNameProperty, value);
        }

        public string ValueString
        {
            get => valueString;
            set => SetAndRaise(ValueStringProperty, ref valueString, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private string valueString;
    }
}
