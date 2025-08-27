using System.ComponentModel.DataAnnotations;
using MyInstitute.SchoolManagement.Shared.Enum;

namespace MyInstitute.SchoolManagement.Shared.Models;

public class UserRoleDetails
{
    [Key]
    public int UserRoleDetailsId { get; set; }

    [Display(Name = "Rol Usuario")]
    public UserType? UserType { get; set; }

    [Display(Name = "User Id")]
    public string? UserId { get; set; }

    //Relaciones
    public User? User { get; set; }
}