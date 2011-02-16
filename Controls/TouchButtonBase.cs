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
    /// Issues with TouchButton caused me to realize that the
    /// origin of the problem lies with the click events
    /// from BUTTON.
    /// </summary>
    public class TouchButtonBase : ContentControl
    {
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register("IsPressed", typeof(bool), typeof(TouchButtonBase), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(TouchButtonBase.OnIsPressedChanged)));
        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register("ClickMode", typeof(ClickMode), typeof(TouchButtonBase), new FrameworkPropertyMetadata(ClickMode.Release), new ValidateValueCallback(TouchButtonBase.IsValidClickMode));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchButtonBase));

        public TouchButtonBase() : base()
        {
            TouchInputManager.AddTouchContactDownHandler(this, new TouchContactEventHandler(OnTouchContactDownHandler));
            TouchInputManager.AddTouchContactUpHandler(this, new TouchContactEventHandler(OnTouchContactUpHandler));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPressed
        {
            set
            {
                SetValue(IsPressedProperty, value);
            }
            get
            {
                return (bool)GetValue(IsPressedProperty);
            }
        }

        public ClickMode ClickMode
        {
            set
            {
                SetValue(ClickModeProperty, value);
            }
            get
            {
                return (ClickMode)GetValue(ClickModeProperty);
            }
        }

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(ClickEvent, value);
            }
        }


        private static void OnIsPressedChanged(object src, DependencyPropertyChangedEventArgs args)
        {
            ((TouchButtonBase)src).OnIsPressedChanged(args);
        }

        protected virtual void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {

        }

        private static bool IsValidClickMode(object src)
        {
            ClickMode mode = (ClickMode)src;
            //if somehow the result is not one of these, then <angryface>
            return (mode == ClickMode.Release) || (mode == ClickMode.Hover) || (mode == ClickMode.Press);
        }

        protected virtual void OnClick()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent,this));
        }


        protected virtual void OnTouchContactDownHandler(object src, TouchContactEventArgs args)
        {

        }

        protected virtual void OnTouchContactUpHandler(object src, TouchContactEventArgs args)
        {

        }
    }

    
}
