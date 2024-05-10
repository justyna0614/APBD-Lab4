using Microsoft.AspNetCore.Mvc;
using WebApp.DTO;
using WebApp.Exceptions;
using WebApp.Services;

namespace WebApp.Controllers;

[Route("api/warehouse")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;


    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task <IActionResult> RegisterProductInWarehouse([FromBody] RegisterProductInWarehouseRequestDTO registerProductInWarehouseRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var productWarehouse = await _warehouseService.AddStock(registerProductInWarehouseRequestDto);
            return productWarehouse != null
                ? StatusCode(StatusCodes.Status200OK, new RegisterProductInWarehouseResponseDTO(productWarehouse.IdProductWarehouse))
                : StatusCode(StatusCodes.Status500InternalServerError, "Error while adding product to warehouse");
        }
        catch (OrderNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ProductNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (WarehouseNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (IncorectAmountException e)
        {
            return Conflict(e.Message);
        }
        catch (OrderCompletedException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
}