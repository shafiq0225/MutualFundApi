using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nav.Domain.Entities
{
    public class Fund
    {
        public string Id { get; set; }
        public string FundId { get; set; }
        public string Name { get; set; }
        public bool FundVisible { get; set; }
        public string SchemeId { get; set; }
        public string SchemeName { get; set; }
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
        public bool SchemeVisible { get; set; }
    }
}
