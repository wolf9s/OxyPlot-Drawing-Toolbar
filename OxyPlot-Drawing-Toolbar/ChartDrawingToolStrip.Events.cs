using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

using OxyPlot_Drawing_Toolbar.Properties;

namespace OxyPlotTesting
{
    sealed partial class ChartDrawingToolStrip
    {
        // Setup used when displaying annotation to user before drawn
        private const int HoverAlpha = 150;
        private readonly OxyColor HoverColor = OxyColor.FromAColor(HoverAlpha, OxyColors.Cyan);
        private const LineStyle HoverLineStyle = LineStyle.Dash;
        private const int HoverLineThickness = 4;

        // Annotation used to draw on the chart
        private Annotation _tempAnnot;

        private void DeleteAnnot_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            ChartModel.Annotations.Remove((Annotation) sender);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawArrow_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());

            _tempAnnot = new ArrowAnnotation
            {
                Color = HoverColor,
                EndPoint = xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis),
                StartPoint = xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis),
            };

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawArrow_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());

            ((ArrowAnnotation) _tempAnnot).EndPoint = xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis);

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawArrow_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((ArrowAnnotation) _tempAnnot).Color = OxyColor.FromAColor(Convert.ToByte(uiSetLineAlphaTextBox.Text),
                uiSetLineColorButton.BackColor.ToOxyColor());
            _tempAnnot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
            ((ArrowAnnotation) _tempAnnot).LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
            ((ArrowAnnotation) _tempAnnot).StrokeThickness = Convert.ToDouble(uiSetLineThicknessTextBox.Text);
            ((ArrowAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawEllipse_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());

            _tempAnnot = new EllipseAnnotation
            {
                Fill = HoverColor,
                Height = 0,
                Layer = AnnotationLayer.AboveSeries,
                Width = 0,
                X = xAxis.InverseTransform(args.Position.X),
                Y = yAxis.InverseTransform(args.Position.Y)
            };

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawEllipse_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());

            ((EllipseAnnotation) _tempAnnot).Width = (xAxis.InverseTransform(args.Position.X) -
                                                      ((EllipseAnnotation) _tempAnnot).X) * 2;
            ((EllipseAnnotation) _tempAnnot).Height = (yAxis.InverseTransform(args.Position.Y) -
                                                       ((EllipseAnnotation) _tempAnnot).Y) * 2;

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawEllipse_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((EllipseAnnotation) _tempAnnot).Fill = OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text),
                uiSetFillColorButton.BackColor.ToOxyColor());
            _tempAnnot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
            ((EllipseAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawLine_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            _tempAnnot = new LineAnnotation
            {
                Color = HoverColor,
                LineStyle = HoverLineStyle,
                StrokeThickness = HoverLineThickness,
            };

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            string type = (string) uiSetTypeComboBox.SelectedItem;

            if (type == Resources.LimitTypeNone)
            {
                ((LineAnnotation) _tempAnnot).X = xAxis.InverseTransform(args.Position.X);
                ((LineAnnotation) _tempAnnot).Y = yAxis.InverseTransform(args.Position.Y);
                ((LineAnnotation) _tempAnnot).Intercept = yAxis.InverseTransform(args.Position.Y);
                ((LineAnnotation) _tempAnnot).Slope = 0;
            }
            else if (type == Resources.LimitTypeVertical)
            {
                ((LineAnnotation) _tempAnnot).Type = LineAnnotationType.Vertical;
                ((LineAnnotation) _tempAnnot).X = xAxis.InverseTransform(args.Position.X);
            }
            else if (type == Resources.LimitTypeHorizontal)
            {
                ((LineAnnotation) _tempAnnot).Type = LineAnnotationType.Horizontal;
                ((LineAnnotation) _tempAnnot).Y = yAxis.InverseTransform(args.Position.Y);
            }
            
            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawLine_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            string type = (string) uiSetTypeComboBox.SelectedItem;

            if (type == Resources.LimitTypeNone)
            {
                double x1 = ((LineAnnotation) _tempAnnot).X;
                double x2 = xAxis.InverseTransform(args.Position.X);
                double y1 = ((LineAnnotation) _tempAnnot).Y;
                double y2 = yAxis.InverseTransform(args.Position.Y);
                double slope = (y2 - y1) / (x2 - x1);

                ((LineAnnotation) _tempAnnot).Intercept = y2 - slope * x2;
                ((LineAnnotation) _tempAnnot).Slope = slope;
            }
            else if (type == Resources.LimitTypeVertical)
                ((LineAnnotation) _tempAnnot).X = xAxis.InverseTransform(args.Position.X);
            else if (type == Resources.LimitTypeHorizontal)
                ((LineAnnotation) _tempAnnot).Y = yAxis.InverseTransform(args.Position.Y);

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawLine_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((LineAnnotation) _tempAnnot).Color = OxyColor.FromAColor(Convert.ToByte(uiSetLineAlphaTextBox.Text),
                uiSetLineColorButton.BackColor.ToOxyColor());
            _tempAnnot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
            ((LineAnnotation) _tempAnnot).LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
            ((LineAnnotation) _tempAnnot).StrokeThickness = Convert.ToDouble(uiSetLineThicknessTextBox.Text);
            ((LineAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolygon_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            _tempAnnot = new PolylineAnnotation
            {
                Color = HoverColor,
                LineStyle = HoverLineStyle,
                StrokeThickness = HoverLineThickness
            };

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolygon_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            PolygonAnnotation polyAnnot = new PolygonAnnotation
            {
                Color = OxyColors.Undefined,
                Fill =
                    OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text),
                        uiSetFillColorButton.BackColor.ToOxyColor()),
                Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem,
                LineStyle = LineStyle.None,
                StrokeThickness = 0,
                Text = uiSetTextTextBox.Text
            };
            polyAnnot.Points.AddRange(((PolylineAnnotation) _tempAnnot).Points);

            ChartModel.Annotations.Remove(_tempAnnot);
            ChartModel.Annotations.Add(polyAnnot);

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolyline_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            _tempAnnot = new PolylineAnnotation
            {
                Color = HoverColor,
                LineStyle = HoverLineStyle,
                StrokeThickness = HoverLineThickness,
            };

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            ((PolylineAnnotation) _tempAnnot).Points.Add(xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis));

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolyline_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            ((PolylineAnnotation) _tempAnnot).Points.Add(xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis));

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolyline_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((PolylineAnnotation) _tempAnnot).Color = OxyColor.FromAColor(Convert.ToByte(uiSetLineAlphaTextBox.Text),
                uiSetLineColorButton.BackColor.ToOxyColor());
            _tempAnnot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
            ((PolylineAnnotation) _tempAnnot).LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
            ((PolylineAnnotation) _tempAnnot).StrokeThickness = Convert.ToDouble(uiSetLineThicknessTextBox.Text);
            ((PolylineAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawRectangle_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            _tempAnnot = new RectangleAnnotation {Fill = HoverColor, Layer = AnnotationLayer.AboveSeries};
            
            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            string type = (string) uiSetTypeComboBox.SelectedItem;

            if (type == Resources.LimitTypeNone || type == Resources.LimitTypeVertical)
            {
                ((RectangleAnnotation) _tempAnnot).MinimumX = xAxis.InverseTransform(args.Position.X);
                ((RectangleAnnotation) _tempAnnot).MaximumX = xAxis.InverseTransform(args.Position.X);
            }

            if (type == Resources.LimitTypeNone || type == Resources.LimitTypeHorizontal)
            {
                ((RectangleAnnotation) _tempAnnot).MinimumY = yAxis.InverseTransform(args.Position.Y);
                ((RectangleAnnotation) _tempAnnot).MaximumY = yAxis.InverseTransform(args.Position.Y);
            }

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawRectangle_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            string type = (string) uiSetTypeComboBox.SelectedItem;

            if (type == Resources.LimitTypeNone || type == Resources.LimitTypeVertical)
                ((RectangleAnnotation) _tempAnnot).MaximumX = xAxis.InverseTransform(args.Position.X);

            if (type == Resources.LimitTypeNone || type == Resources.LimitTypeHorizontal)
                ((RectangleAnnotation) _tempAnnot).MaximumY = yAxis.InverseTransform(args.Position.Y);

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawRectangle_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((RectangleAnnotation) _tempAnnot).Fill = OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text),
                uiSetFillColorButton.BackColor.ToOxyColor());
            _tempAnnot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
            ((RectangleAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawText_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());

            _tempAnnot = new TextAnnotation
            {
                Background = HoverColor,
                Stroke = OxyColors.Undefined,
                Text = "Move mouse to rotate.",
                TextPosition = xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis),
            };

            ChartModel.Annotations.Add(_tempAnnot);
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawText_OnMouseMove(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            Double x = ((TextAnnotation) _tempAnnot).TextPosition.X;
            Double y = ((TextAnnotation) _tempAnnot).TextPosition.Y;

            ((TextAnnotation) _tempAnnot).TextRotation =
                Math.Atan2(args.Position.Y - yAxis.Transform(y), args.Position.X - xAxis.Transform(x)) * 180 / Math.PI;

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawText_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            ((TextAnnotation) _tempAnnot).Background = OxyColors.Undefined;

            if (string.IsNullOrEmpty(uiSetTextTextBox.Text))
                using (InputTextDialog d = new InputTextDialog())
                {
                    d.DisplayText = uiSetTextTextBox.Text;

                    if (d.ShowDialog() == DialogResult.OK)
                        ((TextAnnotation) _tempAnnot).Text = d.DisplayText;
                    else
                        ChartModel.Annotations.Remove(_tempAnnot);
                }
            else 
                ((TextAnnotation) _tempAnnot).Text = uiSetTextTextBox.Text;

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void SelectAnnot_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (((Annotation) sender).IsSelected())
                ((Annotation) sender).Unselect();
            else
                ((Annotation) sender).Select();

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void uiDeleteAnnotButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                foreach (Annotation annot in ChartModel.Annotations)
                    annot.MouseDown += DeleteAnnot_OnMouseDown;
            }
            else
                foreach (Annotation annot in ChartModel.Annotations)
                    annot.MouseDown -= DeleteAnnot_OnMouseDown;
        }

        private void uiDrawArrowButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowPolylineEditingToolbarItems();
                ChartModel.MouseDown += DrawArrow_OnMouseDown;
                ChartModel.MouseMove += DrawArrow_OnMouseMove;
                ChartModel.MouseUp += DrawArrow_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawArrow_OnMouseDown;
                ChartModel.MouseMove -= DrawArrow_OnMouseMove;
                ChartModel.MouseUp -= DrawArrow_OnMouseUp;
            }
        }

        private void uiDrawEllipseButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowShapeEditingToolbarItems();
                ChartModel.MouseDown += DrawEllipse_OnMouseDown;
                ChartModel.MouseMove += DrawEllipse_OnMouseMove;
                ChartModel.MouseUp += DrawEllipse_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawEllipse_OnMouseDown;
                ChartModel.MouseMove -= DrawEllipse_OnMouseMove;
                ChartModel.MouseUp -= DrawEllipse_OnMouseUp;
            }
        }
        
        private void uiDrawLineButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowLineEditingToolbarItems();
                ChartModel.MouseDown += DrawLine_OnMouseDown;
                ChartModel.MouseMove += DrawLine_OnMouseMove;
                ChartModel.MouseUp += DrawLine_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawLine_OnMouseDown;
                ChartModel.MouseMove -= DrawLine_OnMouseMove;
                ChartModel.MouseUp -= DrawLine_OnMouseUp;
            }
        }

        private void uiDrawPolygonButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowShapeEditingToolbarItems();
                ChartModel.MouseDown += DrawPolygon_OnMouseDown;
                ChartModel.MouseMove += DrawPolyline_OnMouseMove;
                ChartModel.MouseUp += DrawPolygon_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawPolygon_OnMouseDown;
                ChartModel.MouseMove -= DrawPolyline_OnMouseMove;
                ChartModel.MouseUp -= DrawPolygon_OnMouseUp;
            }
        }

        private void uiDrawPolylineButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowPolylineEditingToolbarItems();
                ChartModel.MouseDown += DrawPolyline_OnMouseDown;
                ChartModel.MouseMove += DrawPolyline_OnMouseMove;
                ChartModel.MouseUp += DrawPolyline_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawPolyline_OnMouseDown;
                ChartModel.MouseMove -= DrawPolyline_OnMouseMove;
                ChartModel.MouseUp -= DrawPolyline_OnMouseUp;
            }
        }

        private void uiDrawRectangleButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowRectangleEditingToolbarItems();
                ChartModel.MouseDown += DrawRectangle_OnMouseDown;
                ChartModel.MouseMove += DrawRectangle_OnMouseMove;
                ChartModel.MouseUp += DrawRectangle_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawRectangle_OnMouseDown;
                ChartModel.MouseMove -= DrawRectangle_OnMouseMove;
                ChartModel.MouseUp -= DrawRectangle_OnMouseUp;
            }
        }

        private void uiDrawTextButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowTextEditingToolbarItems();
                ChartModel.MouseDown += DrawText_OnMouseDown;
                ChartModel.MouseMove += DrawText_OnMouseMove;
                ChartModel.MouseUp += DrawText_OnMouseUp;
            }
            else
            {
                ChartModel.MouseDown -= DrawText_OnMouseDown;
                ChartModel.MouseMove -= DrawText_OnMouseMove;
                ChartModel.MouseUp -= DrawText_OnMouseUp;
            }
        }

        private void uiSelectAnnotButton_OnCheckedChanged(object sender, EventArgs args)
        {
            RestoreDefaultToolbarSetup();

            if (((ToolStripButton) sender).Checked)
            {
                ResetItemsToggle((ToolStripButton) sender);
                ShowAnnotEditingToolbarItems();
                foreach (Annotation a in ChartModel.Annotations)
                    a.MouseDown += SelectAnnot_OnMouseDown;
            }
            else
                foreach (Annotation a in ChartModel.Annotations)
                    a.MouseDown -= SelectAnnot_OnMouseDown;
        }

        private void uiSetFillAlphaTextBox_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                EllipseAnnotation ellipseAnnot = a as EllipseAnnotation;
                if (ellipseAnnot != null)
                    ellipseAnnot.Fill = OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text),
                        ellipseAnnot.Fill);

                PolygonAnnotation polyAnnot = a as PolygonAnnotation;
                if (polyAnnot != null)
                    polyAnnot.Fill = OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text), polyAnnot.Fill);

                RectangleAnnotation rectAnnot = a as RectangleAnnotation;
                if (rectAnnot != null)
                    rectAnnot.Fill = OxyColor.FromAColor(Convert.ToByte(uiSetFillAlphaTextBox.Text), rectAnnot.Fill);
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetFillColorButton_OnClick(object sender, EventArgs args)
        {
            using (ColorDialog d = new ColorDialog())
            {
                d.Color = uiSetFillColorButton.BackColor;

                if (d.ShowDialog() != DialogResult.OK)
                    return;

                uiSetFillColorButton.BackColor = d.Color;

                if (uiSelectAnnotButton.Checked)
                    foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                    {
                        EllipseAnnotation ellipseAnnot = a as EllipseAnnotation;
                        if (ellipseAnnot != null)
                            ellipseAnnot.Fill = OxyColor.FromAColor(ellipseAnnot.Fill.A, d.Color.ToOxyColor());

                        PolygonAnnotation polygonAnnot = a as PolygonAnnotation;
                        if (polygonAnnot != null)
                            polygonAnnot.Fill = OxyColor.FromAColor(polygonAnnot.Fill.A, d.Color.ToOxyColor());

                        RectangleAnnotation rectAnnot = a as RectangleAnnotation;
                        if (rectAnnot != null)
                            rectAnnot.Fill = OxyColor.FromAColor(rectAnnot.Fill.A, d.Color.ToOxyColor());
                    }

                ChartModel.InvalidatePlot(false);
            }
        }

        private void uiSetLayerComboBox_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            if (!uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                a.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedIndex;

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetLineColorButton_OnClick(object sender, EventArgs args)
        {
            using (ColorDialog d = new ColorDialog())
            {
                d.Color = uiSetLineColorButton.BackColor;

                if (d.ShowDialog() != DialogResult.OK)
                    return;

                uiSetLineColorButton.BackColor = d.Color;

                if (uiSelectAnnotButton.Checked)
                    foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                    {
                        ArrowAnnotation arrowAnnot = a as ArrowAnnotation;
                        if (arrowAnnot != null)
                            arrowAnnot.Color = OxyColor.FromAColor(arrowAnnot.Color.A, d.Color.ToOxyColor());
                        
                        // This will cover both Line and Polyline annotations
                        PathAnnotation pathAnnot = a as PathAnnotation;
                        if (pathAnnot != null)
                            pathAnnot.Color = OxyColor.FromAColor(pathAnnot.Color.A, d.Color.ToOxyColor());
                    }

                ChartModel.InvalidatePlot(false);
            }
        }

        private void uiSetLineAlphaTextBox_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter || !uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                ArrowAnnotation arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                    arrowAnnot.Color = OxyColor.FromAColor(Convert.ToByte(uiSetLineAlphaTextBox.Text), arrowAnnot.Color);

                PathAnnotation pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                    pathAnnot.Color = OxyColor.FromAColor(Convert.ToByte(uiSetLineAlphaTextBox.Text), pathAnnot.Color);
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetLineStyleComboBox_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            if (!uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                ArrowAnnotation arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                    arrowAnnot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;

                PathAnnotation pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                    pathAnnot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetLineThicknessTextBox_OnKeyPress(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter || !uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                ArrowAnnotation arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                    arrowAnnot.StrokeThickness = Convert.ToDouble(uiSetLineThicknessTextBox.Text);

                PathAnnotation pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                    pathAnnot.StrokeThickness = Convert.ToDouble(uiSetLineThicknessTextBox.Text);
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetTextTextBox_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter || !uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                ((TextualAnnotation) a).Text = uiSetTextTextBox.Text;

            ChartModel.InvalidatePlot(false);
        }
    }
}
