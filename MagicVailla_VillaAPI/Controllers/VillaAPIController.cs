using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers;

[Route("api/[controller]")]
[ApiController] // allows to validate annotations in model class 
public class VillaAPIController : ControllerBase
{
    private readonly ILogger<VillaAPIController> _logger;

    public VillaAPIController(ILogger<VillaAPIController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        _logger.LogInformation("Get Villas");
        return Ok(VillaStore.villaList);
    }

    [HttpGet("{id:int}", Name = "GetVillas")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(200, Type =typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<VillaDTO> GetVillaById(int id)
    {
        if (id == 0)
        {
            _logger.LogError("Get Villa By id error");
            return BadRequest();
        }

        ;


        var villa = VillaStore.villaList?.FirstOrDefault(x => x.Id == id);
        if (villa == null) return NotFound();

        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO? villaDTO)
    {
        if (villaDTO == null) return BadRequest(villaDTO);

        if (villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

        if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name) != null)
        {
            ModelState.AddModelError("CustomError", "Villa Already Exist");
            return BadRequest(ModelState);
        }


        villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
        VillaStore.villaList.Add(villaDTO);
        return CreatedAtRoute("GetVillas", new { id = villaDTO.Id }, villaDTO);
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteVilla(int id)
    {
        if (id == 0) return BadRequest();
        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        if (villa == null) return NotFound();

        VillaStore.villaList.Remove(villa);
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaDTO? villaDTO)
    {
        if (villaDTO == null || id != villaDTO.Id) return BadRequest();

        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        if (villa == null) return NotFound();
        villa.Name = villaDTO.Name;
        villa.Sqft = villaDTO.Sqft;
        villa.Occupancy = villaDTO.Occupancy;
        return NoContent();
    }

    //https://jsonpatch.com/
    //Sample request
    // {
    //     "operationType": "replace",
    //     "path": "/name",
    //     "value": "M2"
    // }


    [HttpPatch("{id:int}", Name = "UpdateVillaPartial")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateVillaPartial(int id, JsonPatchDocument<VillaDTO>? patchDto)
    {
        if (patchDto == null || id == 0) return BadRequest();
        var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
        if (villa == null) return NotFound();
        patchDto.ApplyTo(villa, ModelState);
        return NoContent();
    }
}