using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Helpers;
using MusicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageUrl = await FileHelper.UploadImage(artist.Image);
            artist.ImageUrl = imageUrl;
            await _dbContext.Artists.AddAsync(artist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);

        }

        // GET: api/artists
        [HttpGet]
        public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;

            var artists = await (from artist in _dbContext.Artists select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl

            }
            ).ToListAsync();

            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));

        }

        // GET: api/artists/id
        [HttpGet("[action]")]
        public async Task<IActionResult> ArtistDetails(int artistId)
             
        {
             var artistDetails = await _dbContext.Artists.Where(a1 => a1.Id == artistId).Include(a => a.Songs).ToListAsync();

            if (artistDetails == null)
            {
                return NotFound(" No Record Found ");
            }
            return Ok(artistDetails);
        }


        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artist = await _dbContext.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound(" No Record Found Against this Id ");
            }
            else
            {
                _dbContext.Artists.Remove(artist);
                await _dbContext.SaveChangesAsync();
                return Ok("Record Deleted");
            }

        }

        //PUT : api/Artists/5
        [HttpPut]

        public async Task<IActionResult> Put(int id, [FromBody] Artist artistObj)
        {
            var artist = await _dbContext.Songs.FindAsync(id);
            if (artist == null)
            {
                return NotFound(" No Record Found Against this Id ");
            }
            else
            {
                artist.Title = artistObj.Name;
                artist.Duration = artistObj.Gender;
                await _dbContext.SaveChangesAsync();
                return Ok("Record Updated");
            }

        }

    }
}
