using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TangramApp1._35.Controls.Events
{
    public enum VotableButtonTrayChangedType : byte
    {
        VOTETARGET
    }

    public class VotableButtonTrayChangedRoutedEventArgs : RoutedEventArgs
    {
        private VotableButtonTrayChangedType m_changedType;
        public VotableTouchButton2 from;
        public VotableTouchButton2 to;

        public VotableButtonTrayChangedRoutedEventArgs(RoutedEvent evt, Object src, VotableTouchButton2 original, VotableTouchButton2 newv) : base(evt, src) 
        {
            from = original;
            to = newv;
        }

    }

    public delegate void VotableButtonTrayChangedEventHandler(object src, VotableButtonTrayChangedRoutedEventArgs args);
}
