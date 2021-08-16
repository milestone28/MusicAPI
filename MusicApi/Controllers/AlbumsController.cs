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
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Album album)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageUrl = await FileHelper.UploadImage(album.Image);
            album.ImageUrl = imageUrl;
            await _dbContext.Albums.AddAsync(album);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }


        // GET: api/albums
        [HttpGet]
        public async Task<IActionResult> GetAlbums(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var albums = await (from Album in _dbContext.Albums
                                 select new
                                 {
                                     Id = Album.Id,
                                     Name = Album.Name,
                                     ImageUrl = Album.ImageUrl

                                 }
            ).ToListAsync();

            return Ok(albums.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));

        }


        // GET: api/albums/id
        [HttpGet("[action]")]
        public async Task<IActionResult> AlbumDetails(int albumId)

        {
            var albumDetails = await _dbContext.Albums.Where(a => a.Id == albumId).Include(a => a.Songs).ToListAsync();
            return Ok(albumDetails);

        }

    }
}
