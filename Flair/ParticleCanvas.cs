using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Threading;
using System;
using libSMARTMultiTouch.Input;
using System.Windows;
using System.ComponentModel;

namespace TangramApp1._35.Flair
{
    /// <summary>
    /// 
    /// </summary>
    public class ParticleCanvas : Canvas
    {
        private System.Timers.Timer updateTimer;
        public delegate void VoidDelegate();

        EmitterCollection _emitters;

        public ParticleCanvas() : base()
        {
            SetupTimer();
            _emitters = new EmitterCollection(this);
            Loaded += new RoutedEventHandler(ParticleCanvas_Loaded);
            Unloaded += new RoutedEventHandler(ParticleCanvas_Unloaded);
        }

        void ParticleCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            SetupTimer(); //stop
        }
            

        void ParticleCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            start();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public EmitterCollection Emitters{
            get
            {
                return _emitters;
            }
        }

        public void start()
        {
            updateTimer.Start();
        }

        public void SetupTimer()
        {
            if (updateTimer != null)
            {
                updateTimer.Stop();
            }
            updateTimer = new System.Timers.Timer();
            updateTimer.Interval = 40;
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += new ElapsedEventHandler(updateTimer_Elapsed);
        }

        public void updateTimer_Elapsed(object sender, ElapsedEventArgs args)
        {
            Dispatcher.BeginInvoke(new VoidDelegate(update), DispatcherPriority.Normal);
        }

        /// <summary>
        /// 
        /// </summary>
        private void update()
        {
            foreach (Emitter emit in _emitters)
            {
                
               emit.update(); //emitters take care of updating their particles
            }
        }
    }
}
