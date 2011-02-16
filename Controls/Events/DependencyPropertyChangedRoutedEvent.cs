using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TangramApp1._35.Controls.Events
{
    public delegate void DependencyPropertyChangedRoutedEvent(object src, DependencyPropertyChangedEventArgs args);

    public class DependencyPropertyChangedRoutedEventArgs : RoutedEventArgs
    {
        public DependencyPropertyChangedEventArgs info;
        public DependencyPropertyChangedRoutedEventArgs(): base() {}
        public DependencyPropertyChangedRoutedEventArgs(RoutedEvent evt) : base(evt) { }
        public DependencyPropertyChangedRoutedEventArgs(RoutedEvent routedEvent, Object src) : base(routedEvent, src) { }
        public DependencyPropertyChangedRoutedEventArgs(RoutedEvent routedEvent, Object src, DependencyPropertyChangedEventArgs i)
            : base(routedEvent, src)
        {
            info = i;
        }

        public DependencyPropertyChangedRoutedEventArgs(RoutedEvent routedEvent, DependencyPropertyChangedEventArgs i) : base(routedEvent)
        {
            info = i;
        }
    }
}
