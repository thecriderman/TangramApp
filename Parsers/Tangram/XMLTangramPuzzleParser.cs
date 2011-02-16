using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangramApp1._35.Controls.Tangram;
using System.Xml.Linq;
using TangramApp1._35.Libraries.Tangram;
using System.Windows.Shapes;

namespace TangramApp1._35.Parsers.Tangram
{
    public class XMLTangramPuzzleParser : XMLParser<PuzzleData>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        protected override PuzzleData Parse(System.Xml.Linq.XDocument doc)
        {
            PuzzleData puzzData = new PuzzleData();
            
            XElement root = doc.Element("puzzle");
            XElement needsRoot = root.Element("needs");

            puzzData.Scale = (float)ParseDouble(root.Attribute("scale"), 300.0);
            puzzData.PuzzleName = ParseString(root.Attribute("name"), UNIQUENAME++.ToString());

            parseShapes(needsRoot, puzzData);

            return puzzData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="data"></param>
        private void parseShapes(XElement root, PuzzleData data)
        {
            Console.WriteLine(root == null);
            Console.WriteLine(data);

            PolyLibrary plib = PolyLibrary.getInstance();

            foreach (XElement shapeElem in root.Elements("shape"))
            {
                string shapeName = ParseString(shapeElem.Attribute("name"), "ERROR");
                Polygon poly = plib.DerivePolygon(shapeName, data.Scale);

                DraggablePolygon dpoly = new DraggablePolygon();
                dpoly.TranslateTransform.X = ParseDouble(shapeElem.Attribute("x"), 0.0);
                dpoly.TranslateTransform.Y = ParseDouble(shapeElem.Attribute("y"), 0.0);
                dpoly.RotateTransform.Angle = ParseDouble(shapeElem.Attribute("angle"), 0.0);

                dpoly.Polygon = poly;

                data.PuzzlePieces.Add(dpoly);
            }
        }
    }
}
