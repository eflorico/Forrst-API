using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Forrst
{
    public class Post : ForrstLazyLoadingObject
    {
        public Post(int id, ForrstClient client)
            : base(client) {
                this.ID = id;
        }

        public Post(JToken json, ForrstClient client)
            : base(client) {
            this.FromJson(json);
        }

        public int ID {
            get { return this.GetValue<int>("ID"); }
            private set { this.SetValue("ID", value); }
        }

        public Uri PostUri {
            get { return this.GetValue<Uri>("PostUri"); }
            private set { this.SetValue("PostUri", value); }
        }

        public string TinyId {
            get { return this.GetValue<string>("TinyId"); }
            private set { this.SetValue("TinyId", value); }
        }

        public User User {
            get { return this.GetValue<User>("User"); }
            private set { this.SetValue("User", value); }
        }

        public PostType Type {
            get { return this.GetValue<PostType>("Type"); }
            private set { this.SetValue("Type", value); }
        }

        public string Title {
            get { return this.GetValue<string>("Title"); }
            private set { this.SetValue("Title", value); }
        }

        public string Content {
            get { return this.GetValue<string>("Content"); }
            private set { this.SetValue("Content", value); }
        }

        public string FormattedContent {
            get { return this.GetValue<string>("FormattedContent"); }
            private set { this.SetValue("FormattedContent", value); }
        }

        public string MarkdownContent {
            get { return this.GetValue<string>("MarkdownContent"); }
            private set { this.SetValue("MarkdownContent", value); }
        }

        public string Description {
            get { return this.GetValue<string>("Description"); }
            private set { this.SetValue("Description", value); }
        }

        public string FormattedDescription {
            get { return this.GetValue<string>("FormattedDescription"); }
            private set { this.SetValue("FormattedDescription", value); }
        }

        public string MarkdownDescription {
            get { return this.GetValue<string>("MarkdownDescription"); }
            private set { this.SetValue("MarkdownDescription", value); }
        }

        public Uri Link {
            get { return this.GetValue<Uri>("Link"); }
            private set { this.SetValue("Link", value); }
        }

        public string SnapFileName {
            get { return this.GetValue<string>("SnapFileName"); }
            private set { this.SetValue("SnapFileName", value); }
        }

        public string SnapContentType {
            get { return this.GetValue<string>("SnapContentType"); }
            private set { this.SetValue("SnapContentType", value); }
        }

        public int SnapFileSize {
            get { return this.GetValue<int>("SnapFileSize"); }
            private set { this.SetValue("SnapFileSize", value); }
        }


        public DateTime SnapUpdatedAt {
            get { return this.GetValue<DateTime>("SnapUpdatedAt"); }
            private set { this.SetValue("SnapUpdatedAt", value); }
        }

        public DateTime CreatedAt {
            get { return this.GetValue<DateTime>("CreatedAt"); }
            private set { this.SetValue("CreatedAt", value); }
        }

        public DateTime UpdatedAt {
            get { return this.GetValue<DateTime>("UpdatedAt"); }
            private set { this.SetValue("UpdatedAt", value); }
        }

        public int ShortUriRedirects {
            get { return this.GetValue<int>("ShortUriRedirects"); }
            private set { this.SetValue("ShortUriRedirects", value); }
        }

        public int Views {
            get { return this.GetValue<int>("Views"); }
            private set { this.SetValue("Views", value); }
        }

        public int CommentCount {
            get { return this.GetValue<int>("CommentCount"); }
            private set { this.SetValue("CommentCount", value); }
        }

        public int LikeCount {
            get { return this.GetValue<int>("LikeCount"); }
            private set { this.SetValue("LikeCount", value); }
        }

        public List<Tag> Tags {
            get { return this.GetValue <List<Tag>>("Tags"); }
            private set { this.SetValue("Tags", value); }
        }

        public bool IsPublic {
            get { return this.GetValue<bool>("IsPublic"); }
            private set { this.SetValue("IsPublic", value); }
        }

        public Post InReplyTo {
            get { return this.GetValue<Post>("InReplyTo"); }
            private set { this.SetValue("InReplyTo", value); }
        }

        protected void FromJson(JToken json) {
            this.ID = json.Value<int>("id");
            this.User = new User(json.Value<int>("user_id"), this.Client);
            this.Content = json.Value<string>("content");

             switch (json.Value<string>("post_type")) {
                case "link":
                    this.Type = PostType.Link; break;
                case "snap":
                    this.Type = PostType.Snap; break;
                case "code":
                    this.Type = PostType.Code; break;
                case "question":
                    this.Type = PostType.Question; break;
                default:
                    throw new Exception("Unkown post type!");
            }

            if (this.Type == PostType.Link)
                this.Link = new Uri(json.Value<string>("url"));

            if (this.Type == PostType.Snap) {
                this.SnapFileName = json.Value<string>("snap_file_name");
                this.SnapContentType = json.Value<string>("snap_content_type");
                this.SnapFileSize = json.Value<int>("snap_file_size");
                this.SnapUpdatedAt = DateTime.Parse(json.Value<string>("snap_updated_at"));
            }

            this.CreatedAt = DateTime.Parse(json.Value<string>("created_at"));
            this.UpdatedAt = DateTime.Parse(json.Value<string>("updated_at"));
            this.Description = json.Value<string>("description");
            this.ShortUriRedirects = json.Value<int>("short_url_redirects");
            this.Views = json.Value<int>("views");
            this.Title = json.Value<string>("title");
            this.IsPublic = json.Value<int>("is_public") == 1 ? true : false;
            var inReplyToId = json.Value<int?>("in_reply_to_post_id");
            this.InReplyTo = inReplyToId.HasValue ? new Post(inReplyToId.Value, this.Client) : null;
            this.FormattedContent = json.Value<string>("formatted_content");
            this.MarkdownContent = json.Value<string>("markdown_content");
            this.FormattedDescription = json.Value<string>("formatted_description");
            this.MarkdownDescription = json.Value<string>("markdown_description");
            this.CommentCount = json.Value<int>("comment_count");
            this.LikeCount = json.Value<int>("like_count");
            this.PostUri = new Uri(ForrstClient.BaseUri, json.Value<string>("post_url"));

            this.Tags = new List<Tag>();
            foreach (var tag in json["tag_objs"]) this.Tags.Add(new Tag(tag, this.Client));
        }

        protected override bool TryLoad() {
            return false;
        }
    }
}
