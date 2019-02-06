using ComicBookShared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context) => _context = context;

        public ComicBook GetComicBook(int? id)
        {
            return _context.ComicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
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

        public bool ComicBookExists(ComicBook comicBook)
        {
            return _context.ComicBooks
                    .Any(cb => cb.Id != comicBook.Id &&
                    cb.SeriesId == comicBook.SeriesId &&
                    cb.IssueNumber == comicBook.IssueNumber);
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void EditComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void DeleteComicBook(int? id)
        {
            var comicBook = new ComicBook() { Id = (int)id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public IList<Series> GetSeriesList()
        {
            return _context.Series
                .OrderBy(s => s.Title)
                .ToList();
        }

        public IList<Artist> GetArtistsList()
        {
            return _context.Artists
                .OrderBy(a => a.Name)
                .ToList();
        }

        public IList<Role> GetRolesList()
        {
            return _context.Roles
                .OrderBy(r => r.Name)
                .ToList();
        }

        public ComicBookArtist GetComicBookArtist (int? id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Include(cba => cba.ComicBook.Series)
                .Where(cba => cba.Id == (int)id)
                .SingleOrDefault();
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public void DeleteComicBookArtist(int id)
        {
            var comicBookArtist = new ComicBookArtist() { Id = id };
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public bool ComicBookArtistExists(int? comicBookId, int? artistId, int? roleId)
        {
            return _context.ComicBookArtists
                    .Any(cba => cba.ComicBookId == comicBookId &&
                                cba.ArtistId == artistId &&
                                cba.RoleId == roleId);
        }
    }
}
