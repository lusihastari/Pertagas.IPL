
namespace Pertagas.IPL.Domain
{
    public class AddressBlockDomain : DomainObject
    {
        private string _block;

        public string Block
        {
            get { return _block; }
            set { _block = value; }
        }
    }
}
