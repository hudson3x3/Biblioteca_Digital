
using System;

namespace GrupoLTM.WebSmart.Services.Model
{
    public class Account
    {
        public long AccountHolderId { get; set; }
        public long? PriorityAccountHolderId { get; set; }
        public int AccountType { get; set; }
        public bool IsLocked { get; set; }
        public int Status { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Balance { get; set; }
        public decimal CurrencyBalance { get; set; }
        public bool IsExtractUpDated { get; set; }

    }
}
