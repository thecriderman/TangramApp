using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using TangramApp1._35.Controls.Behaviors;

namespace TangramApp1._35.Controls
{
    /// <summary>
    /// A control that allows for touch input in order to translate, rotate, <no>scale</no>
    /// the control as needed. Implements ICentroid for physics specific rotation.
    /// </summary>
    public class DraggableControl: ContentControl, ICentroid
    {
        private RotateTransform _rotateTransform;
        private TranslateTransform _translateTransform;
        private ScaleTransform _scaleTransform;

        //Very useful feature
        private RotateTranslateBehavior _rotateTranslateBehavior;

        /// <summary>
        /// Creates a draggable control.
        /// </summary>
        public DraggableControl() : base()
        {
            _rotateTransform = new RotateTransform();
            _translateTransform = new TranslateTransform();
            _scaleTransform = new ScaleTransform();

            TransformGroup tgroup = new TransformGroup();
            tgroup.Children.Add(_rotateTransform);
            tgroup.Children.Add(_translateTransform);
            tgroup.Children.Add(_scaleTransform);

            RenderTransform = tgroup;

            //
            _rotateTranslateBehavior = new RotateTranslateBehavior(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTranslateEnabled
        {
            get { return _rotateTranslateBehavior.IsRotateEnabled; }
            set { _rotateTranslateBehavior.IsRotateEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRotateEnabled
        {
            set{ _rotateTranslateBehavior.IsRotateEnabled = value; }
            get { return _rotateTranslateBehavior.IsRotateEnabled; }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public bool IsBringToFrontEnabled
        {
            set { } //cant be done yet
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsScaleEnabled
        {
            get
            {
                return false; //CANT DO THAT
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RotateTransform RotateTransform
        {
            get
            {
                return _rotateTransform;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TranslateTransform TranslateTransform
        {
            get
            {
                return _translateTransform;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ScaleTransform ScaleTransform
        {
            get
            {
                return ScaleTransform;
            }
        }

        /// <summary>
        /// Notifies the Control that its underlying transforms were 
        /// changed and need to be updated.
        /// </summary>
        public void NotifyTransformChanged()
        {
        }

        /// <summary>
        /// Gets the centroid "center of rotation of the Control"
        /// </summary>
        /// <returns>The Centroid of the Control.</returns>
        public virtual Point getCentroid()
        {
            return new Point(Width/2.0, Height/2.0);
        }
    }
}
