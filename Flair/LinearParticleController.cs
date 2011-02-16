using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TangramApp1._35.Flair
{
    public class LinearParticleController : ParticleController
    {
        private double vx, vy;

        public double Vy
        {
            get { return vy; }
            set { vy = value; }
        }

        public double Vx
        {
            get { return vx; }
            set { vx = value; }
        }

        public LinearParticleController(double _vx, double _vy)
        {
            Vx = _vx;
            Vy = _vy;
        }

        public void updateParticle( Particle p )
        {
            p.Vx = vx;
            p.Vy = vy;
            p.X = p.X + vx;
            p.Y = p.Y + vy;
            p.Time++;
        }
    }
}
