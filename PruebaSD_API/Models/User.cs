using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UsuId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } =string.Empty;
    public string cargo { get; set; } = string.Empty;
    public string salario { get; set; } = string.Empty;
    public string departamento { get; set; } = string.Empty;

    public string fechaDeContratacion { get; set; } = string.Empty;
}
