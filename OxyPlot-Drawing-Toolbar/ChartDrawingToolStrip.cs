using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Annotations;

namespace OxyPlot_Drawing_Toolbar
{
    public sealed partial class ChartDrawingToolStrip
    {
        /* ToolStrip items. */
        public ToolStripButton uiPrintButton;
        public ToolStripButton uiSaveButton;

        private ToolStripButton uiDeleteAllAnnotsButton;
        private ToolStripButton uiDeleteAnnotButton;
        private ToolStripButton uiDrawArrowButton;
        private ToolStripButton uiDrawEllipseButton;
        private ToolStripButton uiDrawLineButton;
        private ToolStripButton uiDrawPolygonButton;
        private ToolStripButton uiDrawPolylineButton;
        private ToolStripButton uiDrawRectangleButton;
        private ToolStripButton uiDrawTextButton;
        private ToolStripLabel uiInfoLabel;
        private ToolStripSeparator uiLayerSeparator;
        private ToolStripButton uiSelectAnnotButton;
        private ToolStripTextBox uiSetFillAlphaTextBox;
        private ToolStripButton uiSetFillColorButton;
        private ToolStripLabel uiSetFillLabel;
        private ToolStripSeparator uiSetFillSeparator;
        private ToolStripComboBox uiSetLayerComboBox;
        private ToolStripLabel uiSetLayerLabel;
        private ToolStripTextBox uiSetLineAlphaTextBox;
        private ToolStripButton uiSetLineColorButton;
        private ToolStripLabel uiSetLineLabel;
        private ToolStripSeparator uiSetLineSeparator;
        private ToolStripComboBox uiSetLineStyleComboBox;
        private ToolStripTextBox uiSetLineThicknessTextBox;
        private ToolStripLabel uiSetTextLabel;
        private ToolStripSeparator uiSetTextSeparator;
        private ToolStripTextBox uiSetTextTextBox;
        private ToolStripComboBox uiSetTypeComboBox;
        private ToolStripLabel uiSetTypeLabel;
        private ToolStripSeparator uiSetTypeSeparator;

        public ChartDrawingToolStrip()
        {
            InitializeToolBar();
            RestoreDefaultToolbarSetup();
        }

        public PlotModel ChartModel { get; set; }

        private enum LimitType
        {
            None,
            Vertical,
            Horizontal
        };

        private void InitializeToolBar()
        {
            // Setup saving buttons ----------------------------------------------------------------
            uiSaveButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Icons.SaveIcon),
                ToolTipText = ToolTips.SaveChart,
            };

            uiPrintButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Icons.PrintIcon),
                ToolTipText = ToolTips.PrintChart,
            };

            // Setup selection buttons -------------------------------------------------------------
            uiSelectAnnotButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.SelectAnnotIcon),
                ToolTipText = ToolTips.SelectAnnot,
            };
            uiSelectAnnotButton.CheckedChanged += uiSelectAnnotButton_OnCheckedChanged;

            uiDeleteAnnotButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DeleteAnnotIcon),
                ToolTipText = ToolTips.DeleteAnnot
            };
            uiDeleteAnnotButton.CheckedChanged += uiDeleteAnnotButton_OnCheckedChanged;

            uiDeleteAllAnnotsButton = new ToolStripButton
            {
                CheckOnClick = false,
                Image = new Bitmap(Icons.DeleteAllAnnotsIcon),
                ToolTipText = ToolTips.DeleteAllAnnots,
            };
            uiDeleteAllAnnotsButton.Click += uiDeleteAllAnnotsButton_OnClick;

            // Setup drawing buttons ---------------------------------------------------------------
            uiDrawArrowButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawArrowIcon),
                ToolTipText = ToolTips.DrawArrow
            };
            uiDrawArrowButton.CheckedChanged += uiDrawArrowButton_OnCheckedChanged;

            uiDrawLineButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawLineIcon),
                ToolTipText = ToolTips.DrawLine
            };
            uiDrawLineButton.CheckedChanged += uiDrawLineButton_OnCheckedChanged;

            uiDrawPolylineButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawPolylineIcon),
                ToolTipText = ToolTips.DrawPolyline
            };
            uiDrawPolylineButton.CheckedChanged += uiDrawPolylineButton_OnCheckedChanged;

            uiDrawRectangleButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawRectangleIcon),
                ToolTipText = ToolTips.DrawRectangle
            };
            uiDrawRectangleButton.CheckedChanged += uiDrawRectangleButton_OnCheckedChanged;

            uiDrawEllipseButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawEllipseIcon),
                ToolTipText = ToolTips.DrawEllipse
            };
            uiDrawEllipseButton.CheckedChanged += uiDrawEllipseButton_OnCheckedChanged;

            uiDrawPolygonButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawPolygonIcon),
                ToolTipText = ToolTips.DrawPolygon
            };
            uiDrawPolygonButton.CheckedChanged += uiDrawPolygonButton_OnCheckedChanged;

            uiDrawTextButton = new ToolStripButton
            {
                CheckOnClick = true,
                Image = new Bitmap(Icons.DrawTextIcon),
                ToolTipText = ToolTips.DrawText
            };
            uiDrawTextButton.CheckedChanged += uiDrawTextButton_OnCheckedChanged;

            // Setup layer selection ---------------------------------------------------------------
            uiSetLayerLabel = new ToolStripLabel("Layer:");
            uiLayerSeparator = new ToolStripSeparator();

            uiSetLayerComboBox = new ToolStripComboBox
            {
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                AutoSize = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(90, 25),
                ToolTipText = ToolTips.SetLayer
            };
            foreach (AnnotationLayer al in Enum.GetValues(typeof (AnnotationLayer)))
                uiSetLayerComboBox.Items.Add(al);
            uiSetLayerComboBox.SelectedIndexChanged += uiSetLayerComboBox_OnSelectedIndexChanged;

            // Setup line or shape type adjustment items -------------------------------------------
            uiSetTypeLabel = new ToolStripLabel("Type:");
            uiSetTypeSeparator = new ToolStripSeparator();

            uiSetTypeComboBox = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                ToolTipText = ToolTips.SetType
            };
            foreach (LimitType lt in Enum.GetValues(typeof (LimitType)))
                uiSetTypeComboBox.Items.Add(lt);

            // Setup line adjustment items ---------------------------------------------------------
            uiSetLineLabel = new ToolStripLabel("Line:");
            uiSetLineSeparator = new ToolStripSeparator();

            uiSetLineStyleComboBox = new ToolStripComboBox
            {
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                AutoSize = false,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Size = new Size(115, 25),
                ToolTipText = ToolTips.SetLineStyle
            };
            foreach (LineStyle ls in Enum.GetValues(typeof (LineStyle)))
                uiSetLineStyleComboBox.Items.Add(ls);
            uiSetLineStyleComboBox.SelectedIndexChanged +=
                uiSetLineStyleComboBox_OnSelectedIndexChanged;

            uiSetLineThicknessTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = ToolTips.SetLineThickness
            };
            uiSetLineThicknessTextBox.KeyDown += uiSetLineThicknessTextBox_OnKeyDown;

            uiSetLineColorButton = new ToolStripButton
            {
                AutoSize = false,
                Size = new Size(25, 25),
                ToolTipText = ToolTips.SetLineColor
            };
            uiSetLineColorButton.Click += uiSetLineColorButton_OnClick;

            uiSetLineAlphaTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = ToolTips.SetLineAlpha
            };
            uiSetLineAlphaTextBox.KeyDown += uiSetLineAlphaTextBox_OnKeyDown;

            // Setup fill color adjustment items ---------------------------------------------------
            uiSetFillLabel = new ToolStripLabel("Fill:");
            uiSetFillSeparator = new ToolStripSeparator();

            uiSetFillColorButton = new ToolStripButton
            {
                AutoSize = false,
                BackColor = Color.SeaGreen,
                Size = new Size(25, 25),
                ToolTipText = ToolTips.SetFillColor
            };
            uiSetFillColorButton.Click += uiSetFillColorButton_OnClick;

            uiSetFillAlphaTextBox = new ToolStripTextBox
            {
                AutoSize = false,
                Size = new Size(40, 25),
                ToolTipText = ToolTips.SetFillAlpha
            };
            uiSetFillAlphaTextBox.KeyDown += uiSetFillAlphaTextBox_OnKeyDown;

            // Setup text adjustment items ---------------------------------------------------------
            uiSetTextLabel = new ToolStripLabel("Text:");
            uiSetTextSeparator = new ToolStripSeparator();

            uiSetTextTextBox = new ToolStripTextBox {ToolTipText = ToolTips.SetText};
            uiSetTextTextBox.KeyDown += uiSetTextTextBox_OnKeyDown;

            // Setup information tooltip -----------------------------------------------------------
            uiInfoLabel = new ToolStripLabel
            {
                Image = new Bitmap(Icons.InfoIcon),
                ToolTipText =
                    ToolTips.Info
            };

            // Add everything to the toolbar -------------------------------------------------------
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

        private void ResetItemsToggle(ToolStripButton clickedButton)
        {
            foreach (
                ToolStripButton btn in
                    Items.OfType<ToolStripButton>().Where(c => c != clickedButton))
                btn.Checked = false;
        }

        private void RestoreDefaultToolbarSetup()
        {
            uiSetLayerComboBox.SelectedIndex =
                uiSetLayerComboBox.Items.IndexOf(AnnotationLayer.BelowSeries);
            uiSetTypeComboBox.SelectedIndex =
                uiSetTypeComboBox.Items.IndexOf(LimitType.None);

            uiSetLineStyleComboBox.SelectedIndex =
                uiSetLineStyleComboBox.Items.IndexOf(LineStyle.Solid);
            uiSetLineThicknessTextBox.Text = @"2.00";
            uiSetLineColorButton.BackColor = Color.Black;
            uiSetLineAlphaTextBox.Text = @"255";

            uiSetFillColorButton.BackColor = Color.SeaGreen;
            uiSetFillAlphaTextBox.Text = @"255";

            uiSetTextTextBox.Text = "";

            // Un-select all annotations incase they were selected
            if (ChartModel != null)
            {
                foreach (Annotation a in ChartModel.Annotations)
                    a.Unselect();
            }

            // Reset the toolbar items
            HideAllToolbarItems();
        }

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
    }
}
