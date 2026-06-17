using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Model;
using Model.Services;

namespace ViewModel
{
    public class MainVM : ObservableObject
    {
        private readonly ContactSerializer _serializer = new ContactSerializer();
        private ObservableCollection<Contact> _contacts;
        private Contact _selectedContact;
        private Contact _editableContact;
        private bool _isReadOnly = true;
        private bool _isEditing = false;//

        public ObservableCollection<Contact> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (SetProperty(ref _selectedContact, value))
                {
                    if (IsEditing)
                    {
                        IsEditing = false;
                        IsReadOnly = true;
                    }
                    EditableContact = value != null ? value.Clone() : new Contact();
                    UpdateCommands();
                }
            }
        }

        public Contact EditableContact
        {
            get => _editableContact;
            set
            {
                var oldContact = _editableContact;
                if (SetProperty(ref _editableContact, value))
                {
                    if (oldContact != null) oldContact.PropertyChanged -= EditableContact_PropertyChanged;
                    if (_editableContact != null) _editableContact.PropertyChanged += EditableContact_PropertyChanged;
                    UpdateCommands();
                }
            }
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetProperty(ref _isReadOnly, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (SetProperty(ref _isEditing, value))
                {
                    OnPropertyChanged(nameof(IsNotEditing));
                    UpdateCommands();
                }
            }
        }

        public bool IsNotEditing => !IsEditing;

        public IRelayCommand AddCommand { get; }
        public IRelayCommand EditCommand { get; }
        public IRelayCommand RemoveCommand { get; }
        public IRelayCommand ApplyCommand { get; }

        public MainVM()
        {
            Contacts = _serializer.Load();
            EditableContact = new Contact();

            // Используем RelayCommand из библиотеки Microsoft
            AddCommand = new RelayCommand(Add, () => IsNotEditing);
            EditCommand = new RelayCommand(Edit, () => SelectedContact != null && IsNotEditing);
            RemoveCommand = new RelayCommand(Remove, () => SelectedContact != null && IsNotEditing);
            ApplyCommand = new RelayCommand(Apply, () => IsEditing && !EditableContact.HasErrors);
        }

        private void EditableContact_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ApplyCommand.NotifyCanExecuteChanged();
        }

        private void UpdateCommands()
        {
            AddCommand?.NotifyCanExecuteChanged();
            EditCommand?.NotifyCanExecuteChanged();
            RemoveCommand?.NotifyCanExecuteChanged();
            ApplyCommand?.NotifyCanExecuteChanged();
        }

        private void Add()
        {
            SelectedContact = null;
            EditableContact = new Contact();
            IsReadOnly = false;
            IsEditing = true;
        }

        private void Edit()
        {
            if (SelectedContact == null) return;
            IsReadOnly = false;
            IsEditing = true;
        }

        private void Remove()
        {
            if (SelectedContact == null) return;

            int index = Contacts.IndexOf(SelectedContact);
            Contacts.Remove(SelectedContact);

            if (Contacts.Count == 0) SelectedContact = null;
            else if (index >= Contacts.Count) SelectedContact = Contacts[Contacts.Count - 1];
            else SelectedContact = Contacts[index];

            _serializer.Save(Contacts);
        }

        private void Apply()
        {
            if (SelectedContact == null)
            {
                Contacts.Add(EditableContact);
                SelectedContact = EditableContact;
            }
            else
            {
                int index = Contacts.IndexOf(SelectedContact);
                Contacts[index] = EditableContact;
                SelectedContact = EditableContact;
            }

            IsReadOnly = true;
            IsEditing = false;
            _serializer.Save(Contacts);
        }
    }
}