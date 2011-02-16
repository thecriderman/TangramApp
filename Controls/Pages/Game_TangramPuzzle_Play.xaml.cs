using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TangramApp1._35.Parsers.Tangram;
using TangramApp1._35.Controls.Tangram;
using libSMARTMultiTouch.Controls;
using TangramApp1._35.Libraries.Tangram;

namespace TangramApp1._35.Controls.Pages
{
    /// <summary>
    /// Interaction logic for Game_TangramPuzzle_Play.xaml
    /// </summary>
    public partial class Game_TangramPuzzle_Play : TouchMenuPage
    {
        /// <summary>
        /// 
        /// </summary>
        public Game_TangramPuzzle_Play()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Does nothing currently.
        /// </summary>
        public override void InstallVotableButtons()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriLoc"></param>
        public void InitializePuzzle(string uriLoc)
        {
            XMLTangramPuzzleParser parser = new XMLTangramPuzzleParser();
            PuzzleData data = parser.Load(uriLoc);
            data.install(PlacementCanvas);
        }
    }
}
