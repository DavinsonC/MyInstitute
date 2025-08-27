using MyInstitute.SchoolManagement.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace MyInstitute.SchoolManagement.Shared.EntitiesSoftSec;

public class UsuarioRole
{
    [Key]
    public int UsuarioRoleId { get; set; }

    [Required(ErrorMessage = "El largo maximo es de {0}")]
    [Display(Name = "Usuario")]
    public int UsuarioId { get; set; }

    [Display(Name = "Tipo Usuario")]
    public UserType UserType { get; set; }

    //Relaciones

    public Usuario? Usuario { get; set; }
}