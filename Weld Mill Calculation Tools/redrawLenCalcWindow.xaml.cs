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
using System.Windows.Shapes;

namespace Weld_Mill_Calculation_Tools
{
    /// <summary>
    /// Interaction logic for redrawLenCalcWindow.xaml
    /// </summary>
    public partial class redrawLenCalcWindow : Window
    {
        public redrawLenCalcWindow() => InitializeComponent();

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            // coppied from stackoverflow
            //get the textbox that fired the event
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var text = textBox.Text;
            var output = new StringBuilder();
            //use this boolean to determine if the dot already exists
            //in the text so far.
            var dotEncountered = false;
            //loop through all of the text
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (char.IsDigit(c))
                {
                    //append any digit.
                    output.Append(c);
                }
                else if (!dotEncountered && c == '.')
                {
                    //append the first dot encountered
                    output.Append(c);
                    dotEncountered = true;
                }
            }
            var newText = output.ToString();
            textBox.Text = newText;
            //set the caret to the end of text
            textBox.CaretIndex = newText.Length;

        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            // coppied from stackoverflow
            TextBox tb = (TextBox)sender;
            if (tb.Text == "0")
            {
                tb.Text = string.Empty;
            }
            tb.SelectAll();
            //tb.GotFocus -= TextBox_GotFocus; //removes the reset so texts stays on next click
        }



        public static string TextBoxEmptyCheck(string checkThisIsAValidEntry)
        {
            if (string.IsNullOrEmpty(checkThisIsAValidEntry))
            {
                MessageBox.Show("Please fill all entry fields.", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return "0";
            }
            else
            {
                return checkThisIsAValidEntry;
            }
        }

        private void FindRedrawTubeCorrectedWeight_Click(object sender, RoutedEventArgs e)
        {
            redrawLengthOutputBox.Text = string.Empty;  //empty output box

            string fiveFootTubeWeightString = TextBoxEmptyCheck(fiveFootTubeWeight.Text);  //get inputs
            string millOrderTubeWeightString = TextBoxEmptyCheck(millOrderTubeWeight.Text);
            string extraToAddToLengthString = TextBoxEmptyCheck(extraToAddToLength.Text);

            double finalLength = ((Convert.ToDouble(millOrderTubeWeightString)/Convert.ToDouble(fiveFootTubeWeightString))*5.00) + Convert.ToDouble(extraToAddToLengthString);    //gets the final length

            redrawLengthOutputBox.Inlines.Add("Weight corrected redraw footage: "); //outputs the results
            redrawLengthOutputBox.Inlines.Add(Convert.ToString(Math.Round(finalLength, 2)));
            
        }
    }
}
