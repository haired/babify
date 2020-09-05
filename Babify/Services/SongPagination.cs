using System.Collections.Generic;

namespace Babify.Services
{
    public class SongPagination
    {
        public decimal ToTalPages { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
