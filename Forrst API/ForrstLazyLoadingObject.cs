using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    /// <summary>
    /// An object containg data from the Forrst API that supports lazy loading.
    /// </summary>
    public abstract class ForrstLazyLoadingObject : ForrstObject
    {
        public ForrstLazyLoadingObject(ForrstClient client)
            : base(client) {
            this.Fields = new Dictionary<string, object>();
        }

        /// <summary>
        /// Contains all loaded data.
        /// </summary>
        private Dictionary<string, object> Fields { get; set; }

        /// <summary>
        /// Determines if a field's data has been loaded.
        /// </summary>
        /// <returns></returns>
        protected bool IsLoaded(string field) {
            return this.Fields.ContainsKey(field);
        }

        /// <summary>
        /// Returns a field's value. If the field has not yet been loaded, this is done now.
        /// If the field cannot be loaded, a CannotLoadException is thrown.
        /// </summary>
        protected T GetValue<T>(string field) {
            if (!this.Fields.ContainsKey(field)) this.TryLoad();

            if (this.Fields.ContainsKey(field))
                return (T)this.Fields[field];
            else
                throw new CannotLoadException();
        }

        protected void SetValue(string field, object value) {
            this.Fields[field] = value;
        }

        /// <summary>
        /// Tries to load the not yet loaded fields of this object using the present data.
        /// </summary>
        /// <returns>Returns if the present data allow loading.</returns>
        public abstract bool TryLoad();
    }
}
