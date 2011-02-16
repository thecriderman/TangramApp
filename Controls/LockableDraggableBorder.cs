//#define TABLEMODE


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libSMARTMultiTouch.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows;
using libSMARTMultiTouch.Input;
using TangramApp1._35.Controls.Behaviors;

namespace TangramApp1._35.Controls
{
    
    public class LockableDraggableBorder : DraggableBorder
    {
        private Effect lockEffect;
        private Effect unlockEffect;

        //
        private Effect touchedUnlockEffect;
        private Effect touchedLockedEffect;

        //
        public event EventHandler OnLock;
        public event EventHandler OnUnlock;

        public LockableDraggableBorder() : base()
        {
            #if TABLEMODE
                new PressureLockBehavior(this); //DOUBLE RAWR
            #else
                new HoldAndLockBehavior(this); //RAWR!
            #endif

            TouchInputManager.AddTouchContactDownHandler(this, new TouchContactEventHandler(m_onLockableTouched));
            TouchInputManager.AddTouchContactUpHandler(this, new TouchContactEventHandler(m_onLockableUntouched));
        }

        private void m_onLockableTouched(object src, TouchContactEventArgs args)
        {
            if (IsLocked())
            {
                //Console.Out.WriteLine("touch lock effect");
                Effect = touchedLockedEffect;
            }
            else
            {
                //Console.Out.WriteLine("touch unlock effect");
                Effect = touchedUnlockEffect;
            }
        }

        private void m_onLockableUntouched(object src, TouchContactEventArgs args)
        {
            if (IsLocked())
            {
                //Console.Out.WriteLine("lock effect");
                Effect = lockEffect;
            }
            else
            {
                //Console.Out.WriteLine("unlock effect");
                Effect = unlockEffect;
            }
        }


        #region Effects Members
        public Effect UnlockEffect
        {
            set
            {
                unlockEffect = value;
                if (!IsLocked() && !IsTouched())
                {
                    Effect = unlockEffect;
                }
            }
            get
            {
                return unlockEffect;
            }
        }

        public Effect LockEffect
        {
            set
            {
                lockEffect = value;
                if (IsLocked() && !IsTouched())
                {
                    Effect = lockEffect;
                }
            }
            get
            {
                return lockEffect;
            }
        }

        public Effect TouchedUnlockedEffect
        {
            set
            {
                touchedUnlockEffect = value;
                if (!IsLocked() && IsTouched())
                {
                    Effect = touchedUnlockEffect;
                }
            }
            get
            {
                return touchedUnlockEffect;
            }
        }

        public Effect TouchLockedEffect
        {
            get
            {
                return touchedLockedEffect;
            }
            set
            {
                touchedLockedEffect = value;
                if (IsLocked() && IsTouched())
                {
                    Effect = touchedLockedEffect;
                }
            }
        }
        #endregion

        #region Lockable Members

        Clearances lockAuthorization = Clearances.NONE; //FREE

        public bool IsTouched()
        {
            return TouchInputManager.GetTouchContactsCaptured(this).Count != 0;
        }

        public bool IsLocked()
        {
            return lockAuthorization != Clearances.NONE;
        }

        public Clearances GetAuthorization()
        {
            return lockAuthorization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool Lock(Clearances authorization)
        {
            if (authorization > lockAuthorization)
            {
                lockAuthorization = authorization;

                //LOCK THE POLYGON IN PLACE HERE
                IsTranslateEnabled = false;
                IsRotateEnabled = false;

                Effect = TouchLockedEffect;

                if(OnLock != null)
                    OnLock(this, EventArgs.Empty);

                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool Unlock(Clearances authorization)
        {
            if (authorization >= lockAuthorization)
            {
                lockAuthorization = Clearances.NONE; //open

                //UNLOCK THE POLYGON IN PLACE HERE
                IsTranslateEnabled = true;
                IsRotateEnabled = true;

                Effect = touchedUnlockEffect;

                if (OnUnlock != null)
                    OnUnlock(this, EventArgs.Empty);

                return true;
            }
            return false;
        }
        #endregion
    }
    
}
