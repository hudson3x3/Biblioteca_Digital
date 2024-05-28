using System;
using System.Collections.Generic;
using GrupoLTM.WebSmart.Domain.Enums;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public class Participant
    {

        public Participant()
        {
            this.LogonTypeId = EnumMktPlace.LogonType.CPF;
            this.ParticipantStatusId = EnumMktPlace.ParticipantStatus.Active;
            this.PersonTypeId = EnumMktPlace.PersonType.Individual;
            this.MaritalStatusId = EnumMktPlace.MaritalStatusModelType.NaoInformado; //TODO: Mudar para Não informado (não esta funcionando o status 6 no MltPlace, por isso esta fixo Solteiro)

            //this.Accepts = new List<AcceptModel>();
            //this.Accepts.Add(new AcceptModel() { Checked = false });
        }

        public long ParticipantId { get; set; }
        public long ClientId { get; set; }
        public long ProfileId { get; set; }
        public EnumMktPlace.ParticipantStatus ParticipantStatusId { get; internal set; }
        public EnumMktPlace.LogonType LogonTypeId { get; internal set; }
        public long CatalogId { get; set; }
        public string Username { get; set; }
        public long CampaignId { get; set; }
        public EnumMktPlace.MaritalStatusModelType MaritalStatusId { get; set; }
        public string Name { get; set; }
        public EnumMktPlace.PersonType PersonTypeId { get; internal set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Password { get; set; }
        public EnumMktPlace.GenderType Gender { get; set; }
        public Nullable<DateTime> BirthDate { get; set; }
        public IList<AddressModel> Address { get; set; }
        public IList<PhoneModel> Phones { get; set; }
        //public IList<AcceptModel> Accepts { get; set; }
        public IList<EmailModel> Emails { get; set; }

        //Estrutura WebSmart
        public string Estrutura { get; set; }
        public bool OptInEmail { get; set; }
        public bool OptInComunicacaoFisica { get; set; }
        public bool OptInSms { get; set; }
        public bool OptinAceite { get; set; }
        public bool ViewSimulator { get; set; }
        public string ConfirmEmail { get; set; }
        public string ProfileTypeId { get; set; }
        public bool HasInvalidEmail { get; set; }
    }
}
