using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PiDigitLogic;

namespace WinAppPiDigits
{
    public partial class frmCalculatePi : Form
    {
        public frmCalculatePi()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            txtBoxPi.Text = CalcPi(Decimal.ToInt32(numUpDown.Value));
            btnCalculate.Text = "Cancel";
        }

        private string CalcPi(int digits)
        {
            StringBuilder pi = new StringBuilder("3", digits + 2);
            if (digits > 0)
            {
                pi.Append(".");
                for (int i = 0; i < digits; i += 9)
                {
                    int nineDigits = NineDigitsOfPi.StartingAt(i + 1);
                    int digitCount = Math.Min(digits - i, 9);
                    string ds = $"{nineDigits:D9}"; 
                       // string.Format("{0:D9}", nineDigits);
                    pi.Append(ds.Substring(0, digitCount));
                }
            }
            return pi.ToString();
        }
    }
}
