using ComicBookShared.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComicBookShared.Data
{
    public class ComicBooksRepository
    {
        private Context _context = null;

        public ComicBooksRepository(Context context) => _context = context;

        public IList<ComicBook> GetList()
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
        }

        public ComicBook Get(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = _context.ComicBooks.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBooks = comicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role));
            }

            return _context.ComicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public void Add(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void Update(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            var comicBook = new ComicBook() { Id = (int)id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public bool ComicBookSeriesHasIssueNumber(ComicBook comicBook)
        {
            return _context.ComicBooks
                    .Any(cb => cb.Id != comicBook.Id &&
                    cb.SeriesId == comicBook.SeriesId &&
                    cb.IssueNumber == comicBook.IssueNumber);
        }

        public bool ComicBookHasArtistRoleCombination(int comicBookId, int artistId, int roleId)
        {
            return _context.ComicBookArtists
                    .Any(cba => cba.ComicBookId == comicBookId &&
                                cba.ArtistId == artistId &&
                                cba.RoleId == roleId);
        }

        public ComicBook GetComicBookDetail(int? id)
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Where(cb => cb.Id == id)
                    .SingleOrDefault();
        }

        public ComicBook GetComicBookWithSeries(int? id)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

    }
}
