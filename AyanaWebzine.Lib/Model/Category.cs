using AyanaWebzine.Lib.Model;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AyanaWebzine.Lib.Model
{
    public class Category : AyanaWebzine.Lib.Common.BindableBase
    {
        public Guid ID { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set { this.SetProperty(ref this.name, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { this.SetProperty(ref this.description, value); }
        }

        private Uri thumbnail;
        public Uri Thumbnail
        {
            get { return thumbnail; }
            set { this.SetProperty(ref this.thumbnail, value); }
        }

        private byte[] thumbnailBytes;
        public byte[] ThumbnailBytes
        {
            get { return thumbnailBytes; }
            set { thumbnailBytes = value; }
        }

        private ObservableCollection<Article> articles = new ObservableCollection<Article>();
        public ObservableCollection<Article> Articles
        {
            get { return articles; }
            set { this.SetProperty(ref this.articles, value); }  
        }

        private Uri url;
        public Uri Url
        {
            get { return url; }
            set { this.SetProperty(ref this.url, value); }
        }
        public Category()
        {
            this.ID = Guid.NewGuid();
            //this.Articles = new ObservableCollection<Article>();
        }

        public Category(string name, Uri url)
        {
            this.ID = Guid.NewGuid();
            this.Name = name;
            this.Url = url;
            //this.Articles = new ObservableCollection<Article>();
        }

        public static ObservableCollection<Category> _categories = new ObservableCollection<Category>();

        public static EventHandler<Category> CategoryLoadCompleted;

        public static void OnCategoryLoaded(Category category)
        {
            if (category != null)
                if (CategoryLoadCompleted != null)
                    CategoryLoadCompleted(category, category);
        }       

        public static ObservableCollection<Category> CreateCategories()
        {
            #region Create Categories
            var categories = new ObservableCollection<Category>();
            try
            {
                // Maincategory mode
                Uri modeFeed = new Uri("http://ayanawebzine.com/category/mode/feed", UriKind.RelativeOrAbsolute);
                Uri beauteFeed = new Uri("http://ayanawebzine.com/category/beaute/feed", UriKind.RelativeOrAbsolute);
                Uri coupleSexoFeed = new Uri("http://ayanawebzine.com/category/couple-sexo/feed", UriKind.RelativeOrAbsolute);
                Uri bienEtreFeed = new Uri("http://ayanawebzine.com/category/bien-etre-sante/feed", UriKind.RelativeOrAbsolute);

                Uri lifeStyleFeed = new Uri("http://ayanawebzine.com/category/lifestyle/feed", UriKind.RelativeOrAbsolute);
                Uri businessFeed = new Uri("http://ayanawebzine.com/category/corporate/feed", UriKind.RelativeOrAbsolute);
                Uri cultureFeed = new Uri("http://ayanawebzine.com/category/culture/feed", UriKind.RelativeOrAbsolute);
                Uri editoFeed = new Uri("http://ayanawebzine.com/category/edito/feed", UriKind.RelativeOrAbsolute);

                //Uri styleFeed = new Uri("http://www.ayanawebzine.com/mode-a-beaute/abidjan-street-style.html?format=feed&type=rss", UriKind.RelativeOrAbsolute);

                Category mode = new Category();
                mode.Name = "Mode";
                mode.Url = modeFeed;
                //mode.FlowerColor = FlowerColor.Blue;

                Category beaute = new Category();
                beaute.Name = "Beauté";
                beaute.Url = beauteFeed;


                Category coupleSexo = new Category();
                coupleSexo.Name = "Couple/Sexo";
                coupleSexo.Url = coupleSexoFeed;


                Category bienEtre = new Category();
                bienEtre.Name = "Bien être/Santé";
                bienEtre.Url = bienEtreFeed;


                Category lifeStyle = new Category();
                lifeStyle.Name = "Lifestyle";
                lifeStyle.Url = lifeStyleFeed;


                Category business = new Category();
                business.Name = "Corporate";
                business.Url = businessFeed;


                Category culture = new Category();
                culture.Name = "Culture";
                culture.Url = cultureFeed;

                Category edito = new Category();
                edito.Name = "Edito";
                edito.Url = editoFeed;

                categories.Add(mode);
                categories.Add(beaute);
                categories.Add(coupleSexo);
                categories.Add(bienEtre);
                categories.Add(lifeStyle);
                categories.Add(business);
                categories.Add(culture);
                categories.Add(edito);

                return categories;
            #endregion
            }
            catch (Exception)
            {
                return categories;
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

    }

}
