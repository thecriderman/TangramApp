using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// TODO: FINISH
    /// </summary>
    public class PathedTextBlock : FrameworkElement
    {
        /*public static readonly DependencyProperty FontFamilyProperty =
            TextElement.FontFamilyProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));

        public static readonly DependencyProperty FontStyleProperty =
            TextElement.FontStyleProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty =
            TextElement.FontWeightProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));

        public static readonly DependencyProperty FontStretchProperty =
            TextElement.FontStretchProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnFontPropertyChanged));

        public static readonly DependencyProperty ForegroundProperty =
            TextElement.ForegroundProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnForegroundPropertyChanged));

        public static readonly DependencyProperty TextProperty =
            TextBlock.TextProperty.AddOwner(typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnTextPropertyChanged));

        public static readonly DependencyProperty PathFigureProperty =
            DependencyProperty.Register("PathFigure",
                typeof(PathFigure),
                typeof(PathedTextBlock),
                new FrameworkPropertyMetadata(OnPathPropertyChanged));

        // Properties
        public FontFamily FontFamily
        {
            set { SetValue(FontFamilyProperty, value); }
            get { return (FontFamily)GetValue(FontFamilyProperty); }
        }

        public FontStyle FontStyle
        {
            set { SetValue(FontStyleProperty, value); }
            get { return (FontStyle)GetValue(FontStyleProperty); }
        }

        public FontWeight FontWeight
        {
            set { SetValue(FontWeightProperty, value); }
            get { return (FontWeight)GetValue(FontWeightProperty); }
        }

        public FontStretch FontStretch
        {
            set { SetValue(FontStretchProperty, value); }
            get { return (FontStretch)GetValue(FontStretchProperty); }
        }

        public Brush Foreground
        {
            set { SetValue(ForegroundProperty, value); }
            get { return (Brush)GetValue(ForegroundProperty); }
        }

        public string Text
        {
            set { SetValue(TextProperty, value); }
            get { return (string)GetValue(TextProperty); }
        }

        public PathFigure PathFigure
        {
            set { SetValue(PathFigureProperty, value); }
            get { return (PathFigure)GetValue(PathFigureProperty); }
        }

        /// <summary>
        /// UTIL
        /// </summary>
        /// <param name="pathFigure"></param>
        /// <returns></returns>
        public static double GetPathFigureLength(PathFigure pathFigure)
        {
            if (pathFigure == null)
                return 0;

            bool isAlreadyFlattened = true;

            foreach (PathSegment pathSegment in pathFigure.Segments)
            {
                if (!(pathSegment is PolyLineSegment) && !(pathSegment is LineSegment))
                {
                    isAlreadyFlattened = false;
                    break;
                }
            }

            PathFigure pathFigureFlattened = isAlreadyFlattened ? pathFigure : pathFigure.GetFlattenedPathFigure();
            double length = 0;
            Point pt1 = pathFigureFlattened.StartPoint;

            foreach (PathSegment pathSegment in pathFigureFlattened.Segments)
            {
                if (pathSegment is LineSegment)
                {
                    Point pt2 = (pathSegment as LineSegment).Point;
                    length += (pt2 - pt1).Length;
                    pt1 = pt2;
                }
                else if (pathSegment is PolyLineSegment)
                {
                    PointCollection pointCollection = (pathSegment as PolyLineSegment).Points;
                    foreach (Point pt2 in pointCollection)
                    {
                        length += (pt2 - pt1).Length;
                        pt1 = pt2;
                    }
                }
            }
            return length;
        }

         Typeface typeface;
        protected VisualCollection visualChildren;
        protected List<FormattedText> formattedChars = new List<FormattedText>();
        protected double pathLength;
        protected double textLength;
        protected Rect boundingRect = new Rect();

        public TextOnPathVisuals()
        {
            typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            visualChildren = new VisualCollection(this);
        }

        protected override void OnPathPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            pathLength = GetPathFigureLength(PathFigure);
            TransformVisualChildren();
        }

        protected override void OnFontPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            OnTextPropertyChanged(args);
        }

        protected override void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            OnTextPropertyChanged(args);
        }

        protected override void OnTextPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            formattedChars.Clear();
            textLength = 0;

            foreach (char ch in Text)
            {
                FormattedText formattedText =
                    new FormattedText(ch.ToString(), CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight, typeface, 100, Foreground);

                formattedChars.Add(formattedText);
                textLength += formattedText.WidthIncludingTrailingWhitespace;
            }

            GenerateVisualChildren();
        }

        protected virtual void GenerateVisualChildren()
        {
            visualChildren.Clear();

            foreach (FormattedText formText in formattedChars)
            {
                DrawingVisual drawingVisual = new DrawingVisual();

                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new ScaleTransform());
                transformGroup.Children.Add(new RotateTransform());
                transformGroup.Children.Add(new TranslateTransform());
                drawingVisual.Transform = transformGroup;

                DrawingContext dc = drawingVisual.RenderOpen();
                dc.DrawText(formText, new Point(0, 0));
                dc.Close();

                visualChildren.Add(drawingVisual);
            }

            TransformVisualChildren();
        }

        protected virtual void TransformVisualChildren()
        {
            boundingRect = new Rect();

            if (pathLength == 0 || textLength == 0)
                return;

            if (formattedChars.Count != visualChildren.Count)
                return;

            double scalingFactor = pathLength / textLength;
            PathGeometry pathGeometry = 
                new PathGeometry(new PathFigure[] { PathFigure });
            double progress = 0;
            boundingRect = new Rect();

            for (int index = 0; index < visualChildren.Count; index++)
            {
                FormattedText formText = formattedChars[index];
                double width = scalingFactor * 
                            formText.WidthIncludingTrailingWhitespace;
                double baseline = scalingFactor * formText.Baseline;
                progress += width / 2 / pathLength;
                Point point, tangent;
                pathGeometry.GetPointAtFractionLength(progress, 
                                            out point, out tangent);

                DrawingVisual drawingVisual = 
                    visualChildren[index] as DrawingVisual;
                TransformGroup transformGroup = 
                    drawingVisual.Transform as TransformGroup;
                ScaleTransform scaleTransform = 
                    transformGroup.Children[0] as ScaleTransform;
                RotateTransform rotateTransform = 
                    transformGroup.Children[1] as RotateTransform;
                TranslateTransform translateTransform = 
                    transformGroup.Children[2] as TranslateTransform;

                scaleTransform.ScaleX = scalingFactor;
                scaleTransform.ScaleY = scalingFactor;
                rotateTransform.Angle = 
                    Math.Atan2(tangent.Y, tangent.X) * 180 / Math.PI;
                rotateTransform.CenterX = width / 2;
                rotateTransform.CenterY = baseline;
                translateTransform.X = point.X - width / 2;
                translateTransform.Y = point.Y - baseline;

                Rect rect = drawingVisual.ContentBounds;
                rect.Transform(transformGroup.Value);
                boundingRect.Union(rect);

                progress += width / 2 / pathLength;
            }
            InvalidateMeasure();
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return visualChildren.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visualChildren.Count)
                throw new ArgumentOutOfRangeException("index");

            return visualChildren[index];
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return (Size)boundingRect.BottomRight;
        }*/
    }
}
