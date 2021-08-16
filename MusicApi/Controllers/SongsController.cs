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
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Song song)
        {

            var imageUrl = await FileHelper.UploadImage(song.Image);
            song.ImageUrl = imageUrl;

            var audioUrl = await FileHelper.UploadFile(song.AudioFile);
            song.AudioUrl = audioUrl;

            song.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }


        // GET: api/songs
        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var songs = await (from song in _dbContext.Songs
                                select new
                                {
                                    Id = song.Id,
                                    Title = song.Title,
                                    Duration = song.Duration,
                                    ImageUrl = song.ImageUrl,
                                    AudioUrl = song.AudioUrl

                                }
            ).ToListAsync();

            return Ok(songs.Skip((currentPageNumber - 1)* currentPageSize).Take(currentPageSize));

        }

        // GET: api/songs/feature
        [HttpGet("[action]")]
        public async Task<IActionResult> FeatureSongs()
        {
            var songs = await (from song in _dbContext.Songs where song.IsFeatured == true
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl

                               }
            ).ToListAsync();

            return Ok(songs);

        }


        // GET: api/songs/NewSongs
        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var songs = await (from song in _dbContext.Songs
                               orderby song.UploadedDate descending
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl

                               }
            ).Take(15).ToListAsync(); //.Take(15) means 15 ka latest sa descending ang e view

            return Ok(songs);

        }

        //// GET: api/songs?sortTitle==asc
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetSortSong(string sortTitle)
        //{

        //    switch (sortTitle)
        //    {
        //        case "desc":
        //            var songs = await (_dbContext.Songs.OrderByDescending(p => p.Title)).Take(15).ToListAsync();
        //            break;
        //        case "asc":
        //            var songs = await (_dbContext.Songs.OrderBy(p => p.Title)).Take(15).ToListAsync();
        //            break;
        //        default:
        //            songs = _dbContext.Songs;
        //            break;



        //    }

        //    return Ok(songs);

        //}



        // GET: api/songs/SearchSongs?query=1
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchSongs(string query)
        {
            var songs = await (from song in _dbContext.Songs
                               where song.Title.StartsWith(query)
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   ImageUrl = song.ImageUrl,
                                   AudioUrl = song.AudioUrl

                               }
            ).Take(15).ToListAsync(); //.Take(15) means 15 ka latest sa descending ang e view

            return Ok(songs);

        }

    }
}
