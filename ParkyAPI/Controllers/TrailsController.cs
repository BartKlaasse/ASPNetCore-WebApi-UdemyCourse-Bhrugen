using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]

    [ApiController]
    // [ApiExplorerSettings(GroupName = "ParkyOpenApiSpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //FLOW: Api controllers kunnen beter overerven van ControllerBase ipv Controller, aangezien een apicontroller niet alle mvc controller functionaliteiten nodig heeft

    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all the trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDTO>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDTO>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDTO>(obj));
            }
            return Ok(objDto);
        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail </param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepo.GetTrail(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDTO>(obj);
            return Ok(objDto);

        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="nationalParkId">The id of the trail </param>
        /// <returns></returns>
        [HttpGet("GetTrailInNationalPark/{nationalParkId:int}", Name = "GetTrailsInNationalPark")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDTO>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDTO>(obj));
            }
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDTO trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail already exists");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { version = HttpContext.GetRequestedApiVersion().ToString(), trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        //FLOW: Specify statuscode
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDTO trailDto)
        {
            //FLOW: TrailId (string input) should be the same as the id inside the dto
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepo.Update(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when patching the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            //FLOW: Not returning any content after httppatch
            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            //FLOW: if park does not exist return notfound(404)
            if (!_trailRepo.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.Delete(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            //FLOW: Not returning any content after httpdelete
            return NoContent();
        }
    }
}