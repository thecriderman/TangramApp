//#define DEBUGMODE
//#define FLAIRMODE

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
using System.Windows.Shapes;
using libSMARTMultiTouch.Controls;
using libSMARTMultiTouch.Table;
using libSMARTMultiTouch;
using System.IO;
using System.Windows.Resources;
using libSMARTMultiTouch.Input;
//1.35
using TangramApp1._35.Controls;
using TangramApp1._35.Controls.Pages;

namespace TangramApp1._35
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : TableApplicationControl
    {
        private TouchMenuPage currentPage;

        /// <summary>
        /// 
        /// </summary>
        public TableControl()
        {
            InitializeComponent();
        }

        private void TableApplicationControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReplaceCloseButton();
            //TableLayoutRoot.Children.Add(new MainMenu());
            TableLayoutRoot.Children.Add(currentPage = new Controls.Pages.Game_TangramPuzzle_SelectPuzzle());
            //TableLayoutRoot.Children.Add(currentPage = new Controls.Pages.Game_TangramPuzzle_Play());
            //TableLayoutRoot.Children.Add(new LoadScreen());
            //TableLayoutRoot.Children.Add(new StyledComponentDemo());
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReplaceCloseButton()
        {
            CornerDockPanel cornerDockPanel = (CornerDockPanel)((Grid)Content).Children[1];

            //wave goodbye to the close button of yesteryear
            cornerDockPanel.Children.Clear();

            VotableButtonTray tray = VotableButtonTray.getInstance();
            tray.Width = 600;
            tray.Height = 50;

            //populate the tray with an exit/cancel button
            populateTray();
            //QuickPopulate("  1  ");
            //QuickPopulate("  2  ");
            //QuickPopulate("  3  ");

            cornerDockPanel.Children.Add(tray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        private void QuickPopulate(string str)
        {
            VotableTouchButton2 button = new VotableTouchButton2();
            button.Width = button.Height = 50;

            button.IsVotable = true;
            button.VoteRequestWhen = VoteRequestFlags.OnInterval;
            button.VoteRequestInterval = 30000; //25 seconds
            button.VoteAutoCancelInterval = 15000;

            button.Content = str;

            button.Style = (Style)App.Current.FindResource("BraglButton");

            VotableButtonTray.getInstance().RegisterVotable(button);
        }

        /// <summary>
        /// Populates the tray with the standard
        /// </summary>
        private void populateTray()
        {
            VotableTouchButton2 exitCancelButton = new VotableExitCancelButton();
            exitCancelButton.Width = exitCancelButton.Height = 50;

            exitCancelButton.Style = (Style)App.Current.FindResource("UnstyledButton");
            Image icon = new Image();
            icon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/UI/CloseCancelButton.png"));
            exitCancelButton.Content = icon;

            VotableButtonTray.getInstance().RegisterVotable(exitCancelButton);
        }

        
    }
}
