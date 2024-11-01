using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.CR;
using System.Linq;

namespace AISDisputeButton
{
    public class SOInvoiceEntryExtension : PXGraphExtension<PX.Objects.SO.SOInvoiceEntry>
    {

        public SelectFrom<CRRelation>.View RelationView;

        public PXAction<ARInvoice> Dispute;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Dispute", Visible = false)]
        protected void dispute()
        {
            ARInvoice row = Base.Document.Current;

            if (row == null) return;

            var graph = PXGraph.CreateInstance<CRCaseMaint>();

            var crRelationList = PXSelect<CRRelation>.Select(this.Base).ToList();

            if (crRelationList.Count > 0)
            {
                var crRelation = crRelationList.RowCast<CRRelation>().Where(x => x.RefNoteID == row.NoteID).FirstOrDefault();

                if (crRelation != null)
                {


                    var crCaseList = PXSelect<CRCase>.Select(this.Base).ToList();

                    if (crCaseList.Count > 0)
                    {
                        var crCase = crCaseList.RowCast<CRCase>().Where(x => x.NoteID == crRelation.TargetNoteID).FirstOrDefault();

                        if (crCase == null) return;


                        try
                        {
                            graph.Case.Update(crCase);

                            graph.Views["Relations"].Cache.Update(crRelation);
                        }
                        catch { }

                        if (!graph.IsContractBasedAPI)
                            PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.NewWindow);

                        graph.Save.Press();


                    }


                }
                else
                {
                    if (!graph.IsContractBasedAPI)
                        PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.NewWindow);
                }
            }
           

        }

        protected void ARInvoice_RowSelected(PXCache cache, PXRowSelectedEventArgs e)
        {
            var row = (ARInvoice)e.Row;

            if (row == null) return;

            if (row.Status.Equals("N"))
                Dispute.SetVisible(true);
        }
    }
}
