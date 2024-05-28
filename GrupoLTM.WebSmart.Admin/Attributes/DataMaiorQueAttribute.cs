using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GrupoLTM.WebSmart.Admin.Attributes
{
    public class DataMaiorQueAttribute : ValidationAttribute
    {
        private DateTime DataReferencia;
        private readonly string NomeOutroCampo;
        private readonly bool PodeSerIgual;
        private readonly bool Now;

        public DataMaiorQueAttribute(string nomeOutroCampo, bool podeSerIgual = true)
        {
            Now = false;
            NomeOutroCampo = nomeOutroCampo;
            PodeSerIgual = podeSerIgual;
        }

        public DataMaiorQueAttribute(DateTime dataReferencia, bool podeSerIgual = true)
        {
            Now = false;
            DataReferencia = dataReferencia;
            PodeSerIgual = podeSerIgual;
        }

        /// <summary>
        /// Data deve ser maior que Agora.
        /// </summary>
        /// <param name="podeSerIgual"></param>
        public DataMaiorQueAttribute(bool podeSerIgual = true)
        {
            Now = true;
            DataReferencia = DateTime.Now;
            PodeSerIgual = podeSerIgual;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   
            if (value == null)
                return ValidationResult.Success;

            var dateValue = Convert.ToDateTime(value);
            var displayName = "";

            if (!String.IsNullOrEmpty(NomeOutroCampo))
            {
                var propertyName = validationContext.ObjectType.GetProperty(NomeOutroCampo);
                displayName = (GetCustomAttribute(propertyName, typeof(DisplayAttribute), false) as DisplayAttribute)?.Name;
                if (propertyName == null)
                    return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Unknown property {0}", new[] { NomeOutroCampo }));

                var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null) as DateTime?;

                if (!propertyValue.HasValue)
                    return new ValidationResult($"Informe um valor para {displayName}");

                DataReferencia = propertyValue.Value;
            }
            else
            {
                if (Now)
                    displayName = "agora";
                else
                    displayName = DataReferencia.ToString("dd/MM/yyyy HH:mm");
            }
            

            if (dateValue < DataReferencia)
                return new ValidationResult($"{validationContext.DisplayName} não pode ser menor que {displayName}");

            if (!PodeSerIgual && dateValue == DataReferencia)
                return new ValidationResult($"{validationContext.DisplayName} não pode ser menor ou igual que {displayName}");

            return ValidationResult.Success;
        }
    }
}