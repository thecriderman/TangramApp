using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace TangramApp1._35.Flair
{
    public class Particle : DependencyObject
    {
        public static readonly DependencyProperty ParticleImageProperty = DependencyProperty.Register("Image", typeof(Image), typeof(Particle), new PropertyMetadata(new PropertyChangedCallback(ImageChangedCallback)));
        public static readonly DependencyProperty LifetimeProperty = DependencyProperty.Register("Lifetime", typeof(int), typeof(Particle));
        public static readonly DependencyProperty ParticleControllerProperty = DependencyProperty.Register("ParticleController", typeof(ParticleController), typeof(Particle));

        private TranslateTransform translateTransform;
        private ScaleTransform scaleTransform;
        private RotateTransform rotateTransform;

        /// <summary>
        /// 
        /// </summary>
        public Particle() : base()
        {
            Image = new Image();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="args"></param>
        public static void ImageChangedCallback(DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            Image i = (Image)args.NewValue;
            Particle p = (Particle)src;

            p.translateTransform = new TranslateTransform();
            p.rotateTransform = new RotateTransform();
            p.scaleTransform = new ScaleTransform();

            TransformGroup trans = new TransformGroup();
            trans.Children.Add(p.translateTransform);
            trans.Children.Add(p.scaleTransform);
            trans.Children.Add(p.rotateTransform);

            i.RenderTransform = trans;
        }

        /// <summary>
        /// 
        /// </summary>
        public Image Image{
            get
            {
                return (Image)GetValue(ParticleImageProperty);
            }
            set
            {
                SetValue(ParticleImageProperty, value);
            }
        }

        public double X
        {
            get { return translateTransform.X; }
            set { translateTransform.X = value; }
        }

        public double Y
        {
            get { return translateTransform.Y; }
            set { translateTransform.Y = value; }
        }


        public int Lifetime
        {
            get { return (int)GetValue(LifetimeProperty); }
            set { SetValue(LifetimeProperty, value); }
        }

        /**
         * the life of the particle. When the life reaches the lifetime this says to
         * the
         */
        private int time = 0;

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        /**
         * Seperating the particle into a particle Controller allows for creation of
         * particles with a much lower overhead.
         */

        public ParticleController PartControl
        {
            get { return (ParticleController)GetValue(ParticleControllerProperty);  }
            set { SetValue(ParticleControllerProperty, value); }
        }

        /**
         * The velocity of the particle in the x-direction per update.
         */
        private double vx;
        public double Vx
        {
            get { return vx; }
            set { vx = value; }
        }

        /**
         * The velocity of the particle in the y-direction per update.
         */
        private double vy;

        public double Vy
        {
            get { return vy; }
            set { vy = value; }
        }


        /**
         * Updates the particle
         */
        public void update()
        {
            PartControl.updateParticle(this);
        }


        /**
         * Returns whether or not the particle is ready to be recycled.
         */
        public bool isGarbage()
        {
            return (time >= Lifetime);
        }

        public void launched()
        {
            Image.Opacity = 1.0;
        }

        public void die()
        {
            Image.Opacity = 0; //hide the image
        }

        /// <summary>
        /// Clones this particle.
        /// Does not copy ALL data.
        /// Copies necessary information to allow for quick use.
        /// </summary>
        /// <returns>A clone of this Particle.</returns>
        internal Particle clone()
        {
            Particle cloned = new Particle();
            cloned.Lifetime = this.Lifetime;
            cloned.Image.Source = this.Image.Source;
            cloned.PartControl = PartControl;
            return cloned;
        }
    }
}
