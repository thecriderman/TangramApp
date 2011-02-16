using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using TangramApp1._35.Parsers.Tangram;
using System.Windows.Media;
using System.Windows;

namespace TangramApp1._35.Libraries.Tangram
{
    public class PolyLibrary : Library<Polygon>
    {
        private static PolyLibrary _libInstance;

        public static PolyLibrary getInstance()
        {
            if (_libInstance == null)
            {
                _libInstance = new PolyLibrary();
                _libInstance.LoadDefaultLibrary();
            }
            return _libInstance;
        }

        //TODO come up with a reference file.
        public static readonly string[] DEFAULTLIBITEMS = { "largetriangle", "mediumtriangle", "parallelogram", "smalltriangle", "square" };
        private static readonly string DEFAULTBASEPATH = "pack://application:,,,/Resources/XML/Tangram/shapes/";

        /// <summary>
        /// Loads the default polygon library.
        /// </summary>
        public override void LoadDefaultLibrary()
        {
            LoadLibrary(DEFAULTBASEPATH, ".xml", DEFAULTLIBITEMS);
        }

        /// <summary>
        /// Loads a specific Polygon from an XML File.
        /// </summary>
        /// <param name="fullitempath"></param>
        public override void LoadItem(string fullitempath)
        {
            XMLPolygonParser parser = new XMLPolygonParser();
            Polygon p = parser.Load(fullitempath);
            base.library.Add(p.Name, p);
        }

        /// <summary>
        /// Gets a Polygon from the library and properly scales it.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Polygon DerivePolygon(string key, double scale)
        {
            Polygon basePoly = this[key];
            PointCollection baseCollection = basePoly.Points;

            Polygon scaledPoly = new Polygon();
            PointCollection scaledCollection = new PointCollection();


            for (int i = 0; i < baseCollection.Count; i++)
            {
                Point bp = baseCollection[i];
                Point derivativePoint = new Point(bp.X * scale, bp.Y * scale);

                scaledCollection.Add(derivativePoint);
            }

            scaledPoly.Points = scaledCollection;
            
            //clone all other attributes
            scaledPoly.Fill = basePoly.Fill;
            scaledPoly.Stroke = basePoly.Stroke;

            return scaledPoly;
        }
    }
}
