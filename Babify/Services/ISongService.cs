using System.Threading.Tasks;

namespace Babify.Services
{
    public interface ISongService
    {
        Task<ServiceResult<Song>> CreateSong(Song newSong);
        Task<ServiceResult<Song>> EditSong(Song newSong);
        Task<ServiceResult> DeleteSong(string id);
        Task<ServiceResult<Song>> RetreiveSong(string id);
        Task<ServiceResult<long>> SongsStats(int year);
        Task<ServiceResult<SongPagination>> RetreiveSongOfArtist(string artistName, int pageSize, int page);
    }
}
