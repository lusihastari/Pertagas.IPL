
namespace Pertagas.IPL.Domain
{
    public class ClusterDomain : DomainObject
    {
        private string _clusterName;

        public string ClusterName
        {
            get { return _clusterName; }
            set { _clusterName = value; }
        }
    }
}
