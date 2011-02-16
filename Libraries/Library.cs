using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TangramApp1._35.Libraries
{
    /// <summary>
    /// Supports loading library data from a file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Library<T>
    {
        /// <summary>
        /// 
        /// </summary>
        protected Dictionary<string, T> library = new Dictionary<string, T>();

        /// <summary>
        /// Will call LoadLibrary.
        /// </summary>
        public abstract void LoadDefaultLibrary();
        
        /// <summary>
        /// Loads a library from the specified path.
        /// </summary>
        /// <param name="basepath"></param>
        public void LoadLibrary(string basepath, string extention, params string[] itempath)
        {
            foreach (String path in itempath)
            {
                LoadItem(basepath + path + extention);
            }
        }

        public abstract void LoadItem(string fullitempath);

        /// <summary>
        /// Gets or sets the items in the library.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T this[string key]{
            get{ return library[key]; }
            set{ library[key] = value; }
        }
    }
}
