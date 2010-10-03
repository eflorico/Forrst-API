using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forrst
{
    public abstract class ForrstLazyLoadingObject : ForrstObject
    {
        public ForrstLazyLoadingObject(ForrstClient client)
            : base(client) {
            this.Fields = new Dictionary<string, object>();
        }

        private Dictionary<string, object> Fields { get; set; }

        protected bool IsLoaded(string field) {
            return this.Fields.ContainsKey(field);
        }

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

        protected abstract bool TryLoad();
    }
}
