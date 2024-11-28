using System.ComponentModel.DataAnnotations;

namespace CsvQueueProcessor.Models
{
    public class SignUpViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
