using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TangramApp1._35.Flair
{
    public class RandomPolarParticleLauncher : DependencyObject, ParticleLauncher
    {
        public static readonly DependencyProperty MinVelocityProperty = DependencyProperty.Register("MinVelocity", typeof(double), typeof(RandomPolarParticleLauncher), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MaxVelocityProperty = DependencyProperty.Register("MaxVelocity", typeof(double), typeof(RandomPolarParticleLauncher), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", typeof(double), typeof(RandomPolarParticleLauncher), new PropertyMetadata(0.0));
        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", typeof(double), typeof(RandomPolarParticleLauncher), new PropertyMetadata(360.0));

        protected Random particleRandomizer = App.RANDOM;

        public double MinVelocity
        {
            set{ SetValue(MinVelocityProperty, value); }
            get{ return (double)GetValue(MinVelocityProperty); }
        }

        public double MaxVelocity
        {
            set { SetValue(MaxVelocityProperty, value); }
            get { return (double)GetValue(MaxVelocityProperty); }
        }

        public double MinAngle
        {
            set{ SetValue(MinAngleProperty, value);}
            get { return (double)GetValue(MinAngleProperty); }
        }

        public double MaxAngle
        {
            set { SetValue(MaxAngleProperty, value); }
            get { return (double)GetValue(MaxAngleProperty); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relative"></param>
        /// <param name="p"></param>
        public virtual void placeParticle(Emitter relative, Particle p)
        {
            p.X = relative.X;
            p.Y = relative.Y;
        }


        /// <summary>
        /// For this kind of particle launcher, we assume that the particle has
        /// already been placed by the emitter for launching.
        /// </summary>
        /// <param name="part"></param>
        public virtual void launchParticle(Particle part)
        {
            double minTheta = MinAngle * Math.PI / 180.0;
            double maxTheta = MaxAngle * Math.PI / 180.0;
            double minv = MinVelocity;
            double maxv = MaxVelocity;

            double launchAngle = minTheta + (particleRandomizer.NextDouble()
                * (maxTheta - minTheta));
            double launchVelocity = minv + (particleRandomizer.NextDouble()
                * (maxv - minv));

            part.Vx = launchVelocity * Math.Cos(launchAngle);
            part.Vy = launchVelocity * Math.Sin(launchAngle);

            //notify the particle that it has been launched.
            part.launched();
        }
    }

}
