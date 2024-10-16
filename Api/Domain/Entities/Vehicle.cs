

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_minimals_apis.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = default!;
        [Required]
        public int Ano { get; set; } = default!;

    }
}