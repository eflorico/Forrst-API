using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Forrst
{
    public class User : ForrstLazyLoadingObject
    {
        public User(int id, ForrstClient client)
            : base(client) {
            this.ID = id;
        }

        public User(string username, ForrstClient client)
            : base(client) {
            this.Username = username;
        }

        public User(JToken json, ForrstClient client)
            : base(client) {
                this.FromJson(json);
        }

        public int ID {
            get { return this.GetValue<int>("ID"); }
            private set { this.SetValue("ID", value); }
        }

        public string Username {
            get { return this.GetValue<string>("Username"); }
            private set { this.SetValue("Username", value); }
        }

        public ForrstList<Post> Posts {
            get {
                return new ForrstList<Post>(loadedItems => {
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("username", this.Username);
                    if (loadedItems.Count > 0) parameters.Add("since", loadedItems.Last().ID.ToString());

                    var response = (JArray)this.Client.Request("users/posts", parameters, "posts");
                    foreach (var post in response) loadedItems.Add(new Post(post, this.Client));

                    return response.Count == 10;
                }, this.Client);
            }
        }

        protected void FromJson(JToken json) {
            this.ID = json.Value<int>("id");
            this.Username = json.Value<string>("username");
        }

        protected override bool TryLoad() {
            if (this.IsLoaded("Username") || this.IsLoaded("ID")) {
                var parameters = new Dictionary<string, string>();

                if (this.IsLoaded("Username"))
                    parameters.Add("username", this.Username);
                else
                    parameters.Add("id", this.ID.ToString());

                try {
                    var response = this.Client.Request("users/info", parameters, "user");
                    this.FromJson(response);
                    return true;
                }
                catch (WebException ex) {
                    if (ex.Response is HttpWebResponse && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                        return false;
                    else throw ex;
                }
                return true;
            }
            else return false;
        }
    }
}
