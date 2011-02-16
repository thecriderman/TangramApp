using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using libSMARTMultiTouch.Input;
using System.Windows.Input;

namespace TangramApp1._35.Controls.Behaviors
{
    /// <summary>
    /// FOR MY USE
    /// </summary>
    public class RotateAroundPointBehavior
    {
        private FrameworkElement AssociatedObject;
        private Point rotateAround;
        private bool _isRotateEnabled = true;

        private RotateTransform _rotateTransform;

        private Dictionary<int, TouchContact> _elementTouchContacts = new Dictionary<int, TouchContact>();
        private TouchContact _keyTouchContact;
        private Point? _previousKeyPosition; 
        private Point? _previousPosition;
        private UIElement _previousObjectParent;


        public RotateAroundPointBehavior(FrameworkElement target)
        {
            AssociatedObject = target;

            //wow... i cant believe i wrote that.
            if (target.RenderTransform != null)
            {
                //get the rotate transform out of the FrameworkElement
                if (target.RenderTransform is RotateTransform)
                {
                    _rotateTransform = (RotateTransform)target.RenderTransform;
                }
                else if (target.RenderTransform is TransformGroup)
                {
                    try
                    {
                        foreach (RotateTransform rottran in ((TransformGroup)target.RenderTransform).Children.OfType<RotateTransform>())
                        {
                            _rotateTransform = rottran;
                        }

                        //hopefully not going to happen
                        if (_rotateTransform == null)
                        {
                            throw new System.ArgumentNullException();//a waste i know.
                        }
                    }
                    catch (System.ArgumentNullException)
                    {
                        //there isnt a RotateTransform in it.
                        _rotateTransform = new RotateTransform();
                        ((TransformGroup)target.RenderTransform).Children.Add(_rotateTransform);
                    }
                }
                else
                {
                    //there exists no RotateTransform in the Element, need to create one.
                    TransformGroup transforms = new TransformGroup();
                    transforms.Children.Add(target.RenderTransform);
                    transforms.Children.Add(_rotateTransform = new RotateTransform());
                    target.RenderTransform = transforms;
                }
            }
            else
            {
                target.RenderTransform = _rotateTransform = new RotateTransform();
            }

            OnAttached();
        }

