using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class CampanhaConteudoModel
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; }
        public string PreTexto { get; set; }
        public int CampanhaId { get; set; }
        public int TipoConteudoId { get; set; }
        public bool Ativo { get; set; }
        public string Arquivo { get; set; }

    }
}
