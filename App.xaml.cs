using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace TangramApp1._35
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// Stores constants that are used by the application at various points.
    /// Instead of compile constants as done before, which would require me to
    /// change each and then compile, I sacrifice less of my time for miniscule
    /// longer runtime.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Random number generator used any time we need a randomly generated number.
        /// </summary>
        public static Random RANDOM = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Controlled Constant that denotes the number of users using the table.
        /// Usually I would modify "TableManager.TotalLearners" but do to an access exception, 
        /// (Modifying the value causes a change in the registry, which might be locked from changes)
        /// this is not possible.
        /// </summary>
        public static int TABLE_USERS = 3; //TableManager.TotalLearners;

        /// <summary>
        /// When false, mouse takes over for touch events.
        /// Useful for the Locking Behaviors.
        /// </summary>
        public static bool TABLE_MODE = false;

        /// <summary>
        /// Speaks for itself.
        /// </summary>
        public static bool DEBUG_MODE = false;

        /// <summary>
        /// Allows for much easier testing.
        /// </summary>
        public static bool QUICKTEST_MODE = false;
    }
}
