﻿
namespace Pertagas.IPL.Domain
{
    public class IncomeSourceDomain : DomainObject
    {
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
