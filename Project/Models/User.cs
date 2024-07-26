using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email Field is Empty")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Field is Empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role {  get; set; }

    }
}
