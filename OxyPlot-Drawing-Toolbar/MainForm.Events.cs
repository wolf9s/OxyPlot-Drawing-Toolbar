using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

using OxyPlot;

namespace OxyPlot_Drawing_Toolbar
{
    public partial class MainForm
    {
        private void CopyChart_OnKeyDown(
            IPlotView view,
            IController controller,
            OxyKeyEventArgs args)
        {
            var chartCopy = new Bitmap(uiPlotView.Width, uiPlotView.Height);
            uiPlotView.DrawToBitmap(chartCopy,
                                    new Rectangle(0, 0, uiPlotView.Width, uiPlotView.Height));
            Clipboard.SetImage(chartCopy);
        }

        private void PrintDoc_OnPrintPage(object sender, PrintPageEventArgs args)
        {
            var printBitmap = new Bitmap(uiPlotView.Width, uiPlotView.Height);
            uiPlotView.DrawToBitmap(printBitmap,
                                    new Rectangle(0, 0, uiPlotView.Width, uiPlotView.Height));
            args.Graphics.DrawImage(printBitmap, new Point(0, 0));
        }

        private void uiPrintButton_OnClick(object sender, EventArgs args)
        {
            using (var d = new PrintDialog())
            {
                var printDoc = new PrintDocument {DefaultPageSettings = {Landscape = true}};
                printDoc.PrintPage += PrintDoc_OnPrintPage;

                d.AllowPrintToFile = true;
                d.AllowSomePages = true;
                d.Document = printDoc;
                d.ShowHelp = true;

                if (d.ShowDialog() == DialogResult.OK)
                    printDoc.Print();
            }
        }

        private void uiSaveButton_OnClick(object sender, EventArgs args)
        {
            using (var d = new SaveFileDialog())
            {
                d.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                d.FileName = "ChartNameHere";
                d.Filter = @"Bitmap (*.bmp)|*.bmp|Jpg (*.jpg)|*.jpg|Png (*.png)|*.png";
                d.FilterIndex = 4;
                d.DefaultExt = "png";
                d.RestoreDirectory = true;

                if (d.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(d.FileName))
                    return;

                var saveChart = new Bitmap(uiPlotView.Width, uiPlotView.Height);
                uiPlotView.DrawToBitmap(saveChart,
                                        new Rectangle(0, 0, uiPlotView.Width, uiPlotView.Height));

                switch (d.FilterIndex)
                {
                    case 1:
                        saveChart.Save(d.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 2:
                        saveChart.Save(d.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 3:
                        // Can also be done using OxyPlots built in png exporter:
                        // PngExporter.Export(uiPlotView.Model,
                        //                    d.FileName,
                        //                    uiPlotView.Width,
                        //                    uiPlotView.Height);
                        saveChart.Save(d.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
            }
        }
    }
}
