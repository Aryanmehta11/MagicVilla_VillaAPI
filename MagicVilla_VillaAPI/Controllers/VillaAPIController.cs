using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController] //helps in the validation of the Rquired data elements present in VIllaDTO
    public class VillaAPIController : ControllerBase
    {
        private ILogger<VillaAPIController> _logger;
        private ApplicationDbContext _db;

        public VillaAPIController(ILogger<VillaAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;

        }
        [HttpGet]
        public ActionResult <IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting all Villas");
            return Ok(_db.Villas.ToList());
        }
        [HttpGet("{id:int}",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Method to filter out data on basis of an ID
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) {
                _logger.LogError("Get Villa Error with Error" + id);
                 return BadRequest(); }
            var villa=_db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null) { 
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        public ActionResult <VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)


        {
            if (_db.Villas.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("", "Villa already Exists!");
                return BadRequest(ModelState);
            }
            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if (villaDTO.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id=villaDTO.Id,
                ImgUrl = villaDTO.ImgUrl,
                Name=villaDTO.Name,
                Occupancy=villaDTO.Occupancy,
                Rate=villaDTO.Rate,
                Sqft=villaDTO.Sqft,

            };
            _db.Villas.Add(model);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla",new {id= villaDTO.Id },villaDTO);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        
        [HttpDelete("{id:int}", Name = "DeleteVilla")]

        public IActionResult DeleteVilla (int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if(villa== null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla (int id , [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO==null || id!=villaDTO.Id)
            {
                return BadRequest();
            }
            //var villa=VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
            //villa.Name= villaDTO.Name;
            //villa.Occupancy= villaDTO.Occupancy;
            //villa.Sqft  = villaDTO.Sqft;
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImgUrl = villaDTO.ImgUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,

            };
            _db.Villas.Update(model);
            _db.SaveChanges();
             return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialData (int id , JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO==null || id == 0)
            {
                return BadRequest();
            }
            var villa=_db.Villas.AsNoTracking().FirstOrDefault(u=>u.Id==id);
            VillaDTO villaDTO = new()
            {
                Amenity = villa.Amenity,
                Details = villa.Details,
                Id = villa.Id,
                ImgUrl = villa.ImgUrl,
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,

            };

            if (villa == null)
            {
                return BadRequest();

            }
            patchDTO.ApplyTo(villaDTO,ModelState);
            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                Id = villaDTO.Id,
                ImgUrl = villaDTO.ImgUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,

            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent() ;
        }


    }
}
