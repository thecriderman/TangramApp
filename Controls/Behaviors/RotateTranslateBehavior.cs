using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libSMARTMultiTouch.Input;
using System.Windows;

namespace TangramApp1._35.Controls.Behaviors
{
    /// <summary>
    /// Allows for rotation and translation of Elements.
    /// Not in the description, places the object being moved on TOP.
    /// </summary>
    public class RotateTranslateBehavior
    {
        private bool _rotateEnabled = true;
        private bool _translateEnabled = true;
        private bool _bringToFront = true;

        private Dictionary<int, TouchContact> _touchContacts = new Dictionary<int, TouchContact>();


        /// <summary>
        /// Used draggable element as the base as it allows for more scenarios.
        /// </summary>
        private DraggableControl AssociatedObject;

        public RotateTranslateBehavior(DraggableControl target)
        {
            AssociatedObject = target;
            OnAttached();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRotateEnabled
        {
            get { return _rotateEnabled; }
            set { _rotateEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsTranslateEnabled
        {
            get { return _translateEnabled; }
            set { _translateEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnAttached()
        {
            TouchInputManager.AddTouchContactDownHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactDown));
            TouchInputManager.AddTouchContactUpHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactUp));
            TouchInputManager.AddTouchContactMoveHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactMoved));
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDetached()
        {
            TouchInputManager.RemoveTouchContactDownHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactDown));
            TouchInputManager.RemoveTouchContactUpHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactUp));
            TouchInputManager.RemoveTouchContactMoveHandler(AssociatedObject, new TouchContactEventHandler(AssociatedObject_TouchContactMoved));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void AssociatedObject_TouchContactDown(object src, TouchContactEventArgs args)
        {
            if (!_touchContacts.ContainsKey(args.TouchContact.ID))
            {
                _touchContacts.Add(args.TouchContact.ID, args.TouchContact);
                args.TouchContact.Capture(this.AssociatedObject);

                _keyTouchContact = args.TouchContact;
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void AssociatedObject_TouchContactUp(object src, TouchContactEventArgs args)
        {
            if (args.TouchContact.CapturedElement == AssociatedObject)
            {
                _touchContacts.Remove(args.TouchContact.ID);
                args.TouchContact.Release();
            }
            if (args.TouchContact == this._keyTouchContact)
            {
                if (_touchContacts.Count > 0)
                {
                    //use the last touch contact as the new key.
                    _keyTouchContact = _touchContacts.Last<KeyValuePair<int, TouchContact>>().Value;
                }
                else
                {
                    _keyTouchContact = null;
                }
            }
            _previousDistance = 0;
            _previousObjectParent = null;
            _previousPosition = null;
            _previousKeyPosition = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private TouchContact _keyTouchContact;
        private UIElement _previousObjectParent;
        private Point? _previousKeyPosition;
        private Point? _previousPosition;
        private double _previousDistance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        private void AssociatedObject_TouchContactMoved(object src, TouchContactEventArgs args)
        {

            //make sure that we have the key
            if (!this._touchContacts.ContainsKey(args.TouchContact.ID))
            {
                if (args.TouchContact.CapturedElement != this.AssociatedObject)
                {
                    args.TouchContact.Release();
                    args.TouchContact.Capture(this.AssociatedObject);
                    this._touchContacts.Add(args.TouchContact.ID, args.TouchContact);
                }

                this._keyTouchContact = args.TouchContact;
                this._previousKeyPosition = null;
                this._previousPosition = null;
                this._previousDistance = 0.0;
                this._previousObjectParent = null;
            }
            else if ((this._touchContacts.Count >= 2))
            {
                if (args.TouchContact != this._keyTouchContact)
                {
                    return; //dont waste my time
                }

                UIElement parent = AssociatedObject.Parent as UIElement;
                if ((this._previousObjectParent != null) && (parent != _previousObjectParent))
                {
                    this._previousObjectParent = parent;
                    parent = null;
                }
                if (parent != null)
                {
                    Point point = new Point(0.0, 0.0);
                    foreach (TouchContact contact in this._touchContacts.Values)
                    {
                        Point point2 = contact.GetPosition(this.AssociatedObject);
                        point.X += point2.X;
                        point.Y += point2.Y;
                    }
                    point.X /= (double)this._touchContacts.Count;
                    point.Y /= (double)this._touchContacts.Count;
                    Point point3 = this.AssociatedObject.TranslatePoint(point, parent);
                    double num = 0.0;

                    foreach (TouchContact contact2 in this._touchContacts.Values)
                    {
                        Point point4 = contact2.GetPosition(parent);
                        num += Math.Sqrt(((point4.X - point3.X) * (point4.X - point3.X)) + ((point4.Y - point3.Y) * (point4.Y - point3.Y)));
                    }

                    num /= (double)this._touchContacts.Count;
                    Point position = this._keyTouchContact.GetPosition(parent);
                    if ((this._previousKeyPosition.HasValue && _previousPosition.HasValue) && (this._touchContacts.Count >= 1))
                    {
                        double num2 = this._previousPosition.Value.X - this._previousKeyPosition.Value.X;
                        double num3 = this._previousPosition.Value.Y - this._previousKeyPosition.Value.Y;
                        double num4 = point3.X - position.X;
                        double num5 = point3.Y - position.Y;
                        double num6 = Math.Sqrt((num2 * num2) + (num3 * num3));
                        double num7 = Math.Sqrt((num4 * num4) + (num5 * num5));
                        num2 /= num6;
                        num3 /= num6;
                        num4 /= num7;
                        num5 /= num7;
                        double d = (num2 * num4) + (num3 * num5);
                        double num9 = (num2 * num5) - (num4 * num3);
                        double num10 = (Math.Acos(d) * 180.0) / 3.1415926535897931;
                        if ((this._rotateEnabled && (num10 > 0.0)) && (num10 <= 180.0))
                        {
                            if (num9 > 0.0)
                            {
                                this.AssociatedObject.RotateTransform.Angle += num10;
                            }
                            else
                            {
                                this.AssociatedObject.RotateTransform.Angle -= num10;
                            }
                            //
                            //this.AssociatedObject.NotifyRotateTransformUpdated();
                        }
                    }
                    this._previousKeyPosition = new Point(position.X, position.Y);

                    #region SCALECODE
                    //SCALE CODE REMOVED
                    /*if (this.m_bIsScaleEnabled && this.m_dPreviousDistance.HasValue)
                    {

                        double num11 = num / this.m_dPreviousDistance.Value;
                        if ((((this.m_scaleTransform.ScaleX * num11) < base.AssociatedObject.MinScale) || ((this.m_scaleTransform.ScaleY * num11) < base.AssociatedObject.MinScale)) && (num11 < 1.0))
                        {
                            this.m_scaleTransform.ScaleX = base.AssociatedObject.MinScale;
                            this.m_scaleTransform.ScaleY = base.AssociatedObject.MinScale;
                        }
                        else if ((((this.m_scaleTransform.ScaleX * num11) > base.AssociatedObject.MaxScale) || ((this.m_scaleTransform.ScaleX * num11) > base.AssociatedObject.MaxScale)) && (num11 > 1.0))
                        {
                            this.m_scaleTransform.ScaleX = base.AssociatedObject.MaxScale;
                                this.m_scaleTransform.ScaleY = base.AssociatedObject.MaxScale;
                        }
                        else
                        {
                            this.m_scaleTransform.ScaleX *= num11;
                            this.m_scaleTransform.ScaleY *= num11;
                        }
                        Point point6 = this.m_element.TranslatePoint(point, parent);
                        this.m_translateTransform.X += point3.X - point6.X;
                        this.m_translateTransform.Y += point3.Y - point6.Y;
                        base.AssociatedObject.NotifyScaleTransformUpdated();
                    }*/
                    #endregion

                    this._previousDistance = num;
                    if (this._translateEnabled && this._previousPosition.HasValue)
                    {
                        this.AssociatedObject.TranslateTransform.X += point3.X - this._previousPosition.Value.X;
                        this.AssociatedObject.TranslateTransform.Y += point3.Y - this._previousPosition.Value.Y;
                        //Console.WriteLine("Moving " + (point3.X - this._previousPosition.Value.X));
                        //base.AssociatedObject.NotifyTranslateTransformUpdated();
                    }
                    this._previousPosition = point3;
                }
            }
            else if (this._touchContacts.Count == 1) //specific version for rotation around a point
            {
                if (args.TouchContact != this._keyTouchContact)
                {
                    return; //dont waste my time
                }

                UIElement parent = AssociatedObject.Parent as UIElement;
                if ((this._previousObjectParent != null) && (parent != _previousObjectParent))
                {
                    this._previousObjectParent = parent;
                    parent = null;
                }


            }
        }
    }
}
