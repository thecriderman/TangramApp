using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TangramApp1._35.Flair
{
    public interface ParticleLauncher
    {
        void launchParticle(Particle part);
        void placeParticle(Emitter relative, Particle part);
    }
}
