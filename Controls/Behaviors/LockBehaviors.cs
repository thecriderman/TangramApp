using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libSMARTMultiTouch;
using System.Windows.Interactivity;
using libSMARTMultiTouch.Input;
using System.Windows.Threading;
using System.Timers;
using System.Collections.ObjectModel;
using System.Windows;

namespace TangramApp1._35.Controls.Behaviors
{
    
    /// <summary>
    /// Specialized Locking Behavior for ModeratedDraggableBorders
    /// Pressure from a press of the finger causes locking to work.
    /// </summary>
    public class PressureLockBehavior
    {
        private const double LOCK_PRESSURE = 200; //THIS IS ALMOST AS HARD AS I CAN PUSH
        private const double MAXRADIUS = 20 * 20; //ACTUALLY RADIUS SQUARED

        private double touchX = double.MaxValue;
        private double touchY = double.MinValue;

        private LockableDraggableBorder AssociatedObject;

        //
        public delegate void VoidDelegate();
        private Timer updateTimer;
        private const long UPDATEINTERVAL = 20; //20 miliseconds
        private Dispatcher threadDispatch;

        public PressureLockBehavior(LockableDraggableBorder target)
        {
            AssociatedObject = target;
            threadDispatch = Dispatcher.CurrentDispatcher; //use what created it
            OnAttached();
        }

        public void OnAttached()
        {
            TouchInputManager.AddTouchContactDownHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchDown));
            TouchInputManager.AddTouchContactMoveHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchMove));
            TouchInputManager.AddTouchContactUpHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchUp));
        }

        protected void ResetTimer()
        {
            if (updateTimer != null)
            {
                updateTimer.Dispose();
            }
            updateTimer = new Timer(UPDATEINTERVAL);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += new ElapsedEventHandler(UpdateTimer_Elapsed);
        }

        void UpdateTimer_Elapsed(object src, ElapsedEventArgs args){
            threadDispatch.BeginInvoke(DispatcherPriority.Normal, new VoidDelegate(TimerCallback));
        }

        void TimerCallback(){
            //check the pressure of the touch contact
            ReadOnlyCollection<TouchContact> roc = TouchInputManager.GetTouchContactsCaptured(AssociatedObject);
            if(roc.Count == 0){
                ResetTimer();
                return;
            }

            //check the pressure.
            TouchContact tc = roc[0];
            if(tc.Intensity >= LOCK_PRESSURE){
                if(AssociatedObject.IsLocked()){
                    AssociatedObject.Unlock(Clearances.STUDENT);
                }else{
                    AssociatedObject.Lock(Clearances.STUDENT);
                }
                ResetTimer(); //not needed until next time
            }

        }

        void AssociatedObject_TouchDown(object sender, TouchContactEventArgs e)
        {
            ReadOnlyCollection<TouchContact> roc = TouchInputManager.GetTouchContactsCaptured(AssociatedObject);
            if (roc.Count > 1)
            {
                return; //only works if there is 1 contact only
            }

            touchX = e.TouchContact.Position.X;
            touchY = e.TouchContact.Position.Y;

            ResetTimer();
            updateTimer.Start();
        }

        

        void AssociatedObject_TouchMove(object sender, TouchContactEventArgs e)
        {
            ReadOnlyCollection<TouchContact> roc = TouchInputManager.GetTouchContactsCaptured(AssociatedObject);
            if (roc.Count > 1)
            {
                return; //only works if there is 1 contact only
            }

            //check to see if the threshold has been reached, and that it has not gone over the distance threshold

            double dx = e.TouchContact.Position.X - touchX;
            double dy = e.TouchContact.Position.Y - touchY;
            double r2pow = dx * dx + dy * dy;

            if (r2pow > MAXRADIUS)
            {
                ResetTimer();
            }

        }

        void AssociatedObject_TouchUp(object sender, TouchContactEventArgs e)
        {
            ResetTimer();
            touchX = double.MaxValue;
            touchY = double.MinValue;
        }

    }

    
    /// <summary>
    /// Lock Behavor that does uses a hold and wait method in order to 
    /// lock a piece into place.
    /// </summary>
    public class HoldAndLockBehavior
    {
        public delegate void VoidDelegate();

        private Dispatcher threadDispatch; //that shall save the day!

        private const long LOCKTIME = 2500;
        private const double MAXRADIUS = 20 * 20; //ACTUALLY RADIUS SQUARED

        private LockableDraggableBorder AssociatedObject;
        private Timer LockTimer;

        public HoldAndLockBehavior(LockableDraggableBorder mdb)
        {
            AssociatedObject = mdb;
            
            OnAttached();
        }

        public void OnAttached()
        {
            TouchInputManager.AddTouchContactDownHandler( AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchDown) );
            TouchInputManager.AddTouchContactMoveHandler( AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchMove) );
            TouchInputManager.AddTouchContactUpHandler( AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchUp) );
            //AssociatedObject.TouchDown += new TouchContactEventHandler(AssociatedObject_TouchDown);
            //AssociatedObject.TouchMove += new TouchContactEventHandler(AssociatedObject_TouchMove);
            //AssociatedObject.TouchUp += new TouchContactEventHandler(AssociatedObject_TouchUp);
            
            //The thread that created this object, is the one we use.
            threadDispatch = Dispatcher.CurrentDispatcher;
        }

        private int touchID = int.MinValue;
        private double touchX = double.MinValue;
        private double touchY = double.MinValue;

        void AssociatedObject_TouchDown(object sender, TouchContactEventArgs e)
        {
            //we can only lock if there is 1 Finger on the object.
            if (touchID != int.MinValue)
            {
                LockTimer.Dispose(); //stop the timer!
                return;
            }

            HardReset();

            touchX = e.TouchContact.Position.X;
            touchY = e.TouchContact.Position.Y;
            touchID = e.TouchContact.ID;

            //Console.Out.WriteLine("Touch Down: "+touchID+" ("+touchX + ", "+touchY+")");

            LockTimer.Start();
        }

        void AssociatedObject_TouchMove(object sender, TouchContactEventArgs e)
        {
            if (e.TouchContact.ID != touchID)
            {
                return; //not going to waste time with silly calculations
            }

            double dx = e.TouchContact.Position.X - touchX;
            double dy = e.TouchContact.Position.Y - touchY;
            double r2pow = dx * dx + dy * dy;

            if (r2pow > MAXRADIUS)
            {
                touchID = int.MinValue;
                //Console.Out.WriteLine("Reset: " + r2pow); //debug
                HardReset();
            }
            
        }

        void AssociatedObject_TouchUp(object sender, TouchContactEventArgs e)
        {
            HardReset();

            double tX = e.TouchContact.Position.X;
            double tY = e.TouchContact.Position.Y;
            int tID = e.TouchContact.ID;

            //Console.Out.WriteLine("Touch Up: "+tID+" ("+tX + ", "+tY+")");
            //if (touchID == e.TouchContact.ID)
            //{
                touchID = int.MinValue;
            //}
        }


        void LockTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                HardReset();
                threadDispatch.BeginInvoke(DispatcherPriority.Normal, new VoidDelegate(TimerUpCallBack)); 
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        void TimerUpCallBack()
        {
            //Console.WriteLine(AssociatedObject.IsLocked());
            if (AssociatedObject.IsLocked())
            {
                AssociatedObject.Unlock(Clearances.STUDENT);
                //Console.Out.WriteLine("Touch: UNLOCKING");
            }
            else
            {
                AssociatedObject.Lock(Clearances.STUDENT);
                //Console.Out.WriteLine("Touch: LOCKING");
            }
        }

        /**
         * 
         */
        void HardReset()
        {
            if (LockTimer != null)
            {
                LockTimer.Stop();
                LockTimer.Dispose();
            }
            LockTimer = new Timer(LOCKTIME);
            LockTimer.AutoReset = false;
            LockTimer.Elapsed += new ElapsedEventHandler(LockTimer_Elapsed);
        }
    }
}
