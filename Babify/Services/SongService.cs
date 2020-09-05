using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Babify.Services
{
    public class SongService : ISongService
    {
        private readonly IMongoCollection<Song> songCollection;

        public SongService()
        {
            var client = new MongoClient("mongodb+srv://haired:Franciscaine22$@cluster0.fhsf1.azure.mongodb.net/babify?retryWrites=true&w=majority");
            var database = client.GetDatabase("babify");
            songCollection = database.GetCollection<Song>("songs");
        }

        public async Task<ServiceResult<Song>> CreateSong(Song newSong)
        {
            if(newSong == null)
            {
                return ServiceResult.ValidationError<Song>();
            }

            if(newSong.PublishingYear <= 1900)
            {
                newSong.PublishingYear = DateTime.UtcNow.Year;
            }
            await songCollection.InsertOneAsync(newSong);
            return ServiceResult.Succes(newSong);
        }

        public async Task<ServiceResult> DeleteSong(string id)
        {
            var filter = Builders<Song>.Filter.Eq(s => s.Id, id);
            var result = await songCollection.DeleteOneAsync(filter);
            if (result.DeletedCount == 1)
            {
                return ServiceResult.Succes();
            }
            else
            {
                return new ServiceResult { Error = "Cannot delete this song", IsSuccess = false };
            }
        }

        public async Task<ServiceResult<Song>> EditSong(Song song)
        {
            if (song == null)
            {
                return ServiceResult.ValidationError<Song>();
            }

            var filter = Builders<Song>.Filter.Eq(s => s.Id, song.Id);
            var result = await this.songCollection.FindOneAndReplaceAsync(filter, song);
            return ServiceResult.Succes(song);
        }

        public async Task<ServiceResult<Song>> RetreiveSong(string id)
        {
            var filter = Builders<Song>.Filter.Eq(s => s.Id, id);
            var song = await this.songCollection.Find(filter).FirstOrDefaultAsync();
            if(song == null)
            {
                return ServiceResult.ValidationError<Song>("Unknown song");
            }

            return ServiceResult.Succes(song);
        }

        public async Task<ServiceResult<SongPagination>> RetreiveSongOfArtist(string artistName, int pageSize, int page)
        {
            var filter = Builders<Song>.Filter.Eq(s => s.ArtistName, artistName);
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<Song, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<Song>()
                }));

            var paginationFacet = AggregateFacet.Create("pagination",
                PipelineDefinition<Song, Song>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<Song>.Sort.Ascending(x => x.Title)),
                    PipelineStageDefinitionBuilder.Skip<Song>(pageSize * (page - 1)),
                    PipelineStageDefinitionBuilder.Limit<Song>(pageSize)
                }));

            var aggregation = await this.songCollection.Aggregate()
                .Match(filter)
                .Facet(countFacet, paginationFacet)
                .ToListAsync();

            long count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                .First()
                .Count;

            var totalPages = Math.Ceiling((decimal)(count / pageSize));

            IEnumerable<Song> items = aggregation.First()
                .Facets.First(f => f.Name == "pagination")
                .Output<Song>()
                .ToList();

            return ServiceResult.Succes(new SongPagination { ToTalPages = totalPages, Songs = items });
        }

        public async Task<ServiceResult<long>> SongsStats(int year)
        {
            var filter = Builders<Song>.Filter.Gte(s => s.PublishingYear, year);

            var aggregate = await this.songCollection.Aggregate()
                .Match(filter)
                .Count()
                .FirstAsync();

            long count = aggregate.Count;

            return ServiceResult.Succes(count);
        }
    }
}
