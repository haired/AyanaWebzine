using AyanaWebzine.Lib.Data;
using AyanaWebzine.Lib.Model;
using System.Collections.ObjectModel;

namespace AyanaWebzine.WinPhone.ViewModels
{
    public class CategoriesPageViewModel : AyanaWebzine.Lib.Common.BindableBase
    {
        public static CategoriesPageViewModel _ctegoriesPageViewModel;
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private bool isLoading = true;  

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
            get
            {
                return this.isLoading;
            }
            set
            {
                this.SetProperty(ref this.isLoading, value);
            }
        }

        public CategoriesPageViewModel(bool isNew)
        {                 
            var dataStore = DataStore.GetInstance();
            this.Categories = dataStore.Categories;          
        }                  
    }
}

