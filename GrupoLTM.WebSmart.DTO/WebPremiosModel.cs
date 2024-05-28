using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    #region "Model Post"

    public class WebPremiosModelParticipant
    {
        public bool Acitve { get; set; }
        public int LogonTypeId { get; set; }
        public int PersonType { get; set; }
        public DateTime BirthDate { get; set; }
        public int ClientId { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string IE { get; set; }
        public string IM { get; set; }
        public DateTime InsertDate { get; set; }
        public string Login { get; set; }
        public int MaritalStatus { get; set; }
        public string Name { get; set; }
        public string RG { get; set; }
        public int Gender { get; set; }
        public int ParticipantStatusId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool TemporaryPassword { get; set; }
        public DateTime DateFirstAccess { get; set; }
        public int ProjectId { get; set; }
        public int ProjectConfigurationId { get; set; }
        public int ProfileId { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Email> Emails { get; set; }
        public List<Phone> Telephones { get; set; }
    }
    public class Address
    {
        public bool Active { get; set; }
        public int AddressType { get; set; }
        public string City { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public DateTime InsertDate { get; set; }
        public int Status { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public State State { get; set; }
    }

    public class State
    {
        public string UF {get; set;}
        public string Name { get; set; }
    }

    public class Email
    {
        public bool Active { get; set; }
        public string EmailAddress { get; set; }
        public int EmailTypeId { get; set; }
        public DateTime InsertDate { get; set; }
    }

    public class Phone
    {
        public bool Active { get; set; }
        public string Ddd { get; set; }
        public string Number { get; set; }
        public string PhoneTypeId { get; set; }
    }

    #endregion

    #region "Model Result"

    public class WebPremiosModelResultToken
    {
        public string access_token { get; set; }
        public int statusCode { get; set; }
    }

    public class WebPremiosModelResultAuthenticate
    {
        //POST api/Participant/Authenticate
        public string Success { get; set; }
        public string UserNotFound { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public string IdPerfil { get; set; }
        public string Login { get; set; }
        public string Session { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string TokenExpires { get; set; }
        public string LastLogin { get; set; }
        public string IdParticipant { get; set; }
        public string ProjectConfigurationId { get; set; }
        public string IsActiveProject { get; set; }
        public string AccountHolderId { get; set; }
        public string ErrorId { get; set; }
        public string StatusId { get; set; }
    }

    public class WebPremiosModelResultGetBalance
    {
        //GET api/Account/GetBalance?accountHolderId={accountHolderId}
        public string AccountId { get; set; }
        public string PriorityAccountHolderId { get; set; }
        public string IsLocked { get; set; }
        public string AccountType { get; set; }
        public string Balance { get; set; }
        public string PointsRedeemed { get; set; }
        public string PointsCredited { get; set; }
        public string CurrencyBalance { get; set; }
        public string ConversionRate { get; set; }
        public string RenewedDate { get; set; }
    }

    public class WebPremiosModelResultShowCase {
        public string DidYouMean { get; set; }
        public List<WebPremiosModelResultProductShowCase> Products { get; set; }
    }

    public class WebPremiosModelResultProductShowCase
    {
        //GET api/Search/GetProductsShowcase?projectConfigurationId={projectConfigurationId}&ordenationBy={ordenationBy}&productByPage={productByPage}&firstproduct={firstproduct}&identifier={identifier}
        public string ProductId { get; set; }
        public string ProductSearchId { get; set; }
        public string ProductLiveId { get; set; }
        public string SkuProductId { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
        public string SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public string SuplierId { get; set; }
        public string PartnerId { get; set; }
        public string Partner { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string BigImageUrl { get; set; }
        public string Discount { get; set; }
        public string Price { get; set; }
        public string NetPrice { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
    }

    #endregion


}
