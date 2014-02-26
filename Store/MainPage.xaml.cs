using Shared;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Store
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Database _database;

        private async Task<Database> GetDatabase()
        {
            if (_database == null)
            {
                _database = new Database(new SQLitePlatformWinRT(), 
                    Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "todo.sqlite")));
                await _database.Initialize();
            }

            return _database;
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadToDoItems();
        }

        private async void LoadToDoItems()
        {
            var database = await GetDatabase();
            ItemsGrid.ItemsSource = await database.GetAllToDos();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ToDoItem));
        }

        private void ItemsGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(ToDoItem), ((ToDo)e.ClickedItem).Id);
        }
    }
}
