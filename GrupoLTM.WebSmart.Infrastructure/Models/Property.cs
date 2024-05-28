using System;
using System.Reflection;

namespace GrupoLTM.WebSmart.Infrastructure.Models
{
    public class Property
    {
        public Property(string name, Type type, PropertyInfo propertyInfo = null, FieldInfo fieldInfo = null)
        {
            Name = name;
            Type = type;
            PropertyInfo = propertyInfo;
            FieldInfo = fieldInfo;
        }

        public Type Type { get; set; }

        public string Name { get; set; }
        
        public object Value { get; set; }

        public PropertyInfo PropertyInfo { get; set; }
        
        public FieldInfo FieldInfo { get; set; }


        public object[] GetAttributes()
        {
            return FieldInfo != null ? FieldInfo.GetCustomAttributes(true) : PropertyInfo.GetCustomAttributes(true);
        }
    }
}
