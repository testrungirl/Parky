using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;
        public TrailController(IMapper mapper, ITrailRepository trailRepo)
        {
            _mapper = mapper;
            _trailRepo = trailRepo;
        }
        /// <summary>
        /// Gets list of all Trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Trail>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objList);
        }
        /// <summary>
        ///  Gets Trail by Id
        /// </summary>
        /// <param name="id">The id of the Trail</param>
        /// <returns></returns>
        [HttpGet("{id:Guid}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(Trail))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(Guid id)
        {
            var obj = _trailRepo.GetTrail(id);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailUpdateDto trailUpdateDto)
        {
            if(trailUpdateDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailUpdateDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailUpdateDto);
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { id = trailObj.Id }, trailObj);
        }

        [HttpPatch("{id:Guid}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(Guid id, [FromBody]TrailUpdateDto TrailDto)
        {
            if (TrailDto == null || id != TrailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(TrailDto);
            if (!_trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{id:Guid}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(Guid id)
        {
            var TrailObj = _trailRepo.GetTrail(id);

            if (!_trailRepo.TrailExists(TrailObj.Name))
            {
                return NotFound();
            }


            if (!_trailRepo.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {id}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
