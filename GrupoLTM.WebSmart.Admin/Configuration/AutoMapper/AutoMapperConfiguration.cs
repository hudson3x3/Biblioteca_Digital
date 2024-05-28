using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Configuration.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Initialize()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<AutoMapperProfile>();
            });
        }
    }
}