using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMIT2018Common.UserControls
{
    [Serializable]
    public class BusinessRuleException : Exception
    {
        public List<string> RuleDetails { get; set; }
        public BusinessRuleException(string message, List<string> reasons)
            : base(message)
        {
            this.RuleDetails = reasons;
        }
    }
}
