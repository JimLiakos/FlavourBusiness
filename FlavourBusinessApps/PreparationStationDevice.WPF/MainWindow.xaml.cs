using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
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
using GenWebBrowser;

namespace PreparationStationDevice.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{af111142-59d9-433e-a008-34a557c9b396}</MetaDataID>
    public partial class MainWindow : Window
    {
        private readonly SpeechRecognitionEngine recognizer;
        WebBrowserOverlay Browser;
        public MainWindow()
        {
            InitializeComponent();


            DataContext = new FlavoursPreparationStation();
            string url = @"http://192.168.2.8:4301/";//org
            //url = @"http://192.168.2.4:4301/";//Braxati
            //url = @"http://10.0.0.8:4301/";//work


            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url));
            var selectedRecognizer = (from e in SpeechRecognitionEngine.InstalledRecognizers()
                                      where e.Culture.Equals(System.Threading.Thread.CurrentThread.CurrentCulture)
                                      select e).FirstOrDefault();

            //recognizer = new SpeechRecognitionEngine(selectedRecognizer);
            //recognizer.AudioStateChanged += Recognizer_AudioStateChanged;
            //recognizer.SpeechHypothesized += Recognizer_SpeechHypothesized;
            //recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            //recognizer.RecognizeAsync(RecognizeMode.Multiple);

            //SpeechRecognitionEngine.
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Recognized++;
            //LabelRecognized.Content = "Recognized: " + Recognized.ToString();
            //if (RecogState == State.Off)
            //    return;
            float accuracy = (float)e.Result.Confidence;
            string phrase = e.Result.Text;
            System.Diagnostics.Debug.WriteLine(phrase);


            //{
            //    if (phrase == "End Dictate")
            //    {
            //        RecogState = State.Off;
            //        recognizer.RecognizeAsyncStop();
            //        ReadAloud("Dictation Ended");
            //        return;
            //    }

            //    TextBox1.AppendText(" " + e.Result.Text);
            //}
        }

        private void Recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
        }

        private void Recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            
        }
    }
}

