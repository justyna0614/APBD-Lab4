using Microsoft.AspNetCore.Mvc;
using WebApp.DTO;
using System.Data.SqlClient;
using WebApp.Exceptions;
using WebApp.Services;

namespace WebApp.Controllers;

[Route("api/warehouse")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IConfiguration _configuration;
    private IWarehouseService _warehouseService;


    public WarehouseController(IConfiguration configuration, IWarehouseService warehouseService)
    {
        _configuration = configuration;
        _warehouseService = warehouseService;
    }
    
    [HttpPost]
    public IActionResult RegisterProductInWarehouse([FromBody] RegisterProductInWarehouseRequestDTO registerProductInWarehouseRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var productWarehouse = _warehouseService.AddStock(registerProductInWarehouseRequestDto);
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