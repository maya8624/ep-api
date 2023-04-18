﻿using ep.API.Service.Hubs;
using ep.Contract.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ep.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("GetTodaysCustomers/{shopId:int}/")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Customer>>> GetTodaysCustomers(int shopId)
        {
            try
            {
                if (shopId == 0)
                {
                    return BadRequest();
                }

                var result = await _service.GetTodaysCustomers(shopId);                
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(typeof(CustomerRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("SaveCustomer")]
        public async Task<ActionResult> SaveCustomer([FromBody] CustomerRequest customerCreateDto)
        {
            try
            {
                await _service.CreateCustomerAsync(customerCreateDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
