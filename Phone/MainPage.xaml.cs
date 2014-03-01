using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Phone.Resources;
using Shared;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using SQLite.Net.Platform.WindowsPhone8;

namespace Phone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Database _database;

        private async Task<Database> GetDatabase()
        {
            if (_database == null)
            {
                _database = new Database(new SQLitePlatformWP8(),
                    Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "todo.sqlite")));
                await _database.Initialize();
            }

            return _database;
        }
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadToDoItems();
        }

        private async void LoadToDoItems()
        {
            var database = await GetDatabase();
            ToDoList.ItemsSource = await database.GetAllToDos();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}