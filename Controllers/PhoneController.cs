using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Phone_project_API.DTOs;
using Phone_project_API.Models;
using Phone_project_API.Services;

namespace Phone_project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PhonesService _phonesService;

        public PhonesController(IConfiguration configuration, PhonesService phonesService)
        {
            _configuration = configuration;
            _phonesService = phonesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phone>>> Get()
        {
            try
            {
                List<Phone> phones = await _phonesService.GetPhones();

                if (phones.Count == 0)
                {
                    Console.WriteLine("No phones found!");
                    return Ok(phones);
                }

                Console.WriteLine("All phones retrieved.");
                return Ok(phones);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving phones: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Phone>> GetById(int id)
        {
            try
            {
                Phone phone = await _phonesService.GetPhoneById(id);

                if (phone == null)
                {
                    Console.WriteLine($"Phone not found: id={id}");
                    return NotFound();
                }

                Console.WriteLine($"Phone details retrieved: id={id}.");
                return Ok(phone);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving phone details: id={id}. Error message: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Phone>> Post(PhoneDto phoneDto)
        {
            try
            {
                Phone phone = await _phonesService.AddPhone(phoneDto);

                if (phone == null)
                {
                    Console.WriteLine("Error creating new phone: Invalid input data.");
                    return BadRequest();
                }

                Console.WriteLine($"New phone created: {phone.ToString()}.");
                return CreatedAtAction(nameof(GetById), new { id = phone.Id }, phone);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a new phone: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PhoneDto phone)
        {
            try
            {
                bool success = await _phonesService.UpdatePhone(id, phone);

                if (!success)
                {
                    Console.WriteLine($"Phone not found for update: id={id}");
                    return NotFound("Error updating phone. Invalid input data.");
                }

                Console.WriteLine($"Phone updated: {phone.ToString()}.");
                return Ok();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred while updating phone details: id={id}. Error message: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool success = await _phonesService.DeletePhone(id);

                if (!success)
                {
                    Console.WriteLine($"Phone not found for deletion: id={id}");
                    return NotFound();
                }

                Console.WriteLine($"Phone deleted: id={id}.");
                return Ok();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting phone: id={id}. Error message: { ex.Message}");
                return StatusCode(500);
            }
        }
    }
}