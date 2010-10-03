using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    public class ForrstList<T> : ForrstObject, IEnumerable<T> where T : ForrstObject
    {
        public ForrstList(Func<List<T>, bool> loader, ForrstClient client) 
            : base(client) {
            this.LoadedItems = new List<T>();
            this.CanLoadMore = true;
            this.Loader = loader;
        }

        protected List<T> LoadedItems { get; private set; }

        protected bool CanLoadMore { get; private set; }

        protected Func<List<T>, bool> Loader { get; private set; }

        public IEnumerator<T> GetEnumerator() {
            foreach (var item in this.LoadedItems) yield return item;
            var count = this.LoadedItems.Count;

            while (this.CanLoadMore) {
                if (!this.Loader(this.LoadedItems)) this.CanLoadMore = false;

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
