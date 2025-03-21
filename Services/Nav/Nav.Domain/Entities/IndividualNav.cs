using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Domain.Entities
{
    public class IndividualNav
    {
        public string? FundId { get; set; }
        public string? FundName { get; set; }
        public string? SchemeId { get; set; }
        public string? SchemeName { get; set; }
        public List<SchemeHistory>? SchemeHistory { get; set; }
    }
}
