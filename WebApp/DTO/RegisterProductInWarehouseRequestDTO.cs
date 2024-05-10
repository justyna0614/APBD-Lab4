using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO;

public class RegisterProductInWarehouseRequestDTO
{
    [Required] 
    public int IdProduct { get; set; }

    [Required] 
    public int IdWarehouse { get; set; }

    [Required] [Range(1, int.MaxValue)] 
    public int Amount { get; set; }

    [Required] 
    public DateTime CreatedAt { get; set; }
}