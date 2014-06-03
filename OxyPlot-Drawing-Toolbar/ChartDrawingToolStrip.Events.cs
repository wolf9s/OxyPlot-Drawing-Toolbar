using System;
using System.Linq;
using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;

namespace OxyPlot_Drawing_Toolbar
{
    sealed partial class ChartDrawingToolStrip
    {
        /* Setup used when displaying annotation to user before drawn. */
        private const int HoverAlpha = 150;
        private const LineStyle HoverLineStyle = LineStyle.Dash;
        private const int HoverLineThickness = 4;
        private readonly OxyColor HoverColor = OxyColor.FromAColor(HoverAlpha, OxyColors.Cyan);

        /* Annotation used to draw on the chart. */
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

            var annot = _tempAnnot as ArrowAnnotation;
            if (annot != null)
                annot.EndPoint = xAxis.InverseTransform(args.Position.X, args.Position.Y, yAxis);

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawArrow_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as ArrowAnnotation;
            if (annot != null)
            {
                annot.Color = OxyColor.FromAColor(byte.Parse(uiSetLineAlphaTextBox.Text),
                                                  uiSetLineColorButton.BackColor.ToOxyColor());
                annot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
                annot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
                annot.StrokeThickness = double.Parse(uiSetLineThicknessTextBox.Text);
                annot.Text = uiSetTextTextBox.Text;
            }

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

            var annot = _tempAnnot as EllipseAnnotation;
            if (annot != null)
            {
                annot.Width = (xAxis.InverseTransform(args.Position.X)
                               - ((EllipseAnnotation) _tempAnnot).X) * 2;
                annot.Height = (yAxis.InverseTransform(args.Position.Y)
                                - ((EllipseAnnotation) _tempAnnot).Y) * 2;
            }

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawEllipse_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as EllipseAnnotation;
            if (annot != null)
            {
                annot.Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
                                                 uiSetFillColorButton.BackColor.ToOxyColor());
                annot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
                annot.Text = uiSetTextTextBox.Text;
            }

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
            var type = (LimitType) uiSetTypeComboBox.SelectedItem;

