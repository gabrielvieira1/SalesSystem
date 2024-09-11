using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public string Password { get; set; }
    public string AccessToken { get; set; }
    public string AccessTokenExpires { get; set; }
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    [Required]
    public bool Active { get; set; }
  }
}
