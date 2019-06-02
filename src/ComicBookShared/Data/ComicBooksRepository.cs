using ComicBookShared.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ComicBookShared.Data
{
    public class ComicBooksRepository : BaseRepository<ComicBook>
    {
        public ComicBooksRepository(Context context) : base(context)
        {
        }

        public override IList<ComicBook> GetList()
        {
            return Context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
        }

        public override ComicBook Get(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = Context.ComicBooks.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBooks = comicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role));
            }

            return comicBooks
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public bool ComicBookSeriesHasIssueNumber(ComicBook comicBook)
        {
            return Context.ComicBooks
                    .Any(cb => cb.Id != comicBook.Id &&
                    cb.SeriesId == comicBook.SeriesId &&
                    cb.IssueNumber == comicBook.IssueNumber);
        }

        public bool ComicBookHasArtistRoleCombination(int comicBookId, int artistId, int roleId)
        {
            return Context.ComicBookArtists
                    .Any(cba => cba.ComicBookId == comicBookId &&
                                cba.ArtistId == artistId &&
                                cba.RoleId == roleId);
        }

        public ComicBook GetComicBookDetail(int? id)
        {
            return Context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Where(cb => cb.Id == id)
                    .SingleOrDefault();
        }

        public ComicBook GetComicBookWithSeries(int? id)
        {
            return Context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

    }
}
