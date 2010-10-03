using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Forrst
{
    public class Tag : ForrstLazyLoadingObject
    {
        public Tag(JToken json, ForrstClient client)
            : base(client) {
            this.ID = json.Value<int>("id");
            this.Name = json.Value<string>("name");
            this.Uri = new Uri(ForrstClient.BaseUri, json.Value<string>("posts_url"));
        }

        public int ID {
            get { return this.GetValue<int>("ID"); }
            private set { this.SetValue("ID", value); }
        }

        public string Name {
            get { return this.GetValue<string>("Name"); }
            private set { this.SetValue("Name", value); }
        }

        public Uri Uri {
            get { return this.GetValue<Uri>("Uri"); }
            private set { this.SetValue("Uri", value); }
        }

        public override bool TryLoad() {
            //Loading of single tags is not yet supported by the API
            return false;
        }
    }
}
