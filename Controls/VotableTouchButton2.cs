using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using TangramApp1._35.Controls.Events;
using TangramApp1._35.Effects;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using libSMARTMultiTouch.Controls;

namespace TangramApp1._35.Controls
{
    public enum VoteRequestFlags : byte
    {
        Other,   //some other event, not from within the button itself, may make the Button request a vote.
        OnClick,
        OnInterval,
    }

    public enum VoteTouchButton2State : byte
    {
        WAITING,
        VOTING
    }

    /// <summary>
    /// 
    /// </summary>
    public class VotableTouchButton2 : TouchButton2
    {
        public static readonly DependencyProperty IsVotableProperty = DependencyProperty.Register("IsVotable", typeof(bool), typeof(TouchButton2), new PropertyMetadata(false));
        public static readonly DependencyProperty VoteRequestIntervalProperty = DependencyProperty.Register("VoteRequestInterval", typeof(double), typeof(VotableTouchButton2), new PropertyMetadata(0.0));
        public static readonly DependencyProperty VoteAutoCancelIntervalProperty = DependencyProperty.Register("VoteAutoCancelInterval", typeof(double), typeof(VotableTouchButton2), new PropertyMetadata(0.0));
        public static readonly DependencyProperty VoteRequestWhenProperty = DependencyProperty.Register("VoteRequestWhen", typeof(VoteRequestFlags), typeof(VotableTouchButton2), new PropertyMetadata(VoteRequestFlags.OnClick));
        public static readonly DependencyProperty CanInterruptVoteProperty = DependencyProperty.Register("CanInterrupt", typeof(bool), typeof(VotableTouchButton2), new PropertyMetadata(false));

        //Action event is the is the event when a vote is successfull
        public static readonly RoutedEvent VoteSuccessfulEvent = EventManager.RegisterRoutedEvent("VoteSuccessful", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(VotableTouchButton2));

        public IVotableHandler voteHandler;
        private VoteTouchButton2State _voteState = VoteTouchButton2State.WAITING;
        protected ColorSaturatorEffect colorSaturator = new ColorSaturatorEffect();

        /// <summary>
        /// 
        /// </summary>
        private Timer timer = new Timer();
        public delegate void VoidDelegate();

        public VotableTouchButton2()
            : base()
        {
            colorSaturator = new ColorSaturatorEffect(Color.FromArgb(0xFF, 0xFF, 0x0, 0x0), 0);
            Effect = colorSaturator;
            Opacity = 0.3;

            Click += new RoutedEventHandler(VotableTouchButton2_Click);
            Loaded += new RoutedEventHandler(VotableTouchButton2_Loaded);
        }

        private void VotableTouchButton2_Loaded(object sender, RoutedEventArgs e)
        {
            timerVoteCancelIntevalElapsed();
        }

