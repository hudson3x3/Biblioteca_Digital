using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GrupoLTM.WebSmart.Admin.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileTypeAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _DefaultErrorMessage = "O campo {0} somente aceita arquivos com as seguintes extensões: {1}";
        private IEnumerable<string> _ValidTypes { get; set; }

        public FileTypeAttribute(string validTypes)
        {
            _ValidTypes = validTypes.Split(',').Select(s => s.Trim().ToLower());
            ErrorMessage = ErrorMessage ?? _DefaultErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMessage = string.Format(ErrorMessage, validationContext.DisplayName, string.Join(", ", _ValidTypes));
            
            var file = value as HttpPostedFileBase;
            if (file != null && !_ValidTypes.Any(e => file.FileName.EndsWith(e)))
                return new ValidationResult(errorMessage);

            var files = value as IEnumerable<HttpPostedFileBase>;
            if (files != null)
                foreach (HttpPostedFileBase f in files)
                    if (f != null && !_ValidTypes.Any(e => f.FileName.EndsWith(e)))
                        return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filetype",
                ErrorMessage = ErrorMessageString
            };
            rule.ValidationParameters.Add("validtypes", string.Join(",", _ValidTypes));
            yield return rule;
        }
    }
}