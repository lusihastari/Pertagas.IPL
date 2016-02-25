
namespace Pertagas.IPL.Domain
{
    public class IncomeDomain : DomainObject
    {
        private int _incomeSourceId;
        private double _amount;
        private int _month;
        private int _year;

        private string _displayedAmount;
        private string _incomeSourceDescription;
        private string _displayedMonth;

        public int IncomeSourceId
        {
            get { return _incomeSourceId; }
            set { _incomeSourceId = value; }
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

        public string IncomeSourceDescription
        {
            get { return _incomeSourceDescription; }
            set { _incomeSourceDescription = value; }
        }

        public string DisplayedMonth
        {
            get { return _displayedMonth; }
            set { _displayedMonth = value; }
        }
    }
}
