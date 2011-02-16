using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TangramApp1._35.Controls
{
    public class PieSlice : Shape
    {
        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register(
            "OuterRadius", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            "InnerRadius", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Calculations of angles are better done here
        /// </summary>
        public static readonly DependencyProperty RadiusThrustProperty = DependencyProperty.Register(
            "RadiusThrust", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
            "StartAngle", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
            "EndAngle", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(
            "CenterX", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(
            "CenterY", typeof(double), typeof(PieSlice),
            new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }

        public double RadiusThrust
        {
            get { return (double)GetValue(RadiusThrustProperty); }
            set { SetValue(RadiusThrustProperty, value); }
        }

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get{
                StreamGeometry geom = new StreamGeometry();
                geom.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext context = geom.Open())
                {
                    DrawGeometry(context);
                }

                geom.Freeze();

                return geom;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected void DrawGeometry(StreamGeometryContext context)
        {
            Point offset = new Point(
                (double.IsNaN(CenterX)) ? Width / 2.0 : CenterX,
                (double.IsNaN(CenterY)) ? Height / 2.0 : CenterY);

            double angleSpan = Math.Abs(EndAngle - StartAngle);

            Point thrustPoint = PolarToCartesian(StartAngle + angleSpan/2.0, RadiusThrust, offset);
            Point outerArcStartPoint = PolarToCartesian(StartAngle, OuterRadius, offset);
            Point outerArcEndPoint = PolarToCartesian(EndAngle, OuterRadius, offset);

            //using LERP, calculate the innerArc points, sadly we shall be short changing
            Point innerArcStartPoint = PolarToCartesian(StartAngle, InnerRadius - RadiusThrust, thrustPoint);
            Point innerArcEndPoint = PolarToCartesian(EndAngle, InnerRadius - RadiusThrust, thrustPoint);

            bool largeArc = angleSpan > 180.0;

            double innerArcDim = Math.Max(0.0, InnerRadius - RadiusThrust);

            Size outerArcSize = new Size(OuterRadius, OuterRadius);
            Size innerArcSize = new Size(innerArcDim, innerArcDim);

            context.BeginFigure(outerArcStartPoint, true, true);
            //context.LineTo(outerArcStartPoint, true, true);
            context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArc, SweepDirection.Clockwise, true, true);
            context.LineTo(innerArcEndPoint, true, true);
            context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArc, SweepDirection.Counterclockwise, true, true);
        }

        protected static Point PolarToCartesian(double angle, double radius, Point offset)
        {
            double angleRad = (Math.PI / 180.0) * (angle);
            return new Point(
                radius * Math.Cos(angleRad) + offset.X,
                radius * Math.Sin(angleRad) + offset.Y);
        }
    }
}
