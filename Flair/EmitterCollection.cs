using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows;

namespace TangramApp1._35.Flair
{
    public class EmitterCollection : IList, ICollection, IEnumerable
    {
        private List<Emitter> emitters;
        private ParticleCanvas target;

        public EmitterCollection(ParticleCanvas t)
        {
            emitters = new List<Emitter>();
            target = t;
        }

        #region IList Members

        public int Add(object value)
        {
            Emitter e = ((Emitter)value);
            e.target = target;
            emitters.Add(e);
            return emitters.Count - 1;
        }

        public void Clear()
        {
            emitters.Clear();
        }

        public bool Contains(object value)
        {
            return emitters.Contains(value as Emitter);
        }

        public int IndexOf(object value)
        {
            return emitters.IndexOf(value as Emitter);
        }

        public void Insert(int index, object value)
        {
            emitters.Insert(index, value as Emitter);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            emitters.Remove(value as Emitter);
        }

        public void RemoveAt(int index)
        {
            emitters.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return emitters[index];
            }
            set
            {
                emitters[index] = value as Emitter;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return emitters.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return emitters; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return emitters.GetEnumerator();
        }

        #endregion
    }
}
