using System.Collections.Generic;

namespace GrupoLTM.WebSmart.DTO
{
    public class DashboardModel
    {        
        public long LinhasIntegradas { get; set; }
        public long LinhasComErro { get; set; }
        public long LinhasProcessadas { get { return LinhasIntegradas - LinhasComErro; } }
        public long RevendedoresProcessados { get; set; }
        public long PontosDisponiveis { get; set; }
        public long PontosCancelados { get; set; }
        public long PontosPendentes { get; set; }

        public List<DashBoardItem> Itens { get; set; }
    }

    public class DashBoardItem
    {
        public string TipoArquivo { get; set; }
        public long LinhasIntegradas { get; set; }
        public long LinhasComErro { get; set; }
        public long LinhasProcessadas { get { return LinhasIntegradas - LinhasComErro; } }
        public long RevendedoresProcessados { get; set; }
        public long PontosDisponiveis { get; set; }
        public long PontosCancelados { get; set; }
        public long PontosPendentes { get; set; }
    }
}
