using Microsoft.AspNetCore.Mvc;
using MusicService.Exceptions;
using MusicService.DTOs;
using MusicService.Services.Interfaces;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KindController : ControllerBase
    {
        private readonly IKindService _service;

        public KindController(IKindService service)
        {
            _service = service;
        }

        // GET: api/Kind
        [HttpGet]
        public async Task<ActionResult<List<KindDTO>>> GetKinds()
        {
            return await _service.GetKinds();
        }

        // GET: api/Kind/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KindDTO>> GetKind(int id)
        {
            try{
                return await _service.GetKind(id);
            }catch(NotFoundException e){
                return NotFound(e.Content);
            }
        }

        // // PUT: api/Kind/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutKind(int id, KindDTO kDTO)
        // {
        //     if (id != kDTO.KindId) {
        //         return BadRequest();
        //     }
            
        //     try{
        //         await _service.PutKind(id, kDTO);
        //     }catch(NotFoundException e){
        //         return NotFound(e.Content);
        //     }
    
        //     return NoContent();
        // }

        // // POST: api/Kind
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<KindDTO>> PostKind(KindDTO kDTO)
        // {
        //     try{
        //         await _service.PostKind(kDTO);
        //     }catch(AlreadyExistsException e){
        //         return BadRequest(e.Content);
        //     }
        //     return CreatedAtAction("GetKind", new { id = kDTO.KindId }, kDTO);
        // }

        // // DELETE: api/Kind/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteKind(int id)
        // {
        //     try{
        //         await _service.DeleteKind(id);
        //     }catch (NotFoundException e){
        //         return NotFound(e.Content);
        //     }

        //     return NoContent();
        // }
    }
}
