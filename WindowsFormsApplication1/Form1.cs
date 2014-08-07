using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Threading;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SpeechRecognitionEngine sr = new SpeechRecognitionEngine();
        TimeSpan time;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tr = textBox2.Text;
            Choices sList = new Choices();
            sList.Add(new string[] { tr }); //, "check","one","two","three"
            Grammar gr = new Grammar(new GrammarBuilder(sList));

            try
            {
                sr.RequestRecognizerUpdate();
                sr.LoadGrammar(gr);
                sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
                sr.RecognizerUpdateReached += new EventHandler<RecognizerUpdateReachedEventArgs>(sr_RecognizerUpdateReached);
                string tp = textBox3.Text;
                sr.SetInputToWaveFile(tp);
                sr.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch
            {
                return;
            }
        }

        public void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            sr.RequestRecognizerUpdate();
            textBox1.Text += e.Result.Text.ToString() + " at " + time.ToString("g") + Environment.NewLine;
        }

        public void sr_RecognizerUpdateReached(object sender, RecognizerUpdateReachedEventArgs e)
        {
            time = e.AudioPosition;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sr.RecognizeAsyncStop();
            textBox1.Text = "";
        }

        private int Converttosplitfiles(string SavedFileName, string flacfile)
        {
            int Number_of_split_files = 1;
            string[] path = flacfile.Split('.');
            string bigpath;
            int decimalLength = Number_of_split_files.ToString("D").Length + 2;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = SavedFileName + " " + flacfile + " silence 1 1 1% 1 .5 1% : newfile : restart";
            process.StartInfo.FileName = @"C:\Program Files (x86)\sox-14-4-1\sox.exe";
            process.Start();
            while (!process.HasExited)
            {

            }
            bigpath = path[0] + Number_of_split_files.ToString("D" + decimalLength.ToString()) + ".flac";

            while (File.Exists(bigpath))
            {
                decimalLength = Number_of_split_files.ToString("D").Length + 2;
                Number_of_split_files++;
                bigpath = path[0] + Number_of_split_files.ToString("D" + decimalLength.ToString()) + ".flac";
            }
            return Number_of_split_files - 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