        private void VotableTouchButton2_Click(object sender, RoutedEventArgs e)
        {
            if (IsVotable)
            {
                if (_voteState == VoteTouchButton2State.VOTING)
                {
                    if (voteHandler != null && voteHandler.CastVote(this))
                    {
                        voteHandler.CancelVote(this); //cancels the vote on this button

                        DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.0, new Duration(TimeSpan.FromMilliseconds(1500)));
                        colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                        colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);

                        danim = new DoubleAnimation(0.3, new Duration(TimeSpan.FromMilliseconds(1500)));
                        Storyboard board = new Storyboard();
                        Storyboard.SetTarget(danim, this);
                        Storyboard.SetTargetProperty(danim, new PropertyPath(OpacityProperty));
                        board.Children.Add(danim);
                        board.Begin();

                        OnAction();
                    }
                }//even though there are currently only 2 possible states, i am performing this check anyway.
                else if(_voteState == VoteTouchButton2State.WAITING && VoteRequestWhen == VoteRequestFlags.OnClick) 
                {
                    //request vote will update the state of the button
                    if (voteHandler != null && voteHandler.RequestVote(this))
                    {
                        _voteState = VoteTouchButton2State.VOTING;

                        if (voteHandler.CastVote(this)) //should never execute unless there is 1 person
                        {
                            voteHandler.CancelVote(this); //will reset
                            OnAction();
                        }
                        else
                        {
                            ResetTimer();
                            timer.Interval = VoteAutoCancelInterval;
                            timer.Elapsed += new ElapsedEventHandler(delegate(object src, ElapsedEventArgs evt)
                            {
                                Dispatcher.BeginInvoke(new VoidDelegate(timerVoteCancelIntevalElapsed), DispatcherPriority.Normal);
                            });
                            timer.Start();

                            //use transitionizor.
                            DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.5, new Duration(TimeSpan.FromMilliseconds(1500)));
                            colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                            colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);

                            danim = new DoubleAnimation(0.9, new Duration(TimeSpan.FromMilliseconds(1500)));
                            Storyboard board = new Storyboard();
                            Storyboard.SetTarget(danim, this);
                            Storyboard.SetTargetProperty(danim, new PropertyPath(OpacityProperty));
                            board.Children.Add(danim);
                            board.Begin();
                        }
                       
                    }
                }
            }
        }

        public VoteTouchButton2State VoteState
        {
            get
            {
                return _voteState;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color VoteFilterColor
        {
            get
            {
                return colorSaturator.FilterColor;
            }
            set
            {
                if (colorSaturator.ColorSaturation > 0)
                {
                    ColorAnimation canim = new ColorAnimation(VoteFilterColor, value, new Duration(TimeSpan.FromMilliseconds(1000)));
                    colorSaturator.BeginAnimation(ColorSaturatorEffect.FilterColorProperty, null); //memory leak fix
                    colorSaturator.BeginAnimation(ColorSaturatorEffect.FilterColorProperty, canim);
                }
                else
                {
                    colorSaturator.FilterColor = value;
                }
            }
        }


        #region EVENTS
        public event RoutedEventHandler VoteSuccessful
        {
            add
            {
                base.AddHandler(VoteSuccessfulEvent, value);
            }
            remove
            {
                base.RemoveHandler(VoteSuccessfulEvent, value);
            }
        }
        #endregion

        #region DEPENDENCYPROPERTIES
        public bool IsVotable
        {
            get
            {
                return (bool)GetValue(IsVotableProperty);
            }
            set
            {
                SetValue(IsVotableProperty, value);
            }
        }

        public double VoteRequestInterval
        {
            get
            {
                return (double)GetValue(VoteRequestIntervalProperty);
            }
            set
            {
                SetValue(VoteRequestIntervalProperty, value);
            }
        }

        public double VoteAutoCancelInterval
        {
            get
            {
                return (double)GetValue(VoteAutoCancelIntervalProperty);
            }
            set
            {
                SetValue(VoteAutoCancelIntervalProperty, value);
            }
        }

        public VoteRequestFlags VoteRequestWhen
        {
            set
            {
                SetValue(VoteRequestWhenProperty, value);
            }
            get
            {
                return (VoteRequestFlags)GetValue(VoteRequestWhenProperty);
            }
        }

        public bool CanInterruptVote
        {
            set
            {
                SetValue(CanInterruptVoteProperty, value);
            }
            get
            {
                return (bool)GetValue(CanInterruptVoteProperty);
            }
        }
        #endregion

        /// <summary>
        /// Callback METHOD
        /// </summary>
        private void timerRequestVoteIntervalElapsed()
        {
            //make sure that the value hasnt changed.
            if (VoteRequestWhen == VoteRequestFlags.OnInterval)
            {
                if (voteHandler != null && voteHandler.RequestVote(this))
                {
                    _voteState = VoteTouchButton2State.VOTING; //updates itself.

                    ResetTimer();
                    timer.Interval = VoteAutoCancelInterval;
                    timer.Elapsed += new ElapsedEventHandler(delegate(object src, ElapsedEventArgs evt)
                    {
                        Dispatcher.BeginInvoke(new VoidDelegate(timerVoteCancelIntevalElapsed), DispatcherPriority.Normal);
                    });
                    timer.Start();

                    DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.5, new Duration(TimeSpan.FromMilliseconds(1500)));
                    colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                    colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);

                    danim = new DoubleAnimation(0.9, new Duration(TimeSpan.FromMilliseconds(1500)));
                    Storyboard board = new Storyboard();
                    Storyboard.SetTarget(danim, this);
                    Storyboard.SetTargetProperty(danim, new PropertyPath(OpacityProperty));
                    board.Children.Add(danim);
                    board.Begin();
                }
                else //assumed because it could not request the vote
                {
                    ResetTimer();
                    timer.Interval = VoteRequestInterval;
                    timer.Elapsed += new ElapsedEventHandler(
                        delegate(object obj, ElapsedEventArgs args)
                        {
                            Dispatcher.BeginInvoke(new VoidDelegate(timerRequestVoteIntervalElapsed), DispatcherPriority.Normal);
                        });
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// CALLBACK METHOD
        /// </summary>
        public void timerVoteCancelIntevalElapsed()
        {
            if (voteHandler != null)
            {
                //let the handler know we want to stop the vote for this button
                voteHandler.CancelVote(this);

                _voteState = VoteTouchButton2State.WAITING;

                ResetTimer();
                if (VoteRequestWhen == VoteRequestFlags.OnInterval)
                {
                    timer.Interval = VoteRequestInterval;
                    timer.Elapsed += new ElapsedEventHandler(
                        delegate(object obj, ElapsedEventArgs args)
                        {
                            Dispatcher.BeginInvoke(new VoidDelegate(timerRequestVoteIntervalElapsed), DispatcherPriority.Normal);
                        });
                    timer.Start();

                }

                //
                DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.0, new Duration(TimeSpan.FromMilliseconds(1500)));
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);
            }
        }

        /// <summary>
        /// Called
        /// </summary>
        public void ForceWaiting(double time)
        {
            _voteState = VoteTouchButton2State.WAITING;
            if (VoteRequestWhen == VoteRequestFlags.OnInterval)
            {
                ResetTimer();
                timer.Interval = time;
                timer.Elapsed += new ElapsedEventHandler(
                    delegate(object obj, ElapsedEventArgs args)
                    {
                        Dispatcher.BeginInvoke(new VoidDelegate(timerRequestVoteIntervalElapsed), DispatcherPriority.Normal);
                    });
                timer.Start();
            }
        }

        public void ForceRequest()
        {
            if (voteHandler != null && voteHandler.RequestVote(this))
            {
                _voteState = VoteTouchButton2State.VOTING;

                ResetTimer();
                timer.Interval = VoteAutoCancelInterval;
                timer.Elapsed += new ElapsedEventHandler(delegate(object src, ElapsedEventArgs evt)
                {
                    Dispatcher.BeginInvoke(new VoidDelegate(timerVoteCancelIntevalElapsed), DispatcherPriority.Normal);
                });
                timer.Start();

                DoubleAnimation danim = new DoubleAnimation(colorSaturator.ColorSaturation, 0.5, new Duration(TimeSpan.FromMilliseconds(1500)));
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, null);//
                colorSaturator.BeginAnimation(ColorSaturatorEffect.ColorSaturationProperty, danim);

                danim = new DoubleAnimation(0.9, new Duration(TimeSpan.FromMilliseconds(1500)));
                Storyboard board = new Storyboard();
                Storyboard.SetTarget(danim, this);
                Storyboard.SetTargetProperty(danim, new PropertyPath(OpacityProperty));
                board.Children.Add(danim);
                board.Begin();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ResetTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Close(); //BYE BYE
            }
            timer = new Timer();
        }



        #region ACTION
        /// <summary>
        /// 
        /// </summary>
        public void OnAction()
        {
            base.RaiseEvent(new RoutedEventArgs(VoteSuccessfulEvent, this));
        }
        #endregion
    }
}
