using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Models
{
    public class SigninInput
    {
        [Display(Name ="Email adresiniz")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name ="Şifreniz ")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Beni Hatırla")]
        [Required]

        public bool IsRemember { get; set; }
    }
}
