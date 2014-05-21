using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;

namespace OxyPlot_Drawing_Toolbar
{
    public partial class MainForm
    {
        private void CopyChart_OnKeyDown(IPlotView view, IController controller, OxyKeyEventArgs args)
        {
            Bitmap chartCopy = new Bitmap(uiChartPlotView.Width, uiChartPlotView.Height);
            uiChartPlotView.DrawToBitmap(chartCopy, new Rectangle(0, 0, uiChartPlotView.Width, uiChartPlotView.Height));
            Clipboard.SetImage(chartCopy);
        }
    }
}
