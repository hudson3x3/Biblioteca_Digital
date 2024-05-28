using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class ParticipanteModel
    {
        public int Id { get; set; }
        public int? PerfilId { get; set; }
        public string Perfil { get; set; }
        public int? EstruturaId { get; set; }
        public string Estrutura { get; set; }
        public string Login { get; set; }
        public string Codigo { get; set; }
        public string Senha { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public int? EstadoId { get; set; }
        public string Estado { get; set; }
        public string DDDCel { get; set; }
        public string Celular { get; set; }
        public string DDDTel { get; set; }
        public string Telefone { get; set; }
        public string DDDTelComercial { get; set; }
        public string TelefoneComercial { get; set; }
        public string Email { get; set; }
        public bool? Ativo { get; set; }
        public bool? ParticipanteTeste { get; set; }
        public bool? ParticipanteVago { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataDesligamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public bool? OptInEmail { get; set; }
        public bool? OptInComunicacaoFisica { get; set; }
        public bool? OptInSms { get; set; }


    }

    public class ParticipanteResumoModel
    {
        public int Id { get; set; }
        public string Perfil { get; set; }
        public string Estrutura { get; set; }
        public string Login { get; set; }
        public string Status { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public string DataCadastro { get; set; }
        public string DataDesligamento { get; set; }
        public string DataInclusao { get; set; }
        public string DataAlteracao { get; set; }
    }
}