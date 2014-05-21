using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Annotations;

using OxyPlot_Drawing_Toolbar.Properties;

namespace OxyPlotTesting
{
    public sealed partial class ChartDrawingToolStrip 
    {
        // Chart model to draw on. Must be set! 
        public PlotModel ChartModel { get; set; }

        #region Toolbar Items

        public ToolStripButton uiSaveButton;
        public ToolStripButton uiPrintButton;

        private ToolStripButton uiSelectAnnotButton;
        private ToolStripButton uiDeleteAnnotButton;
        private ToolStripButton uiDeleteAllAnnotsButton;

        private ToolStripButton uiDrawArrowButton;
        private ToolStripButton uiDrawLineButton;
        private ToolStripButton uiDrawPolylineButton;
        private ToolStripButton uiDrawRectangleButton;
        private ToolStripButton uiDrawEllipseButton;
        private ToolStripButton uiDrawPolygonButton;
        private ToolStripButton uiDrawTextButton;

        private ToolStripLabel uiSetLayerLabel;
        private ToolStripComboBox uiSetLayerComboBox;
        private ToolStripSeparator uiLayerSeparator;

        private ToolStripLabel uiSetTypeLabel;
        private ToolStripComboBox uiSetTypeComboBox;
        private ToolStripSeparator uiSetTypeSeparator;

        private ToolStripLabel uiSetLineLabel;
        private ToolStripComboBox uiSetLineStyleComboBox;
        private ToolStripTextBox uiSetLineThicknessTextBox;
        private ToolStripButton uiSetLineColorButton;
        private ToolStripTextBox uiSetLineAlphaTextBox;
        private ToolStripSeparator uiSetLineSeparator;

        private ToolStripLabel uiSetFillLabel;
        private ToolStripButton uiSetFillColorButton;
        private ToolStripTextBox uiSetFillAlphaTextBox;
        private ToolStripSeparator uiSetFillSeparator;

        private ToolStripLabel uiSetTextLabel;
        private ToolStripTextBox uiSetTextTextBox;
        private ToolStripSeparator uiSetTextSeparator;

        private ToolStripLabel uiInfoLabel;

        #endregion

        public ChartDrawingToolStrip()
        {
            InitializeToolBar();
            RestoreDefaultToolbarSetup();
        }

        #region Messy ToolBar Setup Code

        private void InitializeToolBar()
        {
            // Setup saving buttons ------------------------------------------------------------------------------------
            uiSaveButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Resources.SaveIcon),  
                ToolTipText = "Save the chart to an image.",
            };

            uiPrintButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Resources.PrintIcon),  
                ToolTipText = "Print the chart.",
            };

            // Setup selection buttons ---------------------------------------------------------------------------------
            uiSelectAnnotButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.SelectAnnotIcon),
                ToolTipText = "Allow annotation selection for modifications.",
            };
            uiSelectAnnotButton.CheckedChanged += uiSelectAnnotButton_OnCheckedChanged;

            uiDeleteAnnotButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DeleteAnnotIcon),
                ToolTipText = "Delete annotations clicked on."
            };
            uiDeleteAnnotButton.CheckedChanged += uiDeleteAnnotButton_OnCheckedChanged;

            uiDeleteAllAnnotsButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Resources.DeleteAllAnnotsIcon), 
                ToolTipText = "Delete all annotations on the chart.",
            };
            uiDeleteAllAnnotsButton.Click += uiDeleteAllAnnotsButton_OnClick;

            // Setup drawing buttons -----------------------------------------------------------------------------------
            uiDrawArrowButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawArrowIcon),
                ToolTipText = "Draw an arrow annotation on the chart."
            };
            uiDrawArrowButton.CheckedChanged += uiDrawArrowButton_OnCheckedChanged;

            uiDrawLineButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawLineIcon),
                ToolTipText = "Draw a line annotation on the chart."
            };
            uiDrawLineButton.CheckedChanged += uiDrawLineButton_OnCheckedChanged;

            uiDrawPolylineButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawPolylineIcon),
                ToolTipText = "Draw a multi-jointed line on the chart."
            };
            uiDrawPolylineButton.CheckedChanged += uiDrawPolylineButton_OnCheckedChanged;

            uiDrawRectangleButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawRectangleIcon),
                ToolTipText = "Draw a rectangle annotation on the chart."
            };
            uiDrawRectangleButton.CheckedChanged += uiDrawRectangleButton_OnCheckedChanged;

            uiDrawEllipseButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawEllipseIcon),
                ToolTipText = "Draw an ellipse annotation on the chart."
            };
            uiDrawEllipseButton.CheckedChanged += uiDrawEllipseButton_OnCheckedChanged;

            uiDrawPolygonButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawPolygonIcon),
                ToolTipText = "Draw a polygon annotation on the chart."
            };
            uiDrawPolygonButton.CheckedChanged += uiDrawPolygonButton_OnCheckedChanged;

            uiDrawTextButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Resources.DrawTextIcon),
                ToolTipText = "Draw a text annotation on the chart."
            };
            uiDrawTextButton.CheckedChanged += uiDrawTextButton_OnCheckedChanged;

            // Setup layer selection -----------------------------------------------------------------------------------
            uiSetLayerLabel = new ToolStripLabel("Layer:");
            uiLayerSeparator = new ToolStripSeparator();

            uiSetLayerComboBox = new ToolStripComboBox
            {
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                AutoSize = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(90, 25),
                ToolTipText = "Set annotation display layer."
            };
            foreach (AnnotationLayer al in Enum.GetValues(typeof (AnnotationLayer)))
                uiSetLayerComboBox.Items.Add(al);
            uiSetLayerComboBox.SelectedIndexChanged += uiSetLayerComboBox_OnSelectedIndexChanged;

            // Setup line or shape type adjustment items ---------------------------------------------------------------
            uiSetTypeLabel = new ToolStripLabel("Type:");
            uiSetTypeSeparator = new ToolStripSeparator();

            uiSetTypeComboBox = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                ToolTipText = "Select line or shape display type."
            };
            uiSetTypeComboBox.Items.Add(Resources.LimitTypeNone);
            uiSetTypeComboBox.Items.Add(Resources.LimitTypeVertical);
            uiSetTypeComboBox.Items.Add(Resources.LimitTypeHorizontal);

            // Setup line adjustment items -----------------------------------------------------------------------------
            uiSetLineLabel = new ToolStripLabel("Line:");
            uiSetLineSeparator = new ToolStripSeparator();

            uiSetLineStyleComboBox = new ToolStripComboBox
            {
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                AutoSize = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(115, 25),
                ToolTipText = "Set Annotation Line Style."
            };
            foreach (LineStyle ls in Enum.GetValues(typeof (LineStyle)))
                uiSetLineStyleComboBox.Items.Add(ls);
            uiSetLineStyleComboBox.SelectedIndexChanged += uiSetLineStyleComboBox_OnSelectedIndexChanged;

            uiSetLineThicknessTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = "Set annotation line thickness."
            };
            uiSetLineThicknessTextBox.KeyDown += uiSetLineThicknessTextBox_OnKeyPress;

            uiSetLineColorButton = new ToolStripButton
            {
                AutoSize = false,
                Size = new Size(25, 25),
                ToolTipText = "Set annotation line colour."
            };
            uiSetLineColorButton.Click += uiSetLineColorButton_OnClick;

            uiSetLineAlphaTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = "Set annotation line transparency (between 0 and 255)."
            };
            uiSetLineAlphaTextBox.KeyDown += uiSetLineAlphaTextBox_OnKeyDown;

            // Setup fill color adjustment items -----------------------------------------------------------------------
            uiSetFillLabel = new ToolStripLabel("Fill:");
            uiSetFillSeparator = new ToolStripSeparator();

            uiSetFillColorButton = new ToolStripButton
            {
                AutoSize = false,
                BackColor = Color.SeaGreen,
                Size = new Size(25, 25),
                ToolTipText = "Set annotation fill colour."
            };
            uiSetFillColorButton.Click += uiSetFillColorButton_OnClick;

            uiSetFillAlphaTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = "Set annotation fill transparency (between 0 and 255)."
            };
            uiSetFillAlphaTextBox.KeyDown += uiSetFillAlphaTextBox_OnKeyDown;

            // Setup text adjustment items -----------------------------------------------------------------------------
            uiSetTextLabel = new ToolStripLabel("Text:");
            uiSetTextSeparator = new ToolStripSeparator();

            uiSetTextTextBox = new ToolStripTextBox {ToolTipText = "Set annotation text."};
            uiSetTextTextBox.KeyDown += uiSetTextTextBox_OnKeyDown;

            // Setup information tooltip -------------------------------------------------------------------------------
            uiInfoLabel = new ToolStripLabel
            {
                Image = new Bitmap(Resources.InfoIcon),
                ToolTipText =
                    "You can copy the chart to the clipboard by clicking on it and pressing Ctrl+C!\n" +
                    @"The icons used in this toolbar are from the Fugue Icon set: http://p.yusukekamiyamane.com/"
            };
            
            // Add everything to the toolbar ---------------------------------------------------------------------------
            Items.Add(uiSaveButton);
            Items.Add(uiPrintButton);
            Items.Add(new ToolStripSeparator());

            Items.Add(uiSelectAnnotButton);
            Items.Add(uiDeleteAnnotButton);
            Items.Add(uiDeleteAllAnnotsButton);
            Items.Add(new ToolStripSeparator());

            Items.Add(uiDrawArrowButton);
            Items.Add(uiDrawLineButton);
            Items.Add(uiDrawPolylineButton);
            Items.Add(uiDrawRectangleButton);
            Items.Add(uiDrawEllipseButton);
            Items.Add(uiDrawPolygonButton);
            Items.Add(uiDrawTextButton);
            Items.Add(new ToolStripSeparator());

            Items.Add(uiSetLayerLabel);
            Items.Add(uiSetLayerComboBox);
            Items.Add(uiLayerSeparator);

            Items.Add(uiSetTypeLabel);
            Items.Add(uiSetTypeComboBox);
            Items.Add(uiSetTypeSeparator);

            Items.Add(uiSetLineLabel);
            Items.Add(uiSetLineStyleComboBox);
            Items.Add(uiSetLineThicknessTextBox);
            Items.Add(uiSetLineColorButton);
            Items.Add(uiSetLineAlphaTextBox);
            Items.Add(uiSetLineSeparator);

            Items.Add(uiSetFillLabel);
            Items.Add(uiSetFillColorButton);
            Items.Add(uiSetFillAlphaTextBox);
            Items.Add(uiSetFillSeparator);

            Items.Add(uiSetTextLabel);
            Items.Add(uiSetTextTextBox);
            Items.Add(uiSetTextSeparator);

            Items.Add(uiInfoLabel);
        }

        #endregion 

        private void ResetItemsToggle(ToolStripButton clickedButton)
        {
            foreach (ToolStripButton btn in Items.OfType<ToolStripButton>().Where(c => c != clickedButton))
                btn.Checked = false;
        }

        private void RestoreDefaultToolbarSetup()
        {
            uiSetLayerComboBox.SelectedIndex = uiSetLayerComboBox.Items.IndexOf(AnnotationLayer.BelowSeries);
            uiSetTypeComboBox.SelectedIndex = uiSetTypeComboBox.Items.IndexOf(Resources.LimitTypeNone);

            uiSetLineStyleComboBox.SelectedIndex = uiSetLineStyleComboBox.Items.IndexOf(LineStyle.Solid);
            uiSetLineThicknessTextBox.Text = "2.00";
            uiSetLineColorButton.BackColor = Color.Black;
            uiSetLineAlphaTextBox.Text = "255";

            uiSetFillColorButton.BackColor = Color.SeaGreen;
            uiSetFillAlphaTextBox.Text = "255";

            uiSetTextTextBox.Text = "";

            // Un-select all annotations incase they were selected
            if (ChartModel != null)
                foreach (Annotation annot in ChartModel.Annotations)
                    annot.Unselect();

            // Reset the toolbar items
            HideAllToolbarItems();
        }

        #region Show and Hide toolbar items

        private void HideAllToolbarItems()
        {
            HideLayerToolbarItems();
            HideTypeToolbarItems();
            HideLineToolbarItems();
            HideFillToolbarItems();
            HideTextToolbarItems();
        }

        private void ShowAnnotEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            HideTypeToolbarItems();
            ShowLineToolbarItems();
            ShowFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void ShowLineEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            ShowTypeToolbarItems();
            ShowLineToolbarItems();
            HideFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void ShowPolylineEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            HideTypeToolbarItems();
            ShowLineToolbarItems();
            HideFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void ShowRectangleEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            ShowTypeToolbarItems();
            HideLineToolbarItems();
            ShowFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void ShowShapeEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            HideTypeToolbarItems();
            HideLineToolbarItems();
            ShowFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void ShowTextEditingToolbarItems()
        {
            ShowLayerToolbarItems();
            HideTypeToolbarItems();
            HideLineToolbarItems();
            HideFillToolbarItems();
            ShowTextToolbarItems();
        }

        private void HideFillToolbarItems()
        {
            uiSetFillLabel.Visible = false;
            uiSetFillColorButton.Visible = false;
            uiSetFillAlphaTextBox.Visible = false;
            uiSetFillSeparator.Visible = false;
        }

        private void HideLayerToolbarItems()
        {
            uiSetLayerLabel.Visible = false;
            uiSetLayerComboBox.Visible = false;
            uiLayerSeparator.Visible = false;
        }

        private void HideLineToolbarItems()
        {
            uiSetLineLabel.Visible = false;
            uiSetLineStyleComboBox.Visible = false;
            uiSetLineThicknessTextBox.Visible = false;
            uiSetLineColorButton.Visible = false;
            uiSetLineAlphaTextBox.Visible = false;
            uiSetLineSeparator.Visible = false;
        }

        private void HideTextToolbarItems()
        {
            uiSetTextLabel.Visible = false;
            uiSetTextTextBox.Visible = false;
            uiSetTextSeparator.Visible = false;
        }

        private void HideTypeToolbarItems()
        {
            uiSetTypeLabel.Visible = false;
            uiSetTypeComboBox.Visible = false;
            uiSetTypeSeparator.Visible = false;
        }

        private void ShowFillToolbarItems()
        {
            uiSetFillLabel.Visible = true;
            uiSetFillColorButton.Visible = true;
            uiSetFillAlphaTextBox.Visible = true;
            uiSetFillSeparator.Visible = true;
        }

        private void ShowLayerToolbarItems()
        {
            uiSetLayerLabel.Visible = true;
            uiSetLayerComboBox.Visible = true;
            uiLayerSeparator.Visible = true;
        }

        private void ShowLineToolbarItems()
        {
            uiSetLineLabel.Visible = true;
            uiSetLineStyleComboBox.Visible = true;
            uiSetLineThicknessTextBox.Visible = true;
            uiSetLineColorButton.Visible = true;
            uiSetLineAlphaTextBox.Visible = true;
            uiSetLineSeparator.Visible = true;
        }

        private void ShowTextToolbarItems()
        {
            uiSetTextLabel.Visible = true;
            uiSetTextTextBox.Visible = true;
            uiSetTextSeparator.Visible = true;
        }

        private void ShowTypeToolbarItems()
        {
            uiSetTypeLabel.Visible = true;
            uiSetTypeComboBox.Visible = true;
            uiSetTypeSeparator.Visible = true;
        }

        #endregion
    }
}
