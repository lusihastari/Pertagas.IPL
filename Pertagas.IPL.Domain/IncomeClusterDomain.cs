using System;

namespace Pertagas.IPL.Domain
{
    public class IncomeClusterDomain : DomainObject
    {
        private int _clusterId;
        private string _occupantName;
        private string _addressBlock;
        private string _addressNumber;
        private string _phoneNumber;
        private int _month;
        private int _year;
        private double _amount;

        private string _displayedAmount;
        private string _clusterName;
        private string _displayedMonth;

        public int ClusterId
        {
            get { return _clusterId; }
            set { _clusterId = value; }
        }

        public string OccupantName
        {
            get { return _occupantName; }
            set { _occupantName = value; }
        }

        public string AddressBlock
        {
            get { return _addressBlock; }
            set { _addressBlock = value; }
        }

        public string AddressNumber
        {
            get { return _addressNumber; }
            set { _addressNumber = value; }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public string DisplayedAmount
        {
            get { return _displayedAmount; }
            set { _displayedAmount = value; }
        }

        public string ClusterName
        {
            get { return _clusterName; }
            set { _clusterName = value; }
        }

        public string DisplayedMonth
        {
            get { return _displayedMonth; }
            set { _displayedMonth = value; }
        }
    }
}
