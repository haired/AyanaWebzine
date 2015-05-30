using AyanaWebzine.Lib.Data;
using AyanaWebzine.Lib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using Windows.Storage;

namespace AyanaWebzine.WinPhone.ViewModels
{
    public class MainPageViewModel : AyanaWebzine.Lib.Common.BindableBase
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private bool isLoading = true;
        private string dataFilename = "categories";
        private DataStore dataStore = DataStore.GetInstance();
        
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                this.SetProperty(ref this.categories, value);
            }
        }                     

        public bool IsLoading
        {
            get{ return isLoading; }
            set
            {
                this.SetProperty(ref this.isLoading, value);
            }
        } 
        
        public async void LoadData(bool newLaunched)
        {
            this.IsLoading = true;
            this.Categories = dataStore.Categories;
            
            // after all get new (or not so new) data from internet
            await dataStore.GetData();
            CategoriesLoaded();
            this.IsLoading = false;
        }

        public async void ReloadArticles()
        {               
            this.IsLoading = true;
            await dataStore.GetData();
            CategoriesLoaded();
            this.IsLoading = false;  
        }
      
        private void CategoriesLoaded()
        {
            if (dataStore.Categories.Count() <= 0)
                //TODO : notify the user, connexion issues
                System.Diagnostics.Debug.WriteLine("Problemes de connexion");
            else
            {
                System.Diagnostics.Debug.WriteLine("Categories chargées");
                this.SaveArticlesToLocalStore(dataStore.Categories);  
            }
        }

        private async void SaveArticlesToLocalStore(IEnumerable<Category> data)
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            await localFolder.CreateFileAsync(dataFilename, CreationCollisionOption.ReplaceExisting);

            var file = await localFolder.GetFileAsync(dataFilename);
            if(file!= null)
            {                   
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Category>));
                using(Stream writer = await file.OpenStreamForWriteAsync())
                {
                    serializer.WriteObject(writer, data);
                    System.Diagnostics.Debug.WriteLine("Categories enregistrées"); 
                }
            }                   
            else
                System.Diagnostics.Debug.WriteLine("Impossible de créer le fichier de categories");
        }
    }
}

