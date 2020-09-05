using Babify.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Babify.Controllers
{
    [ApiController]
    public class SongController: ControllerBase
    {
        private readonly ISongService songService;

        public SongController(ISongService songService)
        {
            this.songService = songService;
        }

        /// <summary>
        /// Add a new son to the library.
        /// </summary>
        /// <param name="newSong">The song details.</param>
        /// <returns></returns>
        [Route("song")]
        [HttpPost]
        public async Task<ActionResult<string>> AddSong(Song newSong)
        {
            var serviceResult = await this.songService.CreateSong(newSong);
            return InterpretServiceResult(serviceResult);
        }

        /// <summary>
        /// Edit an existing.
        /// </summary>
        /// <param name="song">The new details of the song.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("song")]
        public async Task<ActionResult<Song>> EditSong([FromBody]Song song)
        {
            var serviceResult = await this.songService.CreateSong(song);
            return InterpretServiceResult(serviceResult);
        }

        /// <summary>
        /// Delete a song.
        /// </summary>
        /// <param name="id">Id of the song.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("song/{id}")]
        public async Task<ActionResult> DeleteSong(string id)
        {
            var serviceResult = await this.songService.DeleteSong(id);
            return InterpretServiceResult(serviceResult);
        }

        /// <summary>
        /// Retreive the details of a song.
        /// </summary>
        /// <param name="id">Id of the song.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("song/{id}")]
        public async Task<ActionResult<Song>> RetreiveSong(string id)
        {
            var serviceResult = await this.songService.RetreiveSong(id);
            return InterpretServiceResult(serviceResult);
        }

        /// <summary>
        /// Retreive the count of all songs starting from a year.
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns></returns>
        [HttpGet]
        [Route("song/stats/{year}")]
        public async Task<ActionResult<int>> RetreiveSongFromYear(int year)
        {
            var serviceResult = await this.songService.SongsStats(year);
            return InterpretServiceResult(serviceResult);
        }

        /// <summary>
        /// Retreive a paginated list of an artist.
        /// </summary>
        /// <param name="artistName">Artist name.</param>
        /// <param name="page">Actual page.</param>
        /// <param name="pageSize">Pagesize.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("song/artist/{artistName}")]
        public async Task<ActionResult<SongPagination>> RetreiveSongFromArtist(string artistName, int page= 1, int pageSize = 10)
        {
            var serviceResult = await songService.RetreiveSongOfArtist(artistName, pageSize, page);
            return InterpretServiceResult(serviceResult);
        }

        private ActionResult InterpretServiceResult<T>(ServiceResult<T> serviceResult)
        {
            return InterpretServiceResult(serviceResult, serviceResult.Result);
        }

        private ActionResult InterpretServiceResult(ServiceResult serviceResult, object result = null)
        {
            if (serviceResult.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(serviceResult.Error);
            }
        }
    }
}
