using ComicBookShared.Models;
using System.Linq;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBookArtistsRepository
    {
        private Context _context = null;

        public ComicBookArtistsRepository(Context context)
        {
            _context = context;
        }

        public ComicBookArtist Get(int? id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Include(cba => cba.ComicBook.Series)
                .Where(cba => cba.Id == (int)id)
                .SingleOrDefault();
        }

        public void Add(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comicBookArtist = new ComicBookArtist() { Id = id };
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}
