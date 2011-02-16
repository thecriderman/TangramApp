using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows.Media;
using System.Windows;

namespace TangramApp1._35.Parsers.Tangram
{
    /// <summary>
    ///Class used for loading polygon data from an XML.
    /// </summary>
    public class XMLPolygonParser : XMLParser<Polygon>
    {
        public static readonly Color DEFAULT_FILLCOLOR = Colors.Gray;
        public static readonly Color DEFAULT_STROKECOLOR = Colors.Black;

        protected override Polygon Parse(XDocument doc)
        {
            //the item to create
            Polygon p = new Polygon();
            PointCollection pts = new PointCollection();

            XElement root = doc.Element("polygon");

            //used for Library storage.
            p.Name = ParseString(root.Attribute("name"), UNIQUENAME++.ToString());

            foreach(XElement pointElement in root.Elements("point"))
            {
                pts.Add(ParsePoint(pointElement)); //SIMPLE
            }

            p.Points = pts;

            //create a default background and border.
            p.Fill = new SolidColorBrush(DEFAULT_FILLCOLOR);
            p.Stroke = new SolidColorBrush(DEFAULT_STROKECOLOR);
            p.StrokeThickness = 3;

            return p;
        }

    }
}
