using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace TangramApp1._35.Parsers.Tangram
{
    public class XMLTangramPuzzlePreviewParser : XMLParser<Canvas>
    {

        private double _previewScale = 90;

        /// <summary>
        /// 
        /// </summary>
        public double PreviewScale
        {
            get { return _previewScale; }
            set { _previewScale = value; }
        }

        protected override Canvas Parse(System.Xml.Linq.XDocument doc)
        {
            XElement head = doc.Element("puzzle");

            Canvas finishedCanvas = new Canvas();
            foreach (XElement elem in head.Elements("polygon"))
            {
                Polygon p = ParsePolygon(elem);
                p.Fill = Brushes.LightGreen;
                p.Stroke = Brushes.Black;
                p.StrokeThickness = 1;

                finishedCanvas.Children.Add(p);
            }
            FixPreview(finishedCanvas);

            return finishedCanvas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        private void FixPreview(Canvas c)
        {
            double lx = double.MaxValue;
            double ly = double.MaxValue;
            double mx = double.MinValue;
            double my = double.MinValue;

            foreach (Polygon dpoly in c.Children.OfType<Polygon>())
            {
                foreach (Point dp in dpoly.Points)
                {
                    if (dp.X < lx)
                    {
                        lx = dp.X;
                    }
                    else if (dp.X > mx)
                    {
                        mx = dp.X;
                    }
                    if (dp.Y < ly)
                    {
                        ly = dp.Y;
                    }
                    else if (dp.Y > my)
                    {
                        my = dp.Y;
                    }
                }
            }

            //@TODO: FIX THIS
            foreach (Polygon dpoly in c.Children.OfType<Polygon>())
            {
                PointCollection collect = dpoly.Points;

                for(int i = 0; i < collect.Count; i++)
                {
                    Point dp = collect[i];
                    dp.X += (mx - lx) / 2.0;
                    dp.Y += (my - ly) / 2.0; 
                    collect[i] = dp;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private Polygon ParsePolygon(XElement head)
        {
            Polygon endPoly = new Polygon();
            PointCollection pc = new PointCollection();

            //parse the point data
            foreach (XElement xpoint in head.Descendants("point"))
            {
                Point ppoint = new Point(
                    ParseDouble(xpoint.Attribute("x"), 0.0),
                    ParseDouble(xpoint.Attribute("y"), 0.0));

                //make it slightly bigger than normal to prevent perfectionism
                ppoint.X *= _previewScale;
                ppoint.Y *= _previewScale;
                pc.Add(ppoint);
            }

            endPoly.Points = pc;
            return endPoly;
        }
    }
}
