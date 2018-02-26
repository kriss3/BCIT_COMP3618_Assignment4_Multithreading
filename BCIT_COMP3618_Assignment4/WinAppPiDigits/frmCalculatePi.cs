using PiDigitLogic;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace WinAppPiDigits
{
    /// <summary>
    /// BCIT Krzysztof Szczurowski
    /// Assignment 4 Multithreading;
    /// Repo: https://github.com/kriss3/BCIT_COMP3618_Assignment4_Multithreading.git
    /// </summary>
    public partial class frmCalculatePi : Form
    {
        public frmCalculatePi()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            var piDigits = (int)numUpDown.Value;
            if (bgw.IsBusy)
            {
                bgw.CancelAsync();
                return;
            }
            
            if (piDigits > 0)
            {
                numUpDown.Enabled = false;
                btnCalculate.Text = "&Cancel";
                txtBoxPi.Text = string.Empty;
                bgw.RunWorkerAsync((int)(numUpDown.Value));
            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            bgw = sender as BackgroundWorker;
            bgw.WorkerSupportsCancellation = true;
            bgw.WorkerReportsProgress = true;
            int arg = (int)e.Argument;
            e.Result = CalcPi(arg, e);
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            txtBoxPi.Text = (string)e.UserState;
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show($"Error occured: {e.Error.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                txtBoxPi.Text = $"{txtBoxPi.Text} <<cancelled>>";
            }
            else
            {
                txtBoxPi.Text = (string)e.Result;
                progressBar.Value = 100;
            }
            numUpDown.Enabled = true;
            btnCalculate.Text = "&Calculate";
        }

        private string CalcPi(int digits, object e)
        {
            var x = (DoWorkEventArgs)e;
            var progressCheck = 0;
            StringBuilder pi = new StringBuilder("3", digits + 2);
            if (digits > 0)
            {
                pi.Append(".");
                for (int i = 0; i < digits; i += 9)
                {
                    if (bgw.CancellationPending)
                    {
                        x.Cancel = true;
                        break;
                    }

                    int nineDigits = NineDigitsOfPi.StartingAt(i + 1);
                    int digitCount = Math.Min(digits - i, 9);
                    string ds = $"{nineDigits:D9}";
                    pi.Append(ds.Substring(0, digitCount));
                    var prc = (int)((float)i / (float)digits * 100f);

                    if (prc > progressCheck)
                    {
                        progressCheck = prc;
                        bgw.ReportProgress(prc, pi.ToString());
                    }
                }
            }
            return pi.ToString();
        }
    }
}