        public void OnAttached()
        {
            TouchInputManager.AddTouchContactMoveHandler(
                this.AssociatedObject, new TouchContactEventHandler(this.FrameworkElement_TouchContactMove));
            TouchInputManager.AddTouchContactDownHandler(
                this.AssociatedObject, new TouchContactEventHandler(this.FrameworkElement_TouchContactDown));
            TouchInputManager.AddTouchContactUpHandler(
                this.AssociatedObject, new TouchContactEventHandler(this.FrameworkElement_TouchContactUp));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameworkElement_TouchContactMove(object sender, TouchContactEventArgs e)
        {
            if (!this._elementTouchContacts.ContainsKey(e.TouchContact.ID))
            {
                if (e.TouchContact.CapturedElement != this.AssociatedObject)
                {
                    e.TouchContact.Release();
                    e.TouchContact.Capture(this.AssociatedObject);
                }
                if (!this._elementTouchContacts.ContainsKey(e.TouchContact.ID))
                {
                    this._elementTouchContacts.Add(e.TouchContact.ID, e.TouchContact);
                }
                this._keyTouchContact = e.TouchContact;
                this._previousKeyPosition = null;
                this._previousPosition = null;
                this._previousObjectParent = null;
            }
            else if (e.TouchContact == this._keyTouchContact)
            {
                UIElement parent = AssociatedObject.Parent as UIElement;
                if ((this._previousObjectParent != null) && (parent != this._previousObjectParent))
                {
                    this._previousObjectParent = parent;
                    parent = null;
                }
                if (parent != null)
                {
                    Point point = new Point(0.0, 0.0);
                    foreach (TouchContact contact in this._elementTouchContacts.Values)
                    {
                        Point point2 = contact.GetPosition(parent);
                        point.X += point2.X;
                        point.Y += point2.Y;
                    }
                    point.X /= (double)this._elementTouchContacts.Count;
                    point.Y /= (double)this._elementTouchContacts.Count;
                    Point position = this._keyTouchContact.GetPosition(parent);

                    if ((this._previousPosition.HasValue) && (this._previousKeyPosition.HasValue))
                    {
                        double num;
                        double num2;
                        double num3;
                        double num4;
                        if (this._elementTouchContacts.Count == 1)
                        {
                            Point point4 = new Point();
                            if (AssociatedObject is ICentroid)
                            {
                                point4 = ((ICentroid)AssociatedObject).getCentroid();
                            }
                            else
                            {
                                point4.X = this.AssociatedObject.ActualWidth / 2.0;
                                point4.Y = this.AssociatedObject.ActualHeight / 2.0;
                            }
                            point4 = this.AssociatedObject.TranslatePoint(point4, parent);
                            num = this._previousPosition.Value.X - point4.X;
                            num2 = _previousPosition.Value.Y - point4.Y;
                            num3 = point.X - point4.X;
                            num4 = point.Y - point4.Y;
                        }
                        else
                        {
                            num = this._previousPosition.Value.X - this._previousKeyPosition.Value.X;
                            num2 = this._previousPosition.Value.Y - this._previousKeyPosition.Value.Y;
                            num3 = point.X - position.X;
                            num4 = point.Y - position.Y;
                        }
                        double num5 = Math.Sqrt((num * num) + (num2 * num2));
                        double num6 = Math.Sqrt((num3 * num3) + (num4 * num4));
                        num /= num5;
                        num2 /= num5;
                        num3 /= num6;
                        num4 /= num6;
                        double d = (num * num3) + (num2 * num4);
                        double num8 = (num * num4) - (num3 * num2);
                        double num9 = (Math.Acos(d) * 180.0) / Math.PI;

                        if ((_isRotateEnabled && (num9 > 0.0)) && (num9 <= 180.0))
                        {
                            if (num8 > 0.0)
                            {
                                this._rotateTransform.Angle += num9;
                            }
                            else
                            {
                                this._rotateTransform.Angle -= num9;
                            }
                            if (AssociatedObject is ICentroid)
                            {
                                Point centroid = ((ICentroid)AssociatedObject).getCentroid();
                                _rotateTransform.CenterX = centroid.X;
                                _rotateTransform.CenterY = centroid.Y;
                            }
                            else
                            {
                                _rotateTransform.CenterX = this.AssociatedObject.ActualWidth / 2.0;
                                _rotateTransform.CenterY = this.AssociatedObject.ActualHeight / 2.0;
                            }
                            AssociatedObject.RenderTransform = _rotateTransform;
                        }
                    }
                    this._previousKeyPosition = new Point(position.X, position.Y);
                    this._previousPosition = new Point(point.X, point.Y);
                }
            }
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameworkElement_TouchContactUp(object sender, TouchContactEventArgs e)
        {
            if (this._elementTouchContacts.ContainsKey(e.TouchContact.ID))
            {
                this._elementTouchContacts.Remove(e.TouchContact.ID);
            }
            if (e.TouchContact == this._keyTouchContact)
            {
                if (this._elementTouchContacts.Count > 0)
                {
                    this._keyTouchContact = this._elementTouchContacts.Last<KeyValuePair<int, TouchContact>>().Value;
                }
                else
                {
                    this._keyTouchContact = null;
                }
            }
            _previousKeyPosition = null;
            _previousPosition = null;
            _previousObjectParent = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrameworkElement_TouchContactDown(object sender, TouchContactEventArgs e)
        {
            
            _elementTouchContacts.Add(e.TouchContact.ID, e.TouchContact);
        }
    }
}
