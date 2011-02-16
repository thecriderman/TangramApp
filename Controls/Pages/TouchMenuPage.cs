using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Serialization;

namespace TangramApp1._35.Controls.Pages
{
    /// <summary>
    /// Base class for our newfangled menu system.
    /// </summary>
    public class TouchMenuPage : UserControl, ISerializable
    {
        /// <summary>
        /// 
        /// </summary>
        public TouchMenuPage()
            : base()
        {
            Loaded += new RoutedEventHandler(delegate(object src, RoutedEventArgs args){
                InstallVotableButtons();
            });
            Unloaded += new RoutedEventHandler(delegate(object src, RoutedEventArgs args){
                UninstallVotableButtons();
            });
        }

        /// <summary>
        /// Deserealizing constructor.
        /// Note, there should always be one of these constructors as it will be called
        /// by reflection on the change of Menu.
        /// </summary>
        /// <param name="?"></param>
        public TouchMenuPage(SerializationInfo info, StreamingContext context) : this()
        {

        }

        /// <summary>
        /// These methods should be implemented. These methods install 
        /// </summary>
        public virtual void InstallVotableButtons(){}
        public virtual void UninstallVotableButtons() {}

        #region ISerializable Members

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        #endregion

        /// <summary>
        /// Type should be at least of type TouchMenuPage.
        /// </summary>
        public virtual void GotoNextPage(Type pageType, object param)
        {
            TouchMenuPage nextPage = Activator.CreateInstance(pageType) as TouchMenuPage;

            Panel p = Parent as Panel;
            p.Children.Remove(this);
            p.Children.Add(nextPage);

            //once it is loaded, it should take over all generic functions.
        }

        /// <summary>
        /// The child should always know how to create the parent.
        /// </summary>
        /// <param name="param"></param>
        public virtual void GotoPreviousPage(object param)
        {
        }
    }
}
