using System.ComponentModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Model
{
    // Наследуемся от ObservableObject из CommunityToolkit
    public class Contact : ObservableObject, IDataErrorInfo
    {
        private string _name;
        private string _phoneNumber;
        private string _email;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value); // Новый метод уведомления
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public Contact()
        {
            Name = "";
            PhoneNumber = "";
            Email = "";
        }

        public Contact Clone() => new Contact { Name = this.Name, PhoneNumber = this.PhoneNumber, Email = this.Email };

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (Name != null && Name.Length > 100) error = "Имя не может быть длиннее 100 символов.";
                        break;
                    case nameof(PhoneNumber):
                        if (PhoneNumber != null && PhoneNumber.Length > 100) error = "Телефон не может быть длиннее 100 символов.";
                        else if (!string.IsNullOrEmpty(PhoneNumber) && !Regex.IsMatch(PhoneNumber, @"^[0-9\+\-\(\)\s]+$"))
                            error = "Только цифры и символы +-()";
                        break;
                    case nameof(Email):
                        if (Email != null && Email.Length > 100) error = "Email не может быть длиннее 100 символов.";
                        else if (!string.IsNullOrEmpty(Email) && !Email.Contains("@"))
                            error = "Email должен содержать символ @.";
                        break;
                }
                return error;
            }
        }

        public bool HasErrors => !string.IsNullOrEmpty(this[nameof(Name)]) ||
                                 !string.IsNullOrEmpty(this[nameof(PhoneNumber)]) ||
                                 !string.IsNullOrEmpty(this[nameof(Email)]);
    }
}