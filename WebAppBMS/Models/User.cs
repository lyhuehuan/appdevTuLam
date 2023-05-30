using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebAppBMS.Models;

public class User: IdentityUser
{
    [Required]
    public string FullName { get; set; }
    public string Address { get; set; }
    [NotMapped]
    public string Role { get; set; }
}