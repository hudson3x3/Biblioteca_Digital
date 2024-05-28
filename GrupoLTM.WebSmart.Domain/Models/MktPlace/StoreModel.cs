using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public class StoreModel
    {
        public long Id { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public long SupplierId { get; set; }
        public string Description { get; set; }
        public string ProjectConfigurationId { get; set; }
        public string SupplierTypeId { get; set; }
        public bool IsExternal { get; set; }
        public int? Priority { get; set; }
        public int ShowButton { get; set; }
    }
}
