using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace TangramApp1._35.Controls.Tangram
{
    /// <summary>
    /// Not really a 
    /// </summary>
    public class PuzzleData
    {

        private string _puzzlename;

        /// <summary>
        /// The name of the puzzle.
        /// For Library use
        /// </summary>
        public string PuzzleName
        {
            get { return _puzzlename; }
            set { _puzzlename = value; }
        }

        private float scale = 1.0f;

        /// <summary>
        /// Changing the scale of the PuzzleData after installation does
        /// not change scale of the Puzzle and its pieces.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        
        /**
         * These are the puzzle pieces necessary to complete the puzzle.
         * These polygons are already scaled and textured, waiting to be used.
         */
        private List<DraggablePolygon> _puzzlepieces = new List<DraggablePolygon>();

        public List<DraggablePolygon> PuzzlePieces
        {
            get { return _puzzlepieces; }
            set { _puzzlepieces = value; }
        }

        /// <summary>
        /// These are the the polygons that define the outline of the puzzle.
        /// They are already scaled, translated, rotated, and textured
        /// </summary>
        private List<DraggablePolygon> _puzzlepolygons = new List<DraggablePolygon>();

        public List<DraggablePolygon> PuzzlePolygons
        {
            get { return _puzzlepolygons; }
            set { _puzzlepolygons = value; }
        }

        /**
         * All elements that were installed are placed here.
         */
        private List<UIElement> _installedElements = new List<System.Windows.UIElement>();

        /// <summary>
        /// Installs all of the necessary puzzle data.
        /// </summary>
        /// <param name="target"></param>
        public void install(Canvas target)
        {
            foreach (DraggablePolygon dp in PuzzlePolygons)
            {
                _installedElements.Add(dp);
                target.Children.Add(dp);
            }

            foreach (DraggablePolygon pp in PuzzlePieces)
            {
                //pp.TranslateTransform.X += 200;
                //pp.TranslateTransform.Y += 200;

                _installedElements.Add(pp);
                target.Children.Add(pp);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void Uninstall(Canvas target)
        {
            foreach(UIElement elem in _installedElements){
                target.Children.Remove(elem);
            }
            _installedElements.Clear();
        }
    }
}
