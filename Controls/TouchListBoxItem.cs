using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using libSMARTMultiTouch.Input;
using System.Reflection;
using System.Windows.Input;

namespace TangramApp1._35.Controls
{
    public class TouchListBoxItem : ListBoxItem
    {
        public TouchListBoxItem()
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
            //We use Reflection to access the private method "HandleMouseButtonDown" and invoke it
            MethodInfo startTimerMethod = this.GetType().GetMethod("HandleMouseButtonDown", BindingFlags.NonPublic | BindingFlags.Instance);
            startTimerMethod.Invoke(this, new object[]{ MouseButton.Left });
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
                args.TouchContact.Release();
                //no reflection needed
            }
        }
    }
}
