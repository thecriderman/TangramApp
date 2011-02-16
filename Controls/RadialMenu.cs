using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using TangramApp1._35.Controls.Events;

namespace TangramApp1._35.Controls
{
    public class RadialMenu : Panel, ICentroid
    {
        //Properties unique to each item added to the list.
        //public static readonly DependencyProperty ItemAngleProperty 

        public static readonly DependencyProperty ItemRadiusProperty = DependencyProperty.RegisterAttached(
            "Radius", typeof(double), typeof(RadialMenu), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange));

        public static double GetRadius(UIElement elem){
            return (double)elem.GetValue(ItemRadiusProperty);
        }
        public static void SetRadius(UIElement elem, double rad){
            elem.SetValue(ItemRadiusProperty, rad); //relies on AUTOBOXING
        }

        //Properties unique to this object
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
            "StartAngle", typeof(double), typeof(RadialMenu), new FrameworkPropertyMetadata(0d,   FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(OnStartAngleChanged)));
        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
            "EndAngle", typeof(double), typeof(RadialMenu), new FrameworkPropertyMetadata(360d, FrameworkPropertyMetadataOptions.AffectsArrange, new PropertyChangedCallback(OnEndAngleChanged)));

        //EVENTS
        public static readonly RoutedEvent StartAngleChangedEvent = EventManager.RegisterRoutedEvent(
            "StartAngleChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RadialMenu));
        public static readonly RoutedEvent EndAngleChangedEvent = EventManager.RegisterRoutedEvent(
            "EndAngleChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(RadialMenu));


        public event RoutedEventHandler StartAngleChanged
        {
            add{ AddHandler(StartAngleChangedEvent, value); }
            remove{ RemoveHandler(StartAngleChangedEvent, value); }
        }

        public event RoutedEventHandler EndAngleChanged
        {
            add{ AddHandler(EndAngleChangedEvent, value); }
            remove{ RemoveHandler(EndAngleChangedEvent, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnEndAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RadialMenu)d).OnEndAngleChanged(e);
        }

        public virtual void OnEndAngleChanged(DependencyPropertyChangedEventArgs args){
            base.RaiseEvent(new DependencyPropertyChangedRoutedEventArgs(
                EndAngleChangedEvent, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RadialMenu)d).OnStartAngleChanged(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnStartAngleChanged(DependencyPropertyChangedEventArgs args){
            base.RaiseEvent(new DependencyPropertyChangedRoutedEventArgs(
                StartAngleChangedEvent, args));
        }


        //
        private Behaviors.RadialMenuRotateAroundPointBehavior rotateAroundCenter;

        /// <summary>
        /// 
        /// </summary>
        public RadialMenu(): base()
        {
            Loaded += new RoutedEventHandler(RadialMenu_Loaded);
        }

        public bool IsTouchRotationEnabled
        {
            get
            {
                return rotateAroundCenter.IsRotateEnabled;
            }
            set
            {
                rotateAroundCenter.IsRotateEnabled = value;
            }
        }

        void RadialMenu_Loaded(object sender, RoutedEventArgs e)
        {
            rotateAroundCenter = new Behaviors.RadialMenuRotateAroundPointBehavior(this);
            /*foreach (TouchButton2 butt in Children.OfType<TouchButton2>())
            {
                Geometry geo = getStreamingGeometry(butt);
                if (geo != null)
                {
                    butt.ClipToBounds = false;
                    butt.Clip = geo;
                }
            }*/
        }

        public Geometry getStreamingGeometry(TouchButton2 button)
        {
            var value = button.Style;

            foreach (Setter setter in value.Setters)
            {
                if (setter.Value.GetType() == typeof(ControlTemplate))
                {
                    ControlTemplate temp = (ControlTemplate)setter.Value;
                    PieSlice slice = (PieSlice)temp.FindName("BackgroundSlice", button);
                    return slice.RenderedGeometry;
                }
            }
            return null;
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty);}
            set { SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get{ return (double)GetValue(EndAngleProperty); }
            set{ SetValue(EndAngleProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size infsize = new Size(Width, Height);
            foreach(UIElement elem in Children){
                //allows the child to have as much space as needed
                elem.Measure(infsize);
            }
            return base.MeasureOverride(availableSize);
        }

        //Arrange the chilldren
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
            {
                return base.ArrangeOverride(finalSize);
            }

            double orientAngle = StartAngle * (Math.PI / 180.0);
            double orientAngleIncrement = ((EndAngle-StartAngle)*(Math.PI/180.0) / Children.Count); //in rads            

            foreach (UIElement elem in Children)
            {
                double elemRadius = GetRadius(elem);
                double radiusX, radiusY;

                if (double.IsNaN(elemRadius))
                {
                    radiusX = finalSize.Width / 2 - elem.DesiredSize.Width / 2;
                    radiusY = finalSize.Height / 2 - elem.DesiredSize.Width / 2;
                }
                else
                {
                    //centered on the item
                    radiusX = elemRadius - elem.DesiredSize.Width / 2;
                    radiusY = elemRadius - elem.DesiredSize.Width / 2;
                }

                //TODO replace this
                RotateTransform rtran = (elem as FrameworkElement).RenderTransform as RotateTransform;
                if (rtran != null)
                {
                    rtran.Angle = -orientAngle * 180.0 / Math.PI;
                }
                else
                {
                    (elem as FrameworkElement).RenderTransform = new RotateTransform(-orientAngle * 180.0 / Math.PI, elem.DesiredSize.Width / 2.0, elem.DesiredSize.Height / 2.0);
                }

                Point childPoint = new Point(Math.Cos(orientAngle) * radiusX, -Math.Sin(orientAngle) * radiusY);

                //Offsetting the point to the Avalable rectangular area which is FinalSize.
                Point actualChildPoint = new Point(finalSize.Width / 2 + childPoint.X - elem.DesiredSize.Width / 2, finalSize.Height / 2 + childPoint.Y - elem.DesiredSize.Height / 2);

                //Call Arrange method on the child element by giving the calculated point as the placementPoint.
                elem.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y, elem.DesiredSize.Width, elem.DesiredSize.Height));

                //Calculate the new _angle for the next element
                orientAngle += orientAngleIncrement;
            }

            return finalSize;
        }

        #region ICentroid Members

        public Point getCentroid()
        {
            return new Point(Width / 2, Height / 2);
        }

        #endregion
    }
}
