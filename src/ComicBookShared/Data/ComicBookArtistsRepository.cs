using ComicBookShared.Models;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

namespace ComicBookShared.Data
{
    public class ComicBookArtistsRepository : BaseRepository<ComicBookArtist>
    {
        public ComicBookArtistsRepository(Context context) :base(context)
        {
        }

        public override ComicBookArtist Get(int id, bool includRelatedEntites = true)
        {
            var comicBookArtists = Context.ComicBookArtists.AsQueryable();

            if (includRelatedEntites)
            {
                comicBookArtists = comicBookArtists.Include(cba => cba.Artist)
               .Include(cba => cba.Role)
               .Include(cba => cba.ComicBook.Series);
            }

            return comicBookArtists
                .Where(cba => cba.Id == id)
                .SingleOrDefault();
        }

        public override IList<ComicBookArtist> GetList()
        {
            throw new System.NotImplementedException();
        }
    }
}
