using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace EfCoreSample.Entities;

public abstract class EntityBase
{
    [Column("ID"), Key]
    public virtual int Id { get; set; }
}