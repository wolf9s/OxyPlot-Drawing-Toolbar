namespace OxyPlot_Drawing_Toolbar
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.uiMainContainer = new System.Windows.Forms.ToolStripContainer();
            this.uiChartPlotView = new OxyPlot.WindowsForms.PlotView();
            this.uiDrawingToolbar = new OxyPlotTesting.ChartDrawingToolStrip();
            this.uiMainContainer.ContentPanel.SuspendLayout();
            this.uiMainContainer.TopToolStripPanel.SuspendLayout();
            this.uiMainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiMainContainer
            // 
            // 
            // uiMainContainer.ContentPanel
            // 
            this.uiMainContainer.ContentPanel.Controls.Add(this.uiChartPlotView);
            this.uiMainContainer.ContentPanel.Size = new System.Drawing.Size(1264, 657);
            this.uiMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiMainContainer.Location = new System.Drawing.Point(0, 0);
            this.uiMainContainer.Name = "uiMainContainer";
            this.uiMainContainer.Size = new System.Drawing.Size(1264, 682);
            this.uiMainContainer.TabIndex = 0;
            this.uiMainContainer.Text = "toolStripContainer1";
            // 
            // uiMainContainer.TopToolStripPanel
            // 
            this.uiMainContainer.TopToolStripPanel.Controls.Add(this.uiDrawingToolbar);
            // 
            // uiChartPlotView
            // 
            this.uiChartPlotView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiChartPlotView.Location = new System.Drawing.Point(0, 0);
            this.uiChartPlotView.Name = "uiChartPlotView";
            this.uiChartPlotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.uiChartPlotView.Size = new System.Drawing.Size(1264, 657);
            this.uiChartPlotView.TabIndex = 0;
            this.uiChartPlotView.Text = "plot1";
            this.uiChartPlotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.uiChartPlotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.uiChartPlotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // uiDrawingToolbar
            // 
            this.uiDrawingToolbar.ChartModel = null;
            this.uiDrawingToolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.uiDrawingToolbar.Location = new System.Drawing.Point(3, 0);
            this.uiDrawingToolbar.Name = "uiDrawingToolbar";
            this.uiDrawingToolbar.Size = new System.Drawing.Size(247, 25);
            this.uiDrawingToolbar.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.uiMainContainer);
            this.Name = "MainForm";
            this.Text = "OxyPlot Drawing Toolbar Example";
            this.uiMainContainer.ContentPanel.ResumeLayout(false);
            this.uiMainContainer.TopToolStripPanel.ResumeLayout(false);
            this.uiMainContainer.TopToolStripPanel.PerformLayout();
            this.uiMainContainer.ResumeLayout(false);
            this.uiMainContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer uiMainContainer;
        private OxyPlot.WindowsForms.PlotView uiChartPlotView;
        private OxyPlotTesting.ChartDrawingToolStrip uiDrawingToolbar;
    }
}

