using System.ComponentModel.DataAnnotations;

namespace Core.Shared;

public class BaseEntity
{
    [Key]
    public long Id { get; set; }
}
