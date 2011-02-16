using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using libSMARTMultiTouch.Input;
using TangramApp1._35.Effects;
using System.Windows;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// Provides enhanced controls for touch input.
    /// </summary>
    public class TouchToggleButton : TouchButtonBase
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(TouchToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Journal | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TouchToggleButton.OnIsCheckedChanged)));
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchToggleButton));
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TouchToggleButton));

        public TouchToggleButton() : base()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public event RoutedEventHandler Checked
        {
            add
            {
                base.AddHandler(CheckedEvent, value);
            }
            remove
            {
                base.RemoveHandler(CheckedEvent, value);
            }
        }

        public event RoutedEventHandler Unchecked
        {
            add
            {
                base.AddHandler(UncheckedEvent, value);
            }
            remove
            {
                base.RemoveHandler(UncheckedEvent, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        public static void OnIsCheckedChanged(object obj, DependencyPropertyChangedEventArgs args)
        {
            TouchToggleButton butt = (TouchToggleButton)obj;
            bool oldv = (bool)args.OldValue;
            bool newv = (bool)args.NewValue;

            if (newv)
            {
                butt.OnChecked(new RoutedEventArgs(CheckedEvent));
            }
            else
            {
                butt.OnUnchecked(new RoutedEventArgs(UncheckedEvent));
            }

        }

        public virtual void OnChecked(RoutedEventArgs e)
        {
            base.RaiseEvent(e);
        }

        public virtual void OnUnchecked(RoutedEventArgs e)
        {
            base.RaiseEvent(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        protected override void OnTouchContactDownHandler(object src, TouchContactEventArgs args)
        {
            args.TouchContact.Capture(this);
            base.IsPressed = true;
            if (ClickMode == ClickMode.Press)
            {
                IsChecked = !IsChecked;
                OnClick();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        protected override void OnTouchContactUpHandler(object src, TouchContactEventArgs args)
        {
            if (args.TouchContact.CapturedElement == this)
            {
                base.IsPressed = false;
                args.TouchContact.Release();
                if (ClickMode == ClickMode.Release)
                {
                    IsChecked = !IsChecked;
                    OnClick();
                }
            }
        }
    }
}
