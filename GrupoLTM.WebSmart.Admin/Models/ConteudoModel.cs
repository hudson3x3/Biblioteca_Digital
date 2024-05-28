using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GrupoLTM.WebSmart.Domain.Enums;
using System.Collections;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class TipoModuloModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string TituloPagina { get; set; }
        public EnumDomain.TipoModulo TipoModulo { get; set; }
        public EnumDomain.ModuloFixo ModuloFixo { get; set; }
    }

    public class ConteudoModel
    {
        public int Id { get; set; }
        public int TipoModuloId { get; set; }
        public int ModuloId { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Pretexto { get; set; }
        public string Texto { get; set; }
        public string LinkAcesso { get; set; }
        public string Alt { get; set; }
        public string ImgP { get; set; }
        public string ImgM { get; set; }
        public string imgG { get; set; }
        public string LinkDownload { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public bool? AtivoHome { get; set; }   
        public HttpPostedFileBase FileArquivo { get; set; }
        public HttpPostedFileBase FileImagem { get; set; }
        public int[] PerfilId { get; set; }
        public int[] EstruturaId { get; set; }
        public ArrayList ArrPerfilId { get; set; }
        public ArrayList ArrEstruturaId { get; set; }
    }

    public class TipoModuloConteudoModel
    {
        public TipoModuloModel TipoModuloModel { get; set; }
        public List<ConteudoModel> ConteudoModel { get; set; }
        public ConteudoModel ConteudoFirstModel { get; set; }
    }


}