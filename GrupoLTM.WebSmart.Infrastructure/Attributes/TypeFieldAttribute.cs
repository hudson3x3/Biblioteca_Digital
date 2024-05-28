using System;

namespace GrupoLTM.WebSmart.Infrastructure.Attributes
{
    public class FieldTypeAttribute : Attribute
    {
        public readonly string[] Types;

        public FieldTypeAttribute(params string[] types)
        {
            Types = types;
        }
    }
}
