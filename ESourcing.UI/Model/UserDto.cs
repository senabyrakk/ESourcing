using System.ComponentModel.DataAnnotations;

namespace ESourcing.UI.Model
{
    public class UserDto
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email{ get; set; }
        [Required(ErrorMessage ="Password is reqiured")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
