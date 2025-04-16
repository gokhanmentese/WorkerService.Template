using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService.Template.Enumarations
{
    internal class Enums
    {
    }

    public enum ApprovalStatus
    {
        Onay = 0,
        Red = 1
    }

    public enum PurchaseDetailSupplyStatus
    {
        Etkin = 1,
        TedarikEdilemiyor = 100000000,
        ExceleAlindi = 100000001,
        Onaylandi = 100000002,
        RedEdildi = 100000003
    }
}
