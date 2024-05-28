using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class ParticipanteModel
    {

        public ParticipanteModel()
        {
            this.Catalogos = new List<ParticipanteCatalogoModel>();
        }

        public string AccessToken { get; set; }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string SenhaAtual { get; set; }
        public string ConfirmaSenha { get; set; }
        public int StatusId { get; set; }
        public string Estrutura { get; set; }
        public int EstruturaId { get; set; }
        public string Status { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Perfil { get; set; }
        public int PerfilId { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public System.DateTime? DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public int? EstadoId { get; set; }
        public string Estado { get; set; }
        public string PontoReferencia { get; set; }
        public string DDDCel { get; set; }
        public string Celular { get; set; }
        public string DDDTel { get; set; }
        public string Telefone { get; set; }
        public string DDDTelComercial { get; set; }
        public string TelefoneComercial { get; set; }
        public string Email { get; set; }
        public bool? Ativo { get; set; }
        public bool? SimuladorVisualizado { get; set; }
        public System.DateTime? DataCadastro { get; set; }
        public System.DateTime? DataDesligamento { get; set; }
        public System.DateTime DataInclusao { get; set; }
        public System.DateTime? DataAlteracao { get; set; }
        public int TipoAcessoId { get; set; }
        public bool OptInEmail { get; set; }
        public bool OptInComunicacaoFisica { get; set; }
        public bool OptInSms { get; set; }
        public bool OptinAceite { get; set; }
        public bool? ParticipanteTeste { get; set; }
        public List<ParticipanteCatalogoModel> Catalogos { get; set; }
    }
}
