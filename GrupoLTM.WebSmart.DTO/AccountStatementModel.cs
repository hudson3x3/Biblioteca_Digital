using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.DTO
{
    public class AccountStatementModel
    {
        public string authorizationCode { get; set; }
        public int? accountHolderId { get; set; }
        public int? accountId { get; set; }
        public DateTime? insertDate { get; set; }
        public string description { get; set; }
        public int? externalCode { get; set; }
        public int? parentOrderId { get; set; }
        public decimal? value { get; set; }
        public decimal? cashValue { get; set; }
        public decimal? remainingValue { get; set; }
        public int? originAccountHolderId { get; set; }
        public string originAccountHolderName { get; set; }
        public int? transactionTypeId { get; set; }
        public DateTime? expirationDate { get; set; }
        public decimal? valueInPoints { get; set; }
        public decimal? remainingValueInPoints { get; set; }
        public bool isVirtualVoucher { get; set; }
        public bool hasExpirationChange { get; set; }
        public int? transactionCategory { get; set; }
    }
}
