using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using libSMARTMultiTouch.Input;
using System.Windows;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// An improvement over the original touch button. Allows for different click modes.
    /// Named TouchButton2 because TouchButton exists under LibSmartTable.Controls.
    /// Changelog: 
    /// 1/4/2011 Added rerouting of events for custom states.
    /// </summary>
    public class TouchButton2 : TouchButtonBase
    {
        //might add this sometime
        //public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(TouchButton2


        public TouchButton2() : base()
        {
            
        }

        protected override void OnTouchContactDownHandler(object src, TouchContactEventArgs args)
        {
            args.TouchContact.Capture(this);
            base.IsPressed = true;

            if (ClickMode == ClickMode.Press)
            {
                OnClick();
            }
        }

        protected override void OnTouchContactUpHandler(object src, TouchContactEventArgs args)
        {
            if (args.TouchContact.CapturedElement == this )
            {
                base.IsPressed = false;
                if (ClickMode == ClickMode.Release)
                {
                    OnClick();
                }
                args.TouchContact.Release();
            }
        }

    }
}
