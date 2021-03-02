using System.ComponentModel.DataAnnotations;

namespace MiaServiceDotNetLibrary.Environment
{
    public abstract class MiaEnvsSchema
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string USERID_HEADER_KEY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string USER_PROPERTIES_HEADER_KEY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string GROUPS_HEADER_KEY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string CLIENTTYPE_HEADER_KEY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string BACKOFFICE_HEADER_KEY { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string MICROSERVICE_GATEWAY_SERVICE_NAME { get; set; }

        public virtual void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), validateAllProperties: true);
        }
    }
}
