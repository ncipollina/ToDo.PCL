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
    public sealed partial class ToDoItem : Page
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

        public ToDoItem()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadToDo(e.Parameter == null ? 0 : (int)e.Parameter);
        }

        private async void LoadToDo(int id = 0){
            if (id == 0)
            {
                DataContext = new ToDo();
            }
            else
            {
                var database = await GetDatabase();
                DataContext = await database.GetToDoById(id);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = DataContext as ToDo;
            item.TimeStamp = DateTime.UtcNow;
            var database = await GetDatabase();
            var result = 0;
            if (item.Id == 0)
            {
                result = await database.AddNewToDo(item);
            } else
            {
                result = await database.UpdateToDo(item);
            }
            if (result > 0)
            {
                ((Frame)Window.Current.Content).GoBack();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).GoBack();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var item = DataContext as ToDo;

            if (item.Id > 0){
                var database = await GetDatabase();
                await database.DeleteToDo(item);
            }
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
