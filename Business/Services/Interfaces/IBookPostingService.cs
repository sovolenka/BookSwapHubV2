using Data.Models;

namespace Business.Services;

public interface IBookPostingService
{
    BookPosting? GetById(long id);
    IQueryable<BookPosting> Get();

    IQueryable<BookPosting> GetByOwnerId(string ownerId);

    BookPosting? Add(BookPosting bookPosting);

    BookPosting? Update(long postingId, BookPosting newBookPosting);

    void Delete(long id);

    IQueryable<BookPosting> Search(string query);
}
