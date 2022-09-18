using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Shelter.Models;

namespace Shelter.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class AnimalsController : ControllerBase
  {
    private readonly ShelterContext _db;

    public AnimalsController(ShelterContext db)
    {
      _db = db;
    }

// Get methods
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> Get(string species, string breed, string gender, bool spayed, string houseTrained, string vaccinated)
    {
      var query = _db.Animals.AsQueryable();

      if(species != null)
      {
        query = query.Where(entry => entry.Species == species);
      }
      if(breed != null)
      {
        query = query.Where(entry => entry.Breed.Contains(breed));
      }
      if(gender != null)
      {
        query = query.Where(entry => entry.Gender == gender);
      }
  

      return await query.ToListAsync();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
      var animal = await _db.Animals.FindAsync(id);

      if (animal == null)
      {
          return NotFound();
      }

      return animal;
    }

// Post methods
    
    [HttpPost]
    public async Task<ActionResult<Animal>> Post(Animal animal)
    {
      _db.Animals.Add(animal);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetAnimal), new { id = animal.AnimalId }, animal);
    }

    
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Animal animal)
    {
      if (id != animal.AnimalId)
      {
        return BadRequest();
      }

      _db.Entry(animal).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (_db.Animals.Any(e => e.AnimalId == id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

  
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAnimal(int id)
    {
      var animal = await _db.Animals.FindAsync(id);
      if (animal == null)
      {
        return NotFound();
      }

      _db.Animals.Remove(animal);
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}