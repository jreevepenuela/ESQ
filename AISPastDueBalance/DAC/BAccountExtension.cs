using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISPastDueBalance
{
    public class BAccountExtension : PXCacheExtension<PX.Objects.CR.BAccount>
    {
        [PXDBDecimal]
        [PXUIField(DisplayName = "Total Past Due", Enabled = false)]
        public virtual Decimal? UsrTotalPastDue {  get; set; }
        public abstract class usrTotalPastDue : PX.Data.BQL.BqlDecimal.Field<usrTotalPastDue> { }

        [PXDBDecimal]
        [PXUIField(DisplayName = "Amount Outside Credit Verification", Enabled = false)]
        public virtual Decimal? UsrAmountOutsideCreditVerirification { get; set; }
        public abstract class usrAmountOutsideCreditVerification : PX.Data.BQL.BqlDecimal.Field <usrAmountOutsideCreditVerification> { }


    }
}
