using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTO;
using WebApp.Exceptions;
using WebApp.Models;
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
    public async Task<IActionResult> RegisterProductInWarehouseAsync(
        [FromBody] RegisterProductInWarehouseRequestDTO registerProductInWarehouseRequestDto)
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


    [HttpPost("procedure")]
    public async Task<IActionResult> RegisterProductInWarehouseWithProcedureAsync(
        [FromBody] RegisterProductInWarehouseRequestDTO registerProductInWarehouseRequestDto)
    {
        var idProduct = new SqlParameter("@IdProduct", registerProductInWarehouseRequestDto.IdProduct);
        var idWarehouse = new SqlParameter("@IdWarehouse", registerProductInWarehouseRequestDto.IdWarehouse);
        var amount = new SqlParameter("@Amount", registerProductInWarehouseRequestDto.Amount);
        var createdAt = new SqlParameter("@CreatedAt", DateTime.UtcNow);

        await using var connection =
            new SqlConnection("Server=localhost,1433;Database=Warehouse;User=SA;Password=admin123!;TrustServerCertificate=True");
        await using var command = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.Add(idProduct);
        command.Parameters.Add(idWarehouse);
        command.Parameters.Add(amount);
        command.Parameters.Add(createdAt);

        await connection.OpenAsync();
        var result = new ProductWarehouse();
        await using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                result.IdProductWarehouse = (int)dr["IdProductWarehouse"];
                return StatusCode(StatusCodes.Status200OK, new RegisterProductInWarehouseResponseDTO(result.IdProductWarehouse));
            }
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "Error while adding product to warehouse");
    }
}