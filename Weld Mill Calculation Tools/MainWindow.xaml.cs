using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Weld_Mill_Calculation_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public void calcCoilLeght(double stripWallNumberFromBox, double stripWidthNumberFromBox, double coilMeasureNumberFromBox, double millSpeedText = 0)
        {
            double defaultSteelDensity = 0.289;    //lb/in3 standard for 304 steel
            int defaultCoilInnerDiameter = 20; //standard coil ID is 20in 

            //checks if there is carboard in the coil
            if (cardboardIn22.IsChecked == true)
            {
                defaultCoilInnerDiameter += 1;
                outputBox.Inlines.Add("Has cardboard liner: True");
                outputBox.Inlines.Add(new LineBreak());
            }
            else
            {
                outputBox.Inlines.Add("Has cardboard liner: False");
                outputBox.Inlines.Add(new LineBreak());
            }

            //doing the calculations
            double outerCoilDiameter = (2 * coilMeasureNumberFromBox + (defaultCoilInnerDiameter));
            double numberOfTurns = (outerCoilDiameter - defaultCoilInnerDiameter) / (2 * stripWallNumberFromBox); //gives number of turns on the coil
            double stripLengthInInch = Math.PI * numberOfTurns * (outerCoilDiameter + defaultCoilInnerDiameter) / 2;  // should give the length in inches
            double stripLengthInFeet = stripLengthInInch / 12;  //gets length in feet
            double stripWeight = stripLengthInInch * stripWidthNumberFromBox * stripWallNumberFromBox * defaultSteelDensity;    //should give the weight in pounds

            //output into a textbox
            outputBox.Inlines.Add("Strip length: ");
            outputBox.Inlines.Add(Math.Round(stripLengthInFeet, 2).ToString());
            outputBox.Inlines.Add(" feet.");
            outputBox.Inlines.Add(new LineBreak());
            outputBox.Inlines.Add("Coil weight: ");
            outputBox.Inlines.Add(Math.Round(stripWeight, 2).ToString());
            outputBox.Inlines.Add(" pounds.");
            outputBox.Inlines.Add(new LineBreak());

            if (millSpeedText == 0)
            {
                outputBox.Inlines.Add("No Mill speed entered.");
                outputBox.Inlines.Add(new LineBreak());
            }
            else
            {
                double timeLeftMin = Math.Round((stripLengthInInch / millSpeedText), 0);
                //double timeLeftHours = TimeSpan.FromMinutes(timeLeftMin);

                outputBox.Inlines.Add("Time remaining in min: ");
                outputBox.Inlines.Add(timeLeftMin.ToString());
                outputBox.Inlines.Add(new LineBreak());
                outputBox.Inlines.Add("Time remaining in hours: ");
                outputBox.Inlines.Add(TimeSpan.FromMinutes(timeLeftMin).ToString());
                outputBox.Inlines.Add(new LineBreak());
            }

        }

        private void doCalculateResults ()
        {

            outputBox.Text = string.Empty;

            string stripWallText = stripWallNumber.Text;    //get whats stored int the input box
            string stripWidthText = stripWidthNumber.Text;
            string coilMeasureText = coilMeasureNumber.Text;
            string millSpeedText = millSpeedNumber.Text;

            outputBox.Inlines.Add("Strip wall: ");  //printing
            outputBox.Inlines.Add(stripWallText);
            outputBox.Inlines.Add(new LineBreak());
            outputBox.Inlines.Add("Strip width: ");
            outputBox.Inlines.Add(stripWidthText);
            outputBox.Inlines.Add(new LineBreak());
            outputBox.Inlines.Add("Coil measure: ");
            outputBox.Inlines.Add(coilMeasureText);
            outputBox.Inlines.Add(new LineBreak());
            outputBox.Inlines.Add("Mill speed: ");
            outputBox.Inlines.Add(millSpeedText);
            outputBox.Inlines.Add(new LineBreak());

            calcCoilLeght((Convert.ToDouble(stripWallText)), (Convert.ToDouble(stripWidthText)), (Convert.ToDouble(coilMeasureText)), (Convert.ToDouble(millSpeedText))); //calls actual calculation function

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            doCalculateResults();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) //from learn.microsoft
            {
                doCalculateResults();
            }
        }

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
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus; //removes the reset so texts stays on next click
        }

    }
}
