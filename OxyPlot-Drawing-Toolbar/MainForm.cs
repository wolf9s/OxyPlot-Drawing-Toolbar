using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OxyPlot_Drawing_Toolbar
{
    public partial class MainForm : Form
    {
        // Number of series to display
        private const int NumSeries = 10;
        // Number of points to generate per series
        private const int NumPoints = 25;
        // Minimum x value for function series
        private const int MinimumX = 0;
        // Maximum x value for function series
        private const int MaximumX = 50;

        public MainForm()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void InitializeChart()
        {
            // Create the plot model
            PlotModel mainModel = new PlotModel
            {
                DefaultColors = OxyPalettes.HueDistinct(NumSeries).Colors,
                IsLegendVisible = true,
                Title = "Draw on me!"
            };
            
            // Add a basic xAxis
            mainModel.Axes.Add(new LinearAxis
            {
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "X"
            });

            // And a basic yAxis
            mainModel.Axes.Add(new LinearAxis
            {
                IsPanEnabled = true,
                IsZoomEnabled = true,
                Key = "yAxis",
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Y"
            });

            // And generate some interesting data on the chart to play with
            for (int s = 0; s < NumSeries; s++)
                switch (s % 3)
                {
                    case 0:
                        mainModel.Series.Add(
                            new FunctionSeries(x => Math.Sin(x + s), 
                                MinimumX, MaximumX, NumPoints) { YAxisKey = "yAxis" });
                        break;

                    case 1:
                        mainModel.Series.Add(
                            new FunctionSeries(x => Math.Sin((x + s) / 4) * Math.Acos(Math.Sin(x + s)),
                                MinimumX, MaximumX, NumPoints) { YAxisKey = "yAxis" });
                        break;

                    case 2:
                        mainModel.Series.Add(
                            new FunctionSeries(x => Math.Sin(2 * Math.Cos(2 * Math.Sin(2 * Math.Cos(x + s)))),
                                MinimumX, MaximumX, NumPoints) {YAxisKey = "yAxis"});
                        break;
                }

            // Add the model to both the PlotView and the Drawing Toolbar
            uiChartPlotView.Model = mainModel;
            uiDrawingToolbar.ChartModel = mainModel;

            // And for fun, make it so you can copy the chart with Ctrl+C
            uiChartPlotView.ActualController.BindKeyDown(OxyKey.C, OxyModifierKeys.Control,
                new DelegatePlotCommand<OxyKeyEventArgs>(CopyChart_OnKeyDown));
        }
    }
}
