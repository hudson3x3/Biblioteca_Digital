using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.DTO
{
    public class IconeSimuladorModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public short Tipo { get; set; }
        public string CaminhoImagem { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public virtual ICollection<MecanicaSimuladorModel> MecanicaSimulador { get; set; }
    }
}
