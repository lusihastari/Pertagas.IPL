
namespace Pertagas.IPL.Domain
{
    public class UserDomain : DomainObject
    {
        private string _username;
        private string _password;
        private string _firstname;
        private string _lastname;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
    }
}
