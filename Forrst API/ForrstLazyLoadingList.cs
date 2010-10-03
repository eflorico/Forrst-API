using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    /// <summary>
    /// Represents a list of data from the Forrst API that is loading lazily and that can be accessed through a foreach loop.
    /// </summary>
    public class ForrstLazyLoadingList<T> : ForrstObject, IEnumerable<T> where T : ForrstObject
    {
        public ForrstLazyLoadingList(Func<List<T>, bool> loader, ForrstClient client) 
            : base(client) {
            this.LoadedItems = new List<T>();
            this.CanLoadMore = true;
            this.Loader = loader;
        }

        /// <summary>
        /// The already loaded items.
        /// </summary>
        protected List<T> LoadedItems { get; private set; }

        /// <summary>
        /// Determines if more items can be loaded. 
        /// Note: Even if this is set to true, this doesn't necessarily mean there are more items to load.
        /// </summary>
        protected bool CanLoadMore { get; private set; }

        /// <summary>
        /// The function that loads more items.
        /// </summary>
        protected Func<List<T>, bool> Loader { get; private set; }

        public IEnumerator<T> GetEnumerator() {
            //Return the already loaded items first
            foreach (var item in this.LoadedItems) yield return item;
            var count = this.LoadedItems.Count;

            //Then load more items as long as possible
            while (this.CanLoadMore) {
                if (!this.Loader(this.LoadedItems)) this.CanLoadMore = false;

                //Return the newly loaded items
                foreach (var item in this.LoadedItems.Skip(count)) yield return item;
                count = this.LoadedItems.Count;
            }

            yield break;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
