using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows;

namespace TangramApp1._35.Controls.Tangram
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class DraggablePolygon : DraggableControl
    {
        public static readonly DependencyProperty PolygonProperty = DependencyProperty.Register("Polygon", typeof(Polygon), typeof(DraggablePolygon), new PropertyMetadata(new PropertyChangedCallback(PolygonChangedCallback)));

        /// <summary>
        /// Callback method called when the Polygon property has been changed.
        /// Updates the centroid and content information.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private static void PolygonChangedCallback(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            DraggablePolygon thisObj = (DraggablePolygon)src;
            Polygon value = (Polygon)args.NewValue;

            //calculate the centroid of the figure.
            double mx = 0.0, my = 0.0;
            foreach (Point p in value.Points)
            {
                mx += p.X;
                my += p.Y;
            }
            
            thisObj._calculatedCentroid = new Point(
                mx/value.Points.Count,
                my/value.Points.Count);

            //
            thisObj.Content = value;
        }
        
        public DraggablePolygon() : base()
        {
        }

        private Point _calculatedCentroid;

        /// <summary>
        /// 
        /// </summary>
        public Polygon Polygon
        {
            get { return (Polygon)GetValue(PolygonProperty); }
            set { SetValue(PolygonProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Point getCentroid()
        {
            //TODO: have it calculate the new centroid based on the rotation
            return _calculatedCentroid;
        }
    }
}
