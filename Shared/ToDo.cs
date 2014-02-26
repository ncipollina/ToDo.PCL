using SQLite.Net.Attributes;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared
{
    public class ToDo : INotifyPropertyChanged
    {
        private int _id;
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        public DateTime TimeStamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    
        protected void SetProperty<T>(ref T property, T data, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, data))
            {
                return;
            }

            property = data;
            FirePropertyChanged(propertyName);
        }

        private void FirePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
