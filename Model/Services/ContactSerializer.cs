using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Model.Services
{
    public class ContactSerializer
    {
        private readonly string _filePath;

        public ContactSerializer()
        {
            string docsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _filePath = Path.Combine(docsFolder, "Contacts", "contacts.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
        }

        public void Save(ObservableCollection<Contact> contacts)
        {
            string json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public ObservableCollection<Contact> Load()
        {
            if (!File.Exists(_filePath)) return new ObservableCollection<Contact>();
            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<ObservableCollection<Contact>>(json) ?? new ObservableCollection<Contact>();
        }
    }
}