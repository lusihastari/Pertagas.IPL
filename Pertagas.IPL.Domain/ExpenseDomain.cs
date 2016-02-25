
namespace Pertagas.IPL.Domain
{
    public class ExpenseDomain : DomainObject
    {
        private string _description;
        private int _month;
        private int _year;
        private double _amount;

        private string _displayedMonth;
        private string _displayedAmount;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
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

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public string DisplayedAmount
        {
            get { return _displayedAmount; }
            set { _displayedAmount = value; }
        }

        public string DisplayedMonth
        {
            get { return _displayedMonth; }
            set { _displayedMonth = value; }
        }
    }
}
