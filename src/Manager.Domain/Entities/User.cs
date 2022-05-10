using Manager.Domain.Validators;

namespace Manager.Domain.Entities
{
    public class User : Base
    {
        public string? Name { get; private set; }
        public string? Password { get; private set; }
        public string? Email { get; private set; }

        //EF
        protected User() { }

        public User(string name, string password, string email)
        {
            Name = name;
            Password = password;
            Email = email;
            _errors = new List<string>();
        }

        public void ChangeName(string name)
        {
            Name = name;
            Validade();
        }

        public void ChangePassword(string password)
        {
            Password = password;
            Validade();
        }
        public void ChangeEmail(string email)
        {
            Email = email;
            Validade();
        }



        public override bool Validade()
        {
            var validator = new UserValidator();
            var validation = validator.Validate(this);

            if (!validation.IsValid)
            {
                foreach (var item in validation.Errors)
                    _errors.Add(item.ErrorMessage);

                throw new Exception("Alguns campos estão inválidos, por favor verifique!" + _errors[0]);
            }
            return true;
        }
    }
}
