using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Infrastructure.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using GrupoLTM.WebSmart.Infrastructure.Cache;

namespace GrupoLTM.WebSmart.Services
{
    public class MenuService : BaseService<Menu>
    {
        public static List<Menu> ListaMenu(int UsuarioAdmId, int MenuPaiId)
        {
            DataProvider.connectionString = ConfigurationManager.ConnectionStrings["GrupoLTMWebSmart"].ConnectionString;
            string proc = "JP_Sel_Menu";

            List<SqlParameter> listParam = new List<SqlParameter>();
            listParam.Add(new SqlParameter { ParameterName = "@UsuarioAdmId", Value = UsuarioAdmId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            listParam.Add(new SqlParameter { ParameterName = "@MenuPaiId", Value = MenuPaiId, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
            DataTable table = DataProvider.SelectStoreProcedure(proc, listParam);

            List<Menu> listMenu = new List<Menu>();

            foreach (DataRow row in table.Rows)
            {
                listMenu.Add(new Menu
                {
                    Ativo = row.Field<bool>("Ativo"),
                    DataInicio = row.Field<DateTime?>("DataInicio"),
                    DataFim = row.Field<DateTime?>("DataFim"),
                    Icone = row.Field<String>("Icone"),
                    Id = row.Field<Int32>("Id"),
                    Link = row.Field<String>("Link"),
                    Nome = row.Field<String>("Nome"),
                    Target = row.Field<String>("Target"),
                    Titulo = row.Field<String>("Titulo")
                });
            }

            return listMenu;


        }

        [Cache]
        public List<WebSmart.DTO.MenuModel> ListarMenu(int perfilId, int estruturaId, EnumDomain.Menu menuTipo)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {                
                
                IRepository repMenu = context.CreateRepository<Menu>();
                List<WebSmart.DTO.MenuModel> listMenuModel = new List<WebSmart.DTO.MenuModel>();

                List<WebSmart.Domain.Models.Menu> listMenu = new List<WebSmart.Domain.Models.Menu>();
                listMenu = repMenu.Filter<Menu>(x => (EnumDomain.Menu)x.MenuPaiId == menuTipo
                    && x.Ativo == true
                    && x.MenuPerfil.Any(mp => mp.PerfilId == perfilId && mp.Ativo)
                    && x.MenuEstrutura.Any(me => me.EstruturaId == estruturaId && me.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)).OrderBy(x => x.Nome).OrderBy(x => x.Ordem).ToList();

                foreach (var item in listMenu)
                {
                    listMenuModel.Add(new MenuModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Titulo = item.Titulo,
                        Link = item.Link,
                        Target = item.Target,
                        Icone = item.Icone,
                        SubMenu = ListarMenu(perfilId, estruturaId, item.Id)
                    });
                                        
                }
                return listMenuModel;
                                 
            }
        }

        public List<WebSmart.DTO.MenuModel> ListarMenu(int perfilId, int estruturaId, int idMenuPai)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {

                IRepository repMenu = context.CreateRepository<Menu>();
                List<WebSmart.DTO.MenuModel> listMenuModel = new List<WebSmart.DTO.MenuModel>();

                List<WebSmart.Domain.Models.Menu> listMenu = new List<WebSmart.Domain.Models.Menu>();
                listMenu = repMenu.Filter<Menu>(x => x.MenuPaiId == idMenuPai
                    && x.Ativo == true
                    && x.MenuPerfil.Any(mp => mp.PerfilId == perfilId && mp.Ativo)
                    && x.MenuEstrutura.Any(me => me.EstruturaId == estruturaId && me.Ativo)
                    && (x.DataInicio == null || DateTime.Now > x.DataInicio && x.DataFim > DateTime.Now)).OrderBy(x => x.Nome).ToList();

                foreach (var item in listMenu)
                {
                    listMenuModel.Add(new MenuModel
                    {
                        DataInclusao = item.DataInclusao,
                        DataAlteracao = item.DataAlteracao,
                        Ativo = item.Ativo,
                        Id = item.Id,
                        Nome = item.Nome,
                        Titulo = item.Titulo,
                        Link = item.Link,
                        Target = item.Target,
                        Icone = item.Icone
                    });
                    
                }
                return listMenuModel;

            }
        }
    }
}
