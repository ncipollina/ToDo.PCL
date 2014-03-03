using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Shared;
using System.Threading.Tasks;
using SQLite.Net.Platform.WindowsPhone8;
using System.IO;
using Windows.Storage;

namespace Phone
{
    public partial class ToDoItem : PhoneApplicationPage
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

        public ToDoItem()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string serializedParameter;
            NavigationContext.QueryString.TryGetValue("parameter", out serializedParameter);
            LoadToDo(string.IsNullOrEmpty(serializedParameter) ? 0 : Convert.ToInt32(serializedParameter));
        }

        private async void LoadToDo(int id = 0)
        {
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

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            var item = DataContext as ToDo;

            if (item.Id > 0)
            {
                var database = await GetDatabase();
                await database.DeleteToDo(item);
            }
            App.RootFrame.GoBack();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            App.RootFrame.GoBack();
        }
       
        private async void SaveButton_Click(object sender, EventArgs e)
        {
            var binding = Text.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
            {
                binding.UpdateSource();
            }

            var item = DataContext as ToDo;
            item.TimeStamp = DateTime.UtcNow;
            var database = await GetDatabase();
            var result = 0;
            if (item.Id == 0)
            {
                result = await database.AddNewToDo(item);
            }
            else
            {
                result = await database.UpdateToDo(item);
            }
            if (result > 0)
            {
                App.RootFrame.GoBack();
            }
        }
    }
}