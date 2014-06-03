using System.Windows.Forms;

namespace OxyPlot_Drawing_Toolbar
{
    public partial class InputTextDialog : Form
    {
        public InputTextDialog()
        {
            InitializeComponent();
        }

        public string DisplayText
        {
            get { return valueTextBox.Text; }
            set { valueTextBox.Text = value; }
        }
    }
}
