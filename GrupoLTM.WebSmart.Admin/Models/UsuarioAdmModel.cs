using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Models
{
    public class UsuarioAdmModel
    {
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public string Perfil { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Matricula { get; set; }
        public int? SerieCursar { get; set; }
        public string Periodo { get; set; }
        public bool Ativo { get; set; }
        public bool ReenviarSenha { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public int[] MenuId { get; set; }
        public ArrayList ArrMenuId { get; set; }
        //public Nullable<int> Matricula { get; set; }
        //public Nullable<int> SerieCursar { get; set; }
        //public string Periodo { get; set; }

    }
}