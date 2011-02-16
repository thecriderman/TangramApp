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

namespace TangramApp1._35.Controls.Pages
{
    /// <summary>
    /// Interaction logic for LoadScreen.xaml
    /// </summary>
    public partial class LoadScreen : TouchMenuPage
    {
        private UIElement temporaryRemoved;

        public LoadScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This works backwards. Rather than installing a VotableTouchButton,
        /// it will be temporarily removing the VotableExitCancelButton.
        /// </summary>
        public override void InstallVotableButtons()
        {
            VotableButtonTray vtray = VotableButtonTray.getInstance();
            temporaryRemoved = vtray.Children[0];
            vtray.Children.RemoveAt(0);
        }

        /// <summary>
        /// This reinstalls the VotableExitCancelButton that the TouchMenuPage removed.
        /// </summary>
        public override void UninstallVotableButtons()
        {
            VotableButtonTray.getInstance().Children.Insert(0, temporaryRemoved);
            temporaryRemoved = null;
        }
    }
}
