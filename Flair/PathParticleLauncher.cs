using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TangramApp1._35.Flair
{
    public class PathParticleLauncher : RandomPolarParticleLauncher
    {
        public static readonly DependencyProperty PathGeometryProperty = DependencyProperty.Register("PathGeometry", typeof(PathGeometry), typeof(PathParticleLauncher));
        
        public PathGeometry PathGeometry
        {
            get { return (PathGeometry)GetValue(PathGeometryProperty); }
            set { SetValue(PathGeometryProperty, value); }
        }

        #region ParticleLauncher Members

        public override void launchParticle(Particle part)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relative"></param>
        /// <param name="part"></param>
        public override void placeParticle(Emitter relative, Particle part)
        {
            PathGeometry p = PathGeometry;
            Point location, tangent;
            p.GetPointAtFractionLength(particleRandomizer.NextDouble(), out location,out tangent);
            part.X = location.X;
            part.Y = location.Y;
        }

        #endregion
    }
}
