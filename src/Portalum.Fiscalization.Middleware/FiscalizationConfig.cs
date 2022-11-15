using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portalum.Fiscalization.Middleware
{
    public class FiscalizationConfig
    {
        public string StroreId { get; set; }
        public string CashRegisterTerminalNumber { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string VatIdentificationNumber { get; set; }
    }
}
