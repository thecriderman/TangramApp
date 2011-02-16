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
using TangramApp1._35.Controls.Pages;
using libSMARTMultiTouch.Table;
using TangramApp1._35.Controls;
using libSMARTMultiTouch.Controls;

namespace TangramApp1._35
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void WindowLoaded(object obj, RoutedEventArgs args)
        {
            TableManager.Initialize(this, LayoutRoot);
            
            LayoutRoot.Children.Add(new TableControl());


            TableManager.IsFullScreen = true;

            //fixes issue where you need to minimize and normalize to interact
            this.WindowState = System.Windows.WindowState.Minimized;
            System.Threading.Thread.Sleep(100);
            this.WindowState = System.Windows.WindowState.Normal;
        }

        
    }
}
