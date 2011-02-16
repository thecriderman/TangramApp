using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace TangramApp1._35.Flair
{
    public class Emitter : DependencyObject
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register("X", typeof(double), typeof(Emitter), new PropertyMetadata(0.0));
        public static readonly DependencyProperty YProperty = DependencyProperty.Register("Y", typeof(double), typeof(Emitter), new PropertyMetadata(0.0));
        public static readonly DependencyProperty EmitAmountProperty = DependencyProperty.Register("EmitAmount", typeof(int), typeof(Emitter), new PropertyMetadata(1));
        public static readonly DependencyProperty EmitIntervalProperty = DependencyProperty.Register("EmitInterval", typeof(int), typeof(Emitter), new PropertyMetadata(1));
        public static readonly DependencyProperty IsMutedProperty = DependencyProperty.Register("IsMuted", typeof(bool), typeof(Emitter), new PropertyMetadata(false));

        //object dependency properties
        public static readonly DependencyProperty ParticleLauncherProperty = DependencyProperty.Register("ParticleLauncher", typeof(ParticleLauncher), typeof(Emitter));
        public static readonly DependencyProperty ExampleParticleProperty = DependencyProperty.Register("ExampleParticle", typeof(Particle), typeof(Emitter));


        public ParticleCanvas target;

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        protected LinkedList<Particle> particles = new LinkedList<Particle>();
        private int numParticleGarbage = 0;

        public ParticleLauncher ParticleLauncher
        {
            get { return (ParticleLauncher)GetValue(ParticleLauncherProperty); }
            set { SetValue(ParticleLauncherProperty, value); }
        }

        public int EmitAmount
        {
            get { return (int)GetValue(EmitAmountProperty); }
            set { SetValue(EmitAmountProperty, value); }
        }

        public int EmitInterval
        {
            get { return (int)GetValue(EmitIntervalProperty); }
            set { SetValue(EmitIntervalProperty, value); }
        }

        private int time = 0;

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        

        public bool IsMuted
        {
            get { return (bool)GetValue(IsMutedProperty); }
            set { SetValue(IsMutedProperty, value); }
        }

        public Particle ExampleParticle
        {
            get { return (Particle)GetValue(ExampleParticleProperty); }
            set{ SetValue(ExampleParticleProperty, value); }
        }

        public ParticleCanvas Target
        {
            set
            {
                target = value;
            }
            get
            {
                return target;
            }
        }

        public void emit( Particle p )
        {
            ParticleLauncher.placeParticle(this, p);

            p.Lifetime = p.Lifetime;
            p.Time = 0;

            ParticleLauncher.launchParticle( p );
            // p.update(); //Don't update yet

            particles.AddFirst( p );
        }


        /// <summary>
        /// MAKES NEW PARTICALZ
        /// </summary>
        public void emit()
        {
            Particle p = ExampleParticle.clone();
            emit( p );
            Target.Children.Add(p.Image);
            //Console.Out.WriteLine("emitting");
        }

        /// <summary>
        /// Ultra Efficient
        /// </summary>
        public void update()
        {
            // see if we need to emit any particles
            time = (time + 1) % EmitInterval;

            int index = 0;
            int parts = 0; // the number of particles we create

            LinkedListNode<Particle> startUpdate = particles.First;

            // need to emit particles
            if (time == 0 && !IsMuted) //we dont emit if we are muted
            {
                parts = EmitAmount;
                // the number of particles to emit from recycling
                int recycleable = Math.Min(parts, numParticleGarbage);
                // recycle the old particles
                for (index = 0; index < recycleable; index++)
                {
                    Particle p = particles.Last.Value;
                    particles.RemoveLast();
                    emit(p); // should add the particle to the beginning of the
                }
                // the ram has just recycled its old particles
                numParticleGarbage -= recycleable;
                // create the rest of our particles
                int rest = parts - recycleable;
                while (rest > 0)
                {
                    emit(); // create a new particle
                    rest--;
                }
            }

            // Update all of the particles that were not created
            int updateable = particles.Count - numParticleGarbage;

            for (index = parts; index < updateable; index++)
            {
                Particle p = startUpdate.Value;
                LinkedListNode<Particle> next = startUpdate.Next;
                p.update();
                if (p.isGarbage())
                {
                    p.die();
                    particles.Remove(startUpdate);
                    // place the particle at the end with the rest of the garbage
                    particles.AddLast(startUpdate);
                    numParticleGarbage++;
                }
                startUpdate = next;
            }
        }


    }
}
