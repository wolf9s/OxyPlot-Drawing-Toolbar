using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OxyPlotTesting
{
    public partial class InputTextDialog : Form
    {
        public string DisplayText
        {
            get { return valueTextBox.Text; }
            set { valueTextBox.Text = value; }
        }

        public InputTextDialog()
        {
            InitializeComponent();
        }
    }
}
