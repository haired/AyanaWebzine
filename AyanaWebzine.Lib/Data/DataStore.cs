using AyanaWebzine.Lib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AyanaWebzine.Lib.Data
{
    public class DataStore
    {
        public ObservableCollection<Category> Categories { get; set; }
        private static DataStore _dataStore;
        private DataFetch fetcher;

        public bool IsLoading { get; set; }

        public static DataStore GetInstance()
        {
            if(_dataStore == null)
                _dataStore = new DataStore();

            return _dataStore;
        }

        private DataStore()
        {
            fetcher = new DataFetch();
            Categories = new ObservableCollection<Category>(fetcher.Categories);
        }

        public async Task GetData()
        {
            this.IsLoading = true;
            await fetcher.DownloadCategories(this.Categories);
            this.IsLoading = false;
        }

        public Article GetArticle(string id)
        {
            Guid argsGuid = new Guid(id);
            foreach(var category in this.Categories)
            {
                Article article = category.Articles.FirstOrDefault(a => a.Id == argsGuid);
                if (article != null)
                    return article;
            }

            return null;            
        }
    }
}
