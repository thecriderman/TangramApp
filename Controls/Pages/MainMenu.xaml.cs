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
using System.Runtime.Serialization;

namespace TangramApp1._35.Controls.Pages
{
    /// <summary>
    /// MainMenu does not contain any extra junk.
    /// </summary>
    public partial class MainMenu : TouchMenuPage
    {
        /// <summary>
        /// 
        /// </summary>
        public MainMenu() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// TODO: ADD INFO
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MainMenu(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InitializeComponent();
        }
    }
}
