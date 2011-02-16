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
using TangramApp1._35.Controls.Events;
using System.Windows.Media.Animation;

namespace TangramApp1._35.Controls.Pages
{

    

    /// <summary>
    /// Interaction logic for Game_TangramPuzzle_SelectPuzzle.xaml
    /// </summary>
    public partial class Game_TangramPuzzle_SelectPuzzle : TouchMenuPage
    {
        private VotableTouchButton2 okayButton;

        private List<string> puzzles = new List<string>();
        private int changingPuzzleIndex = 0;
        
        private List<TouchToggleButton> buttons = new List<TouchToggleButton>();
        private int changingButtonIndex = 0;


        //Selection Properties
        private TouchToggleButton currentToggle = null;
        private string currentPath = null;

        public Game_TangramPuzzle_SelectPuzzle()
        {
            InitializeComponent();
            LoadPuzzleStringList();
            CreateVotableOKButton();
        }

        private void CreateVotableOKButton()
        {
            okayButton = new VotableTouchButton2();
            okayButton.VoteSuccessful += new RoutedEventHandler(okayButton_VoteSuccessful);
            okayButton.VoteFilterColor = Colors.Green;
            okayButton.IsVotable = true;
            okayButton.VoteRequestWhen = VoteRequestFlags.Other;
            okayButton.VoteAutoCancelInterval = 60000;

            okayButton.Width = okayButton.Height = 50;

            okayButton.Style = (Style)App.Current.FindResource("UnstyledButton");
            Image icon = new Image();
            icon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/UI/OkayButton.png"));
            okayButton.Content = icon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void okayButton_VoteSuccessful(object sender, RoutedEventArgs e)
        {
            try
            {
                GotoNextPage(typeof(Game_TangramPuzzle_Play), getPuzzleURI(currentToggle));
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Console.WriteLine(exc.InnerException);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public override void InstallVotableButtons()
        {
            VotableButtonTray.getInstance().RegisterVotable(okayButton);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UninstallVotableButtons()
        {
            VotableButtonTray.getInstance().UnregisterVotable(okayButton);
        }

        public void LoadPuzzleStringList()
        {
            puzzles.Add("E");
            puzzles.Add("Fisherman");
            puzzles.Add("Flower");
            puzzles.Add("House");
            puzzles.Add("Teapot");
            //NOT SUPPORTED YET
            /*puzzles.Add("A");
            puzzles.Add("B");
            puzzles.Add("C");
            puzzles.Add("D");
            puzzles.Add("E");
            puzzles.Add("F");
            puzzles.Add("G");
            puzzles.Add("H");
            puzzles.Add("I");
            puzzles.Add("J");
            puzzles.Add("K");
            puzzles.Add("L");
            puzzles.Add("M");
            puzzles.Add("N");
            puzzles.Add("O");
            puzzles.Add("P");
            puzzles.Add("Q");
            puzzles.Add("R");
            puzzles.Add("S");*/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asrc"></param>
        /// <param name="args"></param>
        public void UserControl_Loaded(object asrc, RoutedEventArgs args)
        {
            foreach (TouchToggleButton button in PuzzleSelector.Children.OfType<TouchToggleButton>())
            {
                buttons.Add(button);
                button.Checked += new RoutedEventHandler(button_Checked);
                button.Unchecked += new RoutedEventHandler(button_Unchecked);

                TextBlock blocky = new TextBlock();
                blocky.Text = puzzles[++changingPuzzleIndex % puzzles.Count];
                blocky.Padding = new Thickness(0, 0, 25, 0);
                blocky.FontSize = 30;
                blocky.Foreground = Brushes.White;

                button.Content =  blocky; //little bit of string buffering
            }

            changingButtonIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void button_Unchecked(object sender, RoutedEventArgs e)
        {
            if (currentToggle == sender)
            {
                //TouchToggleButton b = currentToggle;
                currentToggle = null;
                //b.IsChecked = true; //CANT BE UNCHECKED

                PuzzleSelector.IsTouchRotationEnabled = true;

                PuzzleDisplay.BorderBrush = (Brush)Resources["ShowMeBrush"];
                Hedge.Fill = (Brush)Resources["ShowMeLinearBrush"];
                PuzzleDisplay.Child = null;

                okayButton.ForceWaiting(double.NaN);
            }
        }

        void button_Checked(object sender, RoutedEventArgs e)
        {
            if(currentToggle != null)
            {
                currentToggle.IsChecked = false;
            }
            currentToggle = (TouchToggleButton)sender;

            PuzzleSelector.IsTouchRotationEnabled = false;

            //DISPLAY THE OBJECT IN QUESTION
            //PuzzleDisplay.Child
            try
            {
                XMLTangramPuzzlePreviewParser p = new XMLTangramPuzzlePreviewParser();
                PuzzleDisplay.Child = p.Load(getPuzzleURI(currentToggle));

                PuzzleDisplay.BorderBrush = (Brush)Resources["OKBrush"];
                Hedge.Fill = (Brush)Resources["OKLinearBrush"];

                okayButton.ForceRequest();
            }
            catch (Exception)
            {
                TextBlock blocky = new TextBlock();
                blocky.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                blocky.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                blocky.FontSize = 20;
                blocky.Width = blocky.Height = 150;
                blocky.TextAlignment = TextAlignment.Center;
                blocky.Text = "Preview Unavailable";
                PuzzleDisplay.Child = blocky;
            }
        }

        public string getPuzzleURI(TouchToggleButton button)
        {
            TextBlock block = (TextBlock)button.Content;
            return "pack://application:,,,/Resources/XML/Tangram/puzzles/" + block.Text + ".xml";
        }

        private double lastChangedStartAngle = 90;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PuzzleSelector_StartAngleChanged(object sender, RoutedEventArgs e)
        {
            /*//Temporarily removed until overflow is fixed.
             * 
            //detect if a button has passed a certain point, then update the label to the next puzzle if necessary
            DependencyPropertyChangedEventArgs args = ((DependencyPropertyChangedRoutedEventArgs)e).info; //the routed event args is actually a carrier
            //double oldValue = (double)args.OldValue - StartAngleOffset;
            double newValue = (double)args.NewValue;

            //double deltaValue = newValue - oldValue;

            //doesnt take into account the buffer between buttons
            double singleButtonSpan = 360.0 / (double)buttons.Count;

            if (newValue - lastChangedStartAngle > 0) //clockwise 
            {
                TextBlock blocky = new TextBlock();
                blocky.Text = puzzles[changingPuzzleIndex];
                blocky.Foreground = Brushes.Red;

                buttons[changingButtonIndex].Content = blocky;

                changingButtonIndex = (changingButtonIndex - 1) < 0 ? buttons.Count  - 1 : changingButtonIndex - 1;
                changingPuzzleIndex = (changingPuzzleIndex - 1) < 0 ? puzzles.Count - 1 : changingPuzzleIndex - 1;

                lastChangedStartAngle += singleButtonSpan;
            }
            else if (newValue - lastChangedStartAngle < -singleButtonSpan) //counterclockwise
            {
                changingButtonIndex = (changingButtonIndex + 1) % buttons.Count;

                TextBlock blocky = new TextBlock();
                blocky.Text = puzzles[changingPuzzleIndex];
                blocky.Foreground = Brushes.Green;

                buttons[changingButtonIndex].Content = blocky;

                
                changingPuzzleIndex = (changingPuzzleIndex + 1) % puzzles.Count;

                lastChangedStartAngle -= singleButtonSpan;
            }
             */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="param"></param>
        public override void GotoNextPage(Type pageType, object param)
        {
            Game_TangramPuzzle_Play playMenu = new Game_TangramPuzzle_Play();
            playMenu.InitializePuzzle(param as string);

            Panel p = (Parent as Panel);
            p.Children.Remove(this);
            p.Children.Add(playMenu);
        }

    }
}
