using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using TangramApp1._35.Effects;

namespace TangramApp1._35.Controls
{
    public class VotableExitCancelButton : VotableTouchButton2
    {
        public static readonly Color EXITMODECOLOR = Colors.Green;
        public static readonly Color CANCELMODECOLOR = Colors.Red;

        private VotableTouchButton2 currTarget = null;

        public VotableExitCancelButton()
            : base()
        {
            //Start in the default mode
            IsVotable = true; //we will toggle this to suit our needs.
            CanInterruptVote = false;
            VoteRequestWhen = VoteRequestFlags.OnClick;
            VoteAutoCancelInterval = 60000; //1 minute

            Click += new System.Windows.RoutedEventHandler(VotableExitCancelButton_Click);
            Loaded += new System.Windows.RoutedEventHandler(VotableExitCancelButton_Loaded);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VotableExitCancelButton_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //since it has been loaded, its safe to assume that the votehandler  has been set.
            voteHandler.VoteTargetChanged += new Events.VotableButtonTrayChangedEventHandler(voteHandler_VoteTargetChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VotableExitCancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsVotable) //means that the button is in CANCELVOTEMODE
            {
                voteHandler.CancelVote(currTarget);
            }
        }

        void  voteHandler_VoteTargetChanged(object src, Events.VotableButtonTrayChangedRoutedEventArgs args)
        {
            IsVotable = (args.to == null || args.to == this);
            bool willVote = args.to == null;

            if (willVote)
            {
                VoteFilterColor = EXITMODECOLOR;

                DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.5, new Duration(TimeSpan.FromMilliseconds(1500)));
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);
            }
            else
            {
                VoteFilterColor = CANCELMODECOLOR;

                DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.5, new Duration(TimeSpan.FromMilliseconds(1500)));
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);
            }

            currTarget = args.to;
        }
    }
}
