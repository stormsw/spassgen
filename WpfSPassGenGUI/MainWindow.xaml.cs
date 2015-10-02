using OV.Tools.Generators.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfSPassGenGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PasswordBuilder builder;
        private string password;
        public MainWindow()
        {
            InitializeComponent();
            initializePasswordBuilder();
            passwordGenerated.Text = password;
        }

        private void initializePasswordBuilder()
        {
            builder = new PasswordBuilder();
            builder.SetLength(12);
            bool addDeafultRules = true;
            password = builder.
                AddConditional<SpecialChars>(addDeafultRules, 2).
                AddConditional<Numerals>(addDeafultRules, 4).
                AddConditional<LowAlpha>(addDeafultRules, 3).
                AddConditional<CapsAlpha>(addDeafultRules, 2).Generate();
        }

        private void Button_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                password = builder.Generate();
                /// TODO: create binding
                passwordGenerated.Text = password;
            }
            // TODO: use special PasswordBuilder exception there
            catch (AggregateException e)
            {
                foreach (var ex in e.InnerExceptions)
                {
                    //Console.WriteLine(ex.Message);
                    if (ex is ArgumentException)
                    {
                        //Console.WriteLine("The parameters is invalid, processing is stopped.");
                    }
                }
            }

        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(passwordGenerated.Text);
        }
    }
}
