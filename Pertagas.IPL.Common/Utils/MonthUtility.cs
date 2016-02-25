using System.Collections.Generic;

namespace Pertagas.IPL.Common
{
    public class Month
    {
        private int _index;
        private string _name;
        private string _shortName;

        public Month(int index, string name, string shortName)
        {
            _index = index;
            _name = name;
            _shortName = shortName;
        }

        public int Index
        {
            get { return _index; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string ShortName
        {
            get { return _shortName; }
        }
    }    

    public static class MonthUtility
    {
        public static List<Month> GetMonths()
        {
            List<Month> months = new List<Month>();

            months.Add(new Month(1, "Januari", "Jan"));
            months.Add(new Month(2, "Februari", "Feb"));
            months.Add(new Month(3, "Maret", "Mar"));
            months.Add(new Month(4, "April", "Apr"));
            months.Add(new Month(5, "Mei", "Mei"));
            months.Add(new Month(6, "Juni", "Jun"));
            months.Add(new Month(7, "Juli", "Jul"));
            months.Add(new Month(8, "Agustus", "Aug"));
            months.Add(new Month(9, "September", "Sept"));
            months.Add(new Month(10, "Oktober", "Okt"));
            months.Add(new Month(11, "November", "Nov"));
            months.Add(new Month(12, "Desember", "Des"));


            return months;
        }
    }
}
