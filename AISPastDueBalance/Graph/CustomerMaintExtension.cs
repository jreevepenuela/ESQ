using AISPastDueBalance;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CR;
using System;
using System.Linq;

namespace AISDisputeButton
{
    public class CustomerMaintExtension : PXGraphExtension<PX.Objects.AR.CustomerMaint>
    {

        protected void Customer_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {

            var row = (Customer)e.Row;

            if (row == null) return;


            decimal? totalAmount = 0;
            decimal? totalCreditDaysPastDueAmount = 0;

            var customerInvoice = PXSelect<ARInvoice, Where<ARInvoice.customerID, Equal<Required<ARInvoice.customerID>>,
                And<ARInvoice.status, Equal<Required<ARInvoice.status>>>>>.Select(this.Base, row.BAccountID, "N").ToList();

            if(customerInvoice.Count > 0)
            {
                var customerBalance = customerInvoice.RowCast<ARInvoice>().Where(x => Base.Accessinfo.BusinessDate.Value > x.DueDate.Value).Sum(x => x.CuryDocBal);

                totalAmount = customerBalance;

                cache.SetValueExt<BAccountExtension.usrTotalPastDue>(row, totalAmount);

                var customerDueDate = customerInvoice.RowCast<ARInvoice>().FirstOrDefault();

                if (customerDueDate == null) return;

                TimeSpan pastDueDate = Base.Accessinfo.BusinessDate.Value - customerDueDate.DueDate.Value;

                if(row.CreditDaysPastDue > pastDueDate.Days)
                {

                }
            }


        }

    }
}