            var annot = _tempAnnot as LineAnnotation;
            if (type == LimitType.None)
            {
                annot.X = xAxis.InverseTransform(args.Position.X);
                annot.Y = yAxis.InverseTransform(args.Position.Y);
                annot.Intercept = yAxis.InverseTransform(args.Position.Y);
                annot.Slope = 0;
            }
            else if (type == LimitType.Vertical)
            {
                annot.Type = LineAnnotationType.Vertical;
                annot.X = xAxis.InverseTransform(args.Position.X);
            }
            else if (type == LimitType.Horizontal)
            {
                annot.Type = LineAnnotationType.Horizontal;
                annot.Y = yAxis.InverseTransform(args.Position.Y);
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
            var type = (LimitType) uiSetTypeComboBox.SelectedItem;

            var annot = _tempAnnot as LineAnnotation;
            if (annot != null)
            {
                if (type == LimitType.None)
                {
                    double x1 = annot.X;
                    double x2 = xAxis.InverseTransform(args.Position.X);
                    double y1 = annot.Y;
                    double y2 = yAxis.InverseTransform(args.Position.Y);
                    double slope = (y2 - y1) / (x2 - x1);

                    annot.Intercept = y2 - slope * x2;
                    annot.Slope = slope;
                }
                else if (type == LimitType.Vertical)
                    annot.X = xAxis.InverseTransform(args.Position.X);
                else if (type == LimitType.Horizontal)
                    annot.Y = yAxis.InverseTransform(args.Position.Y);
            }

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawLine_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as LineAnnotation;
            if (annot != null)
            {
                annot.Color = OxyColor.FromAColor(byte.Parse(uiSetLineAlphaTextBox.Text),
                                                  uiSetLineColorButton.BackColor.ToOxyColor());
                annot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
                annot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
                annot.StrokeThickness = double.Parse(uiSetLineThicknessTextBox.Text);
                annot.Text = uiSetTextTextBox.Text;
            }

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

            var polyAnnot = new PolygonAnnotation
            {
                Color = OxyColors.Undefined,
                Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
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

            ((PolylineAnnotation) _tempAnnot).Points.Add(xAxis.InverseTransform(args.Position.X,
                                                                                args.Position.Y,
                                                                                yAxis));

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
            ((PolylineAnnotation) _tempAnnot).Points.Add(xAxis.InverseTransform(args.Position.X,
                                                                                args.Position.Y,
                                                                                yAxis));

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawPolyline_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as PolylineAnnotation;
            if (annot != null)
            {
                annot.Color = OxyColor.FromAColor(byte.Parse(uiSetLineAlphaTextBox.Text),
                                                  uiSetLineColorButton.BackColor.ToOxyColor());
                annot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
                annot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
                annot.StrokeThickness = double.Parse(uiSetLineThicknessTextBox.Text);
                annot.Text = uiSetTextTextBox.Text;
            }

            _tempAnnot = null;
            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawRectangle_OnMouseDown(object sender, OxyMouseDownEventArgs args)
        {
            if (args.ChangedButton != OxyMouseButton.Left)
                return;

            _tempAnnot = new RectangleAnnotation
            {
                Fill = HoverColor,
                Layer = AnnotationLayer.AboveSeries
            };

            Axis xAxis = ChartModel.Axes.First(a => a.IsHorizontal());
            Axis yAxis = ChartModel.Axes.First(a => a.IsVertical());
            var type = (LimitType) uiSetTypeComboBox.SelectedItem;

            var annot = _tempAnnot as RectangleAnnotation;
            if (type == LimitType.None || type == LimitType.Vertical)
            {
                annot.MinimumX = xAxis.InverseTransform(args.Position.X);
                annot.MaximumX = xAxis.InverseTransform(args.Position.X);
            }
            if (type == LimitType.None || type == LimitType.Horizontal)
            {
                annot.MinimumY = yAxis.InverseTransform(args.Position.Y);
                annot.MaximumY = yAxis.InverseTransform(args.Position.Y);
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
            var type = (LimitType) uiSetTypeComboBox.SelectedItem;

            var annot = _tempAnnot as RectangleAnnotation;
            if (annot != null)
            {
                if (type == LimitType.None || type == LimitType.Vertical)
                    annot.MaximumX = xAxis.InverseTransform(args.Position.X);
                if (type == LimitType.None || type == LimitType.Horizontal)
                    annot.MaximumY = yAxis.InverseTransform(args.Position.Y);
            }

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawRectangle_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as RectangleAnnotation;
            if (annot != null)
            {
                annot.Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
                                                 uiSetFillColorButton.BackColor.ToOxyColor());
                annot.Layer = (AnnotationLayer) uiSetLayerComboBox.SelectedItem;
                annot.Text = uiSetTextTextBox.Text;
            }

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

            var annot = _tempAnnot as TextAnnotation;
            if (annot != null)
            {
                double x = annot.TextPosition.X;
                double y = annot.TextPosition.Y;

                annot.TextRotation = Math.Atan2(args.Position.Y - yAxis.Transform(y),
                                                args.Position.X - xAxis.Transform(x)) * 180
                                     / Math.PI;
            }

            ChartModel.InvalidatePlot(false);
            args.Handled = true;
        }

        private void DrawText_OnMouseUp(object sender, OxyMouseEventArgs args)
        {
            if (_tempAnnot == null)
                return;

            var annot = _tempAnnot as TextAnnotation;
            if (annot != null)
            {
                annot.Background = OxyColors.Undefined;

                if (string.IsNullOrEmpty(uiSetTextTextBox.Text))
                {
                    using (var d = new InputTextDialog())
                    {
                        d.DisplayText = uiSetTextTextBox.Text;

                        if (d.ShowDialog() == DialogResult.OK)
                            annot.Text = d.DisplayText;
                        else
                            ChartModel.Annotations.Remove(annot);
                    }
                }
                else
                    annot.Text = uiSetTextTextBox.Text;
            }

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

        private void uiDeleteAllAnnotsButton_OnClick(object sender, EventArgs args)
        {
            ChartModel.Annotations.Clear();
            ChartModel.InvalidatePlot(false);
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
            {
                foreach (Annotation annot in ChartModel.Annotations)
                    annot.MouseDown -= DeleteAnnot_OnMouseDown;
            }
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
            {
                foreach (Annotation a in ChartModel.Annotations)
                    a.MouseDown -= SelectAnnot_OnMouseDown;
            }
        }

        private void uiSetFillAlphaTextBox_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                var ellipseAnnot = a as EllipseAnnotation;
                if (ellipseAnnot != null)
                {
                    ellipseAnnot.Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
                                                            ellipseAnnot.Fill);
                }

                var polyAnnot = a as PolygonAnnotation;
                if (polyAnnot != null)
                {
                    polyAnnot.Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
                                                         polyAnnot.Fill);
                }

                var rectAnnot = a as RectangleAnnotation;
                if (rectAnnot != null)
                {
                    rectAnnot.Fill = OxyColor.FromAColor(byte.Parse(uiSetFillAlphaTextBox.Text),
                                                         rectAnnot.Fill);
                }
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetFillColorButton_OnClick(object sender, EventArgs args)
        {
            using (var d = new ColorDialog())
            {
                d.Color = uiSetFillColorButton.BackColor;

                if (d.ShowDialog() != DialogResult.OK)
                    return;

                uiSetFillColorButton.BackColor = d.Color;

                if (uiSelectAnnotButton.Checked)
                {
                    foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                    {
                        var ellipseAnnot = a as EllipseAnnotation;
                        if (ellipseAnnot != null)
                        {
                            ellipseAnnot.Fill = OxyColor.FromAColor(ellipseAnnot.Fill.A,
                                                                    d.Color.ToOxyColor());
                        }

                        var polygonAnnot = a as PolygonAnnotation;
                        if (polygonAnnot != null)
                        {
                            polygonAnnot.Fill = OxyColor.FromAColor(polygonAnnot.Fill.A,
                                                                    d.Color.ToOxyColor());
                        }

                        var rectAnnot = a as RectangleAnnotation;
                        if (rectAnnot != null)
                        {
                            rectAnnot.Fill = OxyColor.FromAColor(rectAnnot.Fill.A,
                                                                 d.Color.ToOxyColor());
                        }
                    }
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
            using (var d = new ColorDialog())
            {
                d.Color = uiSetLineColorButton.BackColor;

                if (d.ShowDialog() != DialogResult.OK)
                    return;

                uiSetLineColorButton.BackColor = d.Color;

                if (uiSelectAnnotButton.Checked)
                {
                    foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
                    {
                        var arrowAnnot = a as ArrowAnnotation;
                        if (arrowAnnot != null)
                        {
                            arrowAnnot.Color = OxyColor.FromAColor(arrowAnnot.Color.A,
                                                                   d.Color.ToOxyColor());
                        }

                        // This will cover both Line and Polyline annotations
                        var pathAnnot = a as PathAnnotation;
                        if (pathAnnot != null)
                        {
                            pathAnnot.Color = OxyColor.FromAColor(pathAnnot.Color.A,
                                                                  d.Color.ToOxyColor());
                        }
                    }
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
                var arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                {
                    arrowAnnot.Color = OxyColor.FromAColor(byte.Parse(uiSetLineAlphaTextBox.Text),
                                                           arrowAnnot.Color);
                }

                var pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                {
                    pathAnnot.Color = OxyColor.FromAColor(byte.Parse(uiSetLineAlphaTextBox.Text),
                                                          pathAnnot.Color);
                }
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetLineStyleComboBox_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            if (!uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                var arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                    arrowAnnot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;

                var pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                    pathAnnot.LineStyle = (LineStyle) uiSetLineStyleComboBox.SelectedItem;
            }

            ChartModel.InvalidatePlot(false);
        }

        private void uiSetLineThicknessTextBox_OnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode != Keys.Enter || !uiSelectAnnotButton.Checked)
                return;

            foreach (Annotation a in ChartModel.Annotations.Where(a => a.IsSelected()))
            {
                var arrowAnnot = a as ArrowAnnotation;
                if (arrowAnnot != null)
                    arrowAnnot.StrokeThickness = double.Parse(uiSetLineThicknessTextBox.Text);

                var pathAnnot = a as PathAnnotation;
                if (pathAnnot != null)
                    pathAnnot.StrokeThickness = double.Parse(uiSetLineThicknessTextBox.Text);
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
