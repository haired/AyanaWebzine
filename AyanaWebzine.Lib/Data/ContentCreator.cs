using AyanaWebzine.Lib.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace AyanaWebzine.Lib.Data                                                                                  
{
    public class ContentCreator
    {                                                               
        public static IEnumerable<Article> CreateArticles(IEnumerable<XElement> items)
        {
            List<Article> articles = new List<Article>();

            foreach (var item in items)
            {
                if (item != null)
                {
                    try
                    {
                        var title = item.Elements().FirstOrDefault(i => i.Name.LocalName.Equals("title")).Value;
                        var PublishedDate = item.Elements().FirstOrDefault(i => i.Name.LocalName.Equals("pubDate")).Value;
                        var articleContent = item.Elements().FirstOrDefault(i => i.Name.LocalName.Equals("encoded")).Value;
                        var intro = item.Elements().FirstOrDefault(i => i.Name.LocalName.Equals("description")).Value;
                        var url = item.Elements().FirstOrDefault(i => i.Name.LocalName.Equals("link")).Value;

                        Article article = new Article()
                        {
                            Title = title,
                            Content = WebUtility.HtmlDecode(articleContent),
                            Date = DateTime.Parse(PublishedDate).Date.ToString("d MMM yyyy"),
                            Url = new Uri(url)
                        };

                        // Retreive picture from articles                            
                        //article.CompositeContent = await CreateCompositeContent(article.Content); ;
                        //CompositeContent FirstPicture = GetFirstPicture(article.CompositeContent);
                        IEnumerable<CompositeContent> introduction = CreateCompositeContent(intro);
                        article.ThumbnailUri = introduction.FirstOrDefault(i => i.Type == ContentType.Image).Content;
                        article.Introduction = WebUtility.HtmlDecode(introduction.FirstOrDefault(i => i.Type == ContentType.Text && !string.IsNullOrWhiteSpace(i.Content)).Content);
                            
                        //article.ThumbnailUri = GetFirstPicture(articleContent);


                        articles.Add(article);

                        // If the article is not in the category.articles, then it's new
                        if (articles.Any(a => a.Url.Equals(article.Url)) == false)
                        {
                            articles.Add(article);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        // TODO: notify to admin an error in feeds
                    }
                }
            }

            return articles;
        }    

        public static ObservableCollection<CompositeContent> CreateCompositeContent(string content)
        {
            ObservableCollection<CompositeContent> compositeContent = new ObservableCollection<CompositeContent>();
            try
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(content);
                //var descendants = document.DocumentNode.Descendants().Where(d => d.Name.Equals("p") || d.Name.StartsWith("h") || d.Name.Equals("img")).ToList();
                var descendants = document.DocumentNode.Descendants().Where(d => d.Name.Equals("#text") || d.Name.Equals("img")).ToList();
                foreach (HtmlNode node in descendants)
                {
                    if (node.Name.Equals("img"))
                    {
                        string pictureLink = string.Empty;
                        if (node.Attributes.Any(a => a.Name.Equals("src")))
                            pictureLink = node.Attributes.FirstOrDefault(a => a.Name.Equals("src")).Value;
                        else if(node.Attributes.Any(a => a.Name.Equals("data-orig-file")))
                            pictureLink = node.Attributes.FirstOrDefault(a => a.Name.Equals("data-orig-file")).Value;
   
                        RemoveComment(node);                        

                        // Download picture from Uri
                        if (!string.IsNullOrEmpty(pictureLink))
                        {
                            CompositeContent image = new CompositeContent() { Type = ContentType.Image, Content = pictureLink };
                            compositeContent.Add(image);
                        }
                    }
                    else 
                    {
                        if(!string.IsNullOrWhiteSpace(node.InnerText) && node.InnerText != "\n")
                        {   
                            RemoveComment(node);
                            CompositeContent texte = new CompositeContent() { Type = ContentType.Text, Content = node.InnerText };
                            compositeContent.Add(texte);
                        }
                    }
                }

                return compositeContent;
            }
            catch (Exception)
            {
                return compositeContent;
            }
        }

        private static void RemoveComment(HtmlNode node)
        {
            foreach (var n in node.ChildNodes)
            {
                if (n.NodeType == HtmlNodeType.Comment)
                    n.Remove();
                else
                    RemoveComment(n);
            }
        }
                                  
        private static string GetFirstPicture(string htmlContent)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            // Remove all comments
            RemoveComment(document.DocumentNode);

            var imageNode = document.DocumentNode.Descendants().FirstOrDefault(n => n.Name.Equals("img"));
            string pictureLink = null;
            try
            {
                if (imageNode != null)
                {
                    try
                    {
                        pictureLink = imageNode.Attributes["src"].Value;
                    }
                    catch (Exception)
                    {
                        pictureLink = imageNode.Attributes["data-orig-file"].Value;
                    }
                }
            }
            catch (Exception)
            {
                pictureLink = null;
            }

            return pictureLink;
        }      
    }
}
