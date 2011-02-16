using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

using System.Xml.Serialization;
using System.Xml;
using libSMARTMultiTouch.Content;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Resources;
using System.Xml.Linq;

namespace TangramApp1._35.Parsers
{
    /**
     * Abstract class that allows for parsing 
     * of specially serialized objects.
     */
    public abstract class XMLParser<T>
    {
        /// <summary>
        /// When an object needs to be stored within a Library,
        /// we can provide a default name based on the value of this counter.
        /// </summary>
        public static uint UNIQUENAME = 0;

        private string uripath;

        public XMLParser() : this(null)
        {
        }

        public XMLParser(string uri){
            uripath = uri;
        }

        public string UriPath{
            set
            {
                uripath = value;
            }
            get
            {
                return uripath;
            }
        }

        /// <summary>
        /// Loads the file internally.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public T Load(string uri)
        {
            uripath = uri;
            return Load();
        }

        /// <summary>
        /// Loads an external file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadPLEASE(string path)
        {
            XDocument xmlDoc = XDocument.Load(new XmlTextReader(new StreamReader(path)));
            return Parse(xmlDoc);
        }

        public T Load()
        {
            StreamResourceInfo info = Application.GetResourceStream(new Uri(uripath));
            XDocument xmlDoc = XDocument.Load(new XmlTextReader(info.Stream));
            return Parse(xmlDoc);
        }

        protected abstract T Parse(XDocument doc);

#region QUICKPARSERS

        /// <summary>
        /// Redundant, maybe, but I have a task to complete with this code.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static String ParseString(XAttribute attribute, String defaultValue)
        {
            if (attribute == null)
            {
                return defaultValue;
            }
            return attribute.Value; //simple, elegant.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseInt(XAttribute attribute, int defaultValue)
        {
            if (attribute == null)
            {
                return defaultValue;
            }
            return int.Parse(attribute.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ParseFloat(XAttribute attribute, float defaultValue)
        {
            if (attribute == null)
            {
                return defaultValue;
            }
            return float.Parse(attribute.Value);
        }

        public static double ParseDouble(XAttribute attribute, double defaultValue)
        {
            if (attribute == null)
            {
                return defaultValue;
            }
            return double.Parse(attribute.Value);
        }

        public static Color ParseColor(XAttribute attribute)
        {
            if (attribute == null)
            {
                return Colors.Transparent;
            }
            return ParseColor(attribute.Value);
        }


        /// <summary>
        /// Parses a string into a set Color.
        /// Supports strings as a color name, ie red, Purple, GREEN.
        /// Supports various color functions:
        ///   <list>
        ///     <item><term>color lighter(color c, double factor)</term>
        ///       <description>Lightens the color by a certain factor. Factor should be a number between 0.0 and 1.0. The supplied color can be a color function.</description>
        ///     </item>
        ///     <item><term>color darker(color c, double factor)</term>
        ///       <description>Darkens the color by a certain factor. Factor should be a number between 0.0 and 1.0. The supplied color can be a color function.</description>
        ///     </item>
        ///     <item><term>color random()</term>
        ///       <description>Creates an opaque color with randomized RGB values.</description>
        ///     </item>
        ///     <item><term>color random(color c...)</term>
        ///       <description>Randomly selects a color from the list of parameters. Parameters can be colors or color functions.</description>
        ///     </item>
        ///   </list>
        /// </summary>
        /// <param name="str"></param>
        /// <returns>The parsed color.</returns>
        protected static Color ParseColor(string str)
        {
            Color color = Colors.Transparent;

            //CASE 1: THE COLOR IS 'random'
            if (str.StartsWith("random"))
            {
                string[] args = GetParams(str);
                if (args.Length == 0)
                {
                    byte r = (byte)App.RANDOM.Next(256);
                    byte g = (byte)App.RANDOM.Next(256);
                    byte b = (byte)App.RANDOM.Next(256);
                    color = Color.FromRgb(r, g, b);
                    return color;
                }
                else
                {
                    //random with parameters means that a selection should be made from the parameters randomly.
                    //ie random(red,blue,#3300FF,random) randomly chooses from red, blue, #3300FF, and a random color
                    //nesting is supported.
                    return ParseColor(args[App.RANDOM.Next(args.Length)]);
                }
            }
            else if (str.StartsWith("darker("))
            {
                string[] args = GetParams(str);
                if (args.Length < 2)
                {
                    throw new System.ArgumentException("funct \'darker\' requires 2 arguments " + args[0]);
                }

                float multiplier = float.Parse(args[1]);
                Color baseColor = ParseColor(args[0]);
                byte alpha = baseColor.A; //preserve alpha
                baseColor *= multiplier;
                baseColor.A = alpha;
                return baseColor;
            }
            else if (str.StartsWith("lighter("))
            {
                string[] args = GetParams(str);
                if (args.Length < 2)
                {
                    throw new System.ArgumentException("funct \'lighter\' requires 2 arguments");
                }

                float multiplier = 1.0f + float.Parse(args[1]);
                Color baseColor = ParseColor(args[0]);
                byte alpha = baseColor.A; //preserve alpha
                baseColor *= multiplier;
                baseColor.A = alpha;
                return baseColor;
            }

            try
            {
                System.Drawing.Color dcolor = System.Drawing.ColorTranslator.FromHtml(str);
                color = Color.FromArgb(dcolor.A, dcolor.R, dcolor.G, dcolor.B); //conversion
                return color;
            }
            catch (Exception)
            {
            }

            return color; //DEFAULT
        }

        protected static string[] GetParams(string str)
        {
            List<string> parameters = new List<string>(5);

            //find the first instance of '(' and last instance of ')'
            int start = str.IndexOf('(') + 1;

            int depth = 0;
            for (int i = start + 1; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '(':
                        depth++;
                        break;
                    case ')':
                        if (--depth <= 0)
                        {
                            parameters.Add(str.Substring(start, i - start));
                            i = str.Length; //END
                        }
                        break;
                    case ',':
                        if (depth == 0)
                        {
                            parameters.Add(str.Substring(start, i - start));
                            start = i + 1;
                        }
                        break;
                }
            }
            return parameters.ToArray();
        }

        //parsers that use multiple attributes from an element.
#region SEMICOMPLEXPARSERS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointElement"></param>
        /// <returns></returns>
        public static Point ParsePoint(XElement pointElement)
        {
            return new Point(
                ParseDouble(pointElement.Attribute("x"), 0.0),
                ParseDouble(pointElement.Attribute("y"), 0.0));
        }

#endregion

#endregion
    }
}
