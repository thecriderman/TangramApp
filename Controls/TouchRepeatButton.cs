using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libSMARTMultiTouch.Input;
using System.Windows.Controls.Primitives;
using System.Reflection;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// Version of the RepeatButton that allows for touch input.
    /// Note: Currently unknown if RepeatButton supports different types of input.
    /// </summary>
    public class TouchRepeatButton : RepeatButton
    {
        public TouchRepeatButton()
        {
            TouchInputManager.AddTouchContactDownHandler(this, new TouchContactEventHandler(OnTouchContactDownHandler));
            TouchInputManager.AddTouchContactUpHandler(this, new TouchContactEventHandler(OnTouchContactUpHandler));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        protected void OnTouchContactDownHandler(object src, TouchContactEventArgs args)
        {
            args.TouchContact.Capture(this);
            base.IsPressed = true;
            //We use Reflection to access the private method "StartTimer" and invoke it
            MethodInfo startTimerMethod = this.GetType().GetMethod("StartTimer", BindingFlags.NonPublic | BindingFlags.Instance);
            startTimerMethod.Invoke(this, System.Type.EmptyTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        protected void OnTouchContactUpHandler(object src, TouchContactEventArgs args)
        {
            if (args.TouchContact.CapturedElement == this)
            {
                base.IsPressed = false;
                args.TouchContact.Release();
                //We use Reflection to access the private method "StopTimer" and invoke it
                MethodInfo stopTimerMethod = this.GetType().GetMethod("StopTimer", BindingFlags.NonPublic | BindingFlags.Instance);
                stopTimerMethod.Invoke(this, System.Type.EmptyTypes);
            }
        }
    }
}
