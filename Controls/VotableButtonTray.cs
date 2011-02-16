using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using libSMARTMultiTouch.Controls;
using TangramApp1._35.Controls.Events;

namespace TangramApp1._35.Controls
{
    public class VotableButtonTray : StackPanel, IVotableHandler
    {
        //STATIC METHODS USED FOR INSTANCE PURPOSES
        private static VotableButtonTray _trayInstance;
        public static VotableButtonTray getInstance()
        {
            if (_trayInstance == null)
            {
                _trayInstance = new VotableButtonTray();
            }
            return _trayInstance;
        }
        
        //PROPERTIES
        public static readonly DependencyProperty VoteYesColorProperty = DependencyProperty.Register("VoteYesFilterColor", typeof(Color), typeof(VotableButtonTray), new FrameworkPropertyMetadata(Colors.Green, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty VoteCancelColorProperty = DependencyProperty.Register("VoteNoFilterColor", typeof(Color), typeof(VotableButtonTray), new FrameworkPropertyMetadata(Colors.Red, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly RoutedEvent VoteTargetChangedEvent = EventManager.RegisterRoutedEvent("VoteTargetChanged", RoutingStrategy.Bubble, typeof(VotableButtonTrayChangedEventHandler), typeof(VotableButtonTray));

        public HashSet<VotableTouchButton2> Buttons = new HashSet<VotableTouchButton2>();
        public VotableTouchButton2 voteTarget = null;

        private int _TouchesRemaining = App.TABLE_USERS; //number of users on the table
        private int _CornerDockIncrement = App.TABLE_USERS == 2 ? 2 : 1;

        public VotableButtonTray()
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal;
            CornerDockPanel.SetCornerDock(this, CornerDock.TopRightCorner, false);
        }

        public void RegisterVotable(VotableTouchButton2 v)
        {
            v.voteHandler = this;
            Buttons.Add(v);
            Children.Add(v);
        }

        public void UnregisterVotable(VotableTouchButton2 v)
        {
            v.voteHandler = null;
            Buttons.Remove(v);
            Children.Remove(v);

            if (voteTarget == v)
            {
                VotableTouchButton2 temp = voteTarget;
                voteTarget = null;
                RaiseVoteTargetChanged(temp, voteTarget);
            }
        }

        /// <summary>
        /// Tells the Tray that a button is canceling their vote.
        /// </summary>
        /// <returns>True if the Vote is canceled.</returns>
        public bool CancelVote(VotableTouchButton2 v)
        {
            if (voteTarget != null && v == voteTarget) //currently voting...
            {
                VotableTouchButton2 temp = voteTarget;
                voteTarget = null;

                RaiseVoteTargetChanged(temp, voteTarget);

                //return the Tray to the starting location
                CornerDockPanel.SetCornerDock(this, CornerDock.TopRightCorner, true);
                _TouchesRemaining = App.TABLE_USERS;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Tells the tray that a button is casting a vote.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool CastVote(VotableTouchButton2 src)
        {
            if (src == voteTarget) //check validity of vote being cast.
            {
                int currentCorner = (int)CornerDockPanel.GetCornerDock(this);
                //looks better this way i think
                CornerDockPanel.SetCornerDock(this,
                    (CornerDock)((((currentCorner - _CornerDockIncrement) % 4) + 4) % 4), true);

                return --_TouchesRemaining <= 0;
            }
            return false; //stupid >>
        }

        /// <summary>
        /// Tells the tray that a button wishes to become the new vote target.
        /// </summary>
        /// <param name="src"></param>
        /// <returns>whether or not the button becomes the new vote target</returns>
        public bool RequestVote(VotableTouchButton2 src)
        {
            if(!src.IsVotable){ return false;  } ////dont waste my time

            if (voteTarget == null || src.CanInterruptVote)
            {
                VotableTouchButton2 temp = voteTarget;
                voteTarget = src;

                RaiseVoteTargetChanged(temp, src);

                //@TODO: Add code that resets all buttons
                int i = 0;
                foreach (VotableTouchButton2 button in Buttons)
                {
                    //force everything to wait.
                    if (button != src)
                    {
                        button.ForceWaiting(button.VoteRequestInterval + 10 * (++i));
                    }                  
                }

                return true;
            }
            return false;
        }

        #region PROPERTIES
        public Color VoteYesColor
        {
            set
            {
                SetValue(VoteYesColorProperty, value);
            }
            get
            {
                return (Color)GetValue(VoteYesColorProperty);
            }
        }

        public Color VoteCancelColor
        {
            set
            {
                SetValue(VoteCancelColorProperty, value);
            }
            get
            {
                return (Color)GetValue(VoteCancelColorProperty);
            }
        }
        #endregion

        #region EVENTS
        public event VotableButtonTrayChangedEventHandler VoteTargetChanged
        {
            add { AddHandler(VoteTargetChangedEvent, value); }
            remove { RemoveHandler(VoteTargetChangedEvent, value); }
        }
        #endregion

        #region EVENTRAISERS
        public void RaiseVoteTargetChanged(VotableTouchButton2 from, VotableTouchButton2 to)
        {
            RaiseEvent(new VotableButtonTrayChangedRoutedEventArgs(VoteTargetChangedEvent, this, from, to));
        }
        #endregion
    }
}
