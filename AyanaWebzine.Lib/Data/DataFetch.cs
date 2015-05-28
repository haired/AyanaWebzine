using AyanaWebzine.Lib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AyanaWebzine.Lib.Data
{
    public class DataFetch
    {

        public ICollection<Category> Categories { get; set; }

        private ContentCreator contentCreator = new ContentCreator();

        public DataFetch()
        {
            // Create categories
            Dictionary<string, Uri> categoriesFeed = new Dictionary<string, Uri>();
            string uriRoot = "http://ayanawebzine.com/category/";
            
            this.Categories = new List<Category>();
            this.Categories.Add(new Category("Mode", new Uri(uriRoot + "/mode/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Beauté", new Uri(uriRoot + "/beaute/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Couple/Sexo", new Uri(uriRoot + "/couple-sexo/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Bien être/Santé", new Uri(uriRoot + "/bien-etre-sante/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Lifestyle", new Uri(uriRoot + "/lifestyle/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Corporate", new Uri(uriRoot + "/corporate/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Culture", new Uri(uriRoot + "/culture/feed", UriKind.RelativeOrAbsolute)));
            this.Categories.Add(new Category("Edito", new Uri(uriRoot + "/edito/feed", UriKind.RelativeOrAbsolute)));          
        }

        public async Task<ICollection<Category>> DownloadCategories(ICollection<Category> dataStore)
        {               
            dataStore = this.Categories;

            var categoriesData = await DownloadData();

            if (categoriesData.Count() == 0)
                return new List<Category>();

            foreach (var categoryData in categoriesData)
            {
                var category = this.Categories.FirstOrDefault(c => c.ID == categoryData.Key);
                if(category != null)
                {
                    XElement itemsRoot =  categoryData.Value;
                    var feedItems = itemsRoot.Descendants().Where(n => n.Name.LocalName.Equals("item"));

                    var articles = ContentCreator.CreateArticles(feedItems); 
                    category.Articles = new ObservableCollection<Article>(articles);     
                }
            }

            return this.Categories;
        }

        private async Task<Dictionary<Guid, XElement>> DownloadData()
        {                                
            Dictionary<Guid, XElement> categoriesFeed = new Dictionary<Guid, XElement>();
            HttpClient client = new HttpClient();

            foreach (var category in this.Categories)
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(category.Url);
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                if (response == null || response.IsSuccessStatusCode == false)
                {
                    // TODO : notify the admin that this feed is empty
                    System.Diagnostics.Debug.WriteLine("Probleme de connexion aux flux");
                    return new Dictionary<Guid, XElement>();
                    //throw new HttpRequestException();
                }
                    // continue;

                string xmlFeed = await response.Content.ReadAsStringAsync();
                categoriesFeed.Add(category.ID, XElement.Parse(xmlFeed));
            }
            return categoriesFeed;
        }          
    }
}
