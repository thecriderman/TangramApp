using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TangramApp1._35.Flair
{
    public class QuadraticParticleController : DependencyObject, ParticleController
    {
        public static readonly DependencyProperty AccelerationXProperty = DependencyProperty.Register("AccelerationX", typeof(double), typeof(QuadraticParticleController));
        public static readonly DependencyProperty AccelerationYProperty = DependencyProperty.Register("AccelerationY", typeof(double), typeof(QuadraticParticleController));

        public double Ay
        {
            get { return (double)GetValue(AccelerationYProperty); }
            set { SetValue(AccelerationYProperty, value); }
        }

        public double Ax
        {
            get { return (double)GetValue(AccelerationXProperty); }
            set { SetValue(AccelerationXProperty, value); }
        }

        public void updateParticle( Particle p )
        {
            p.Vx += Ax;
            p.Vy += Ay;
            p.X = p.X + p.Vx;
            p.Y = p.Y + p.Vy;
            p.Image.Opacity = Math.Max((1 - p.Time * 1.0 / p.Lifetime), 0);
            p.Time++;
        }
    }
}
