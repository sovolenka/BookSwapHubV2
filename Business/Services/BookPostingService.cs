using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class BookPostingService : IBookPostingService
{
    private readonly PostgresContext _context;

    public BookPostingService(PostgresContext context)
    {
        _context = context;
    }

    public IQueryable<BookPosting> Get()
    {
        return _context
            .BookPostings!
            .Include(x => x.Owner)
            .Include(x => x.Book)
            .Include(x => x.Address);
    }

    public BookPosting? GetById(long id)
    {
        return Get()
            .FirstOrDefault(x => x.Id == id);
    }

    public IQueryable<BookPosting> GetByOwnerId(string ownerId)
    {
        return Get()
            .Where(x => x.OwnerId == ownerId);
    }

    public BookPosting? Add(BookPosting bookPosting)
    {
        var p = _context.BookPostings!.Add(bookPosting);
        _context.SaveChanges();
        return p.Entity;
    }

    public BookPosting? Update(long postingId, BookPosting newBookPosting)
    {
        var existingBookPosting = Get()
            .FirstOrDefault(x => x.Id == postingId);

        if (existingBookPosting != null)
        {
            existingBookPosting.Description = newBookPosting.Description;
            existingBookPosting.PostDateTime = DateTime.UtcNow;
            existingBookPosting.ExpireDateTime = DateTime.UtcNow.AddDays(30);
            existingBookPosting.PictureUrl = newBookPosting.PictureUrl;

            existingBookPosting.Address.Street = newBookPosting.Address.Street;
            existingBookPosting.Address.City = newBookPosting.Address.City;
            existingBookPosting.Address.State = newBookPosting.Address.State;
            existingBookPosting.Address.Zip = newBookPosting.Address.Zip;
            existingBookPosting.Address.Country = newBookPosting.Address.Country;

            existingBookPosting.Book.Name = newBookPosting.Book.Name;
            existingBookPosting.Book.Author = newBookPosting.Book.Author;
            existingBookPosting.Book.Year = newBookPosting.Book.Year;
            existingBookPosting.Book.Genre = newBookPosting.Book.Genre;
            existingBookPosting.Book.Language = newBookPosting.Book.Language;
            existingBookPosting.Book.Publisher = newBookPosting.Book.Publisher;
            existingBookPosting.Book.PublicationYear = newBookPosting.Book.PublicationYear;

            _context.BookPostings!.Update(existingBookPosting);

            _context.SaveChanges();
        }
        return existingBookPosting;
    }

    public void Delete(long id)
    {
        var bookPosting = _context
            .BookPostings!
            .FirstOrDefault(x => x.Id == id);
        if (bookPosting != null)
        {
            _context.BookPostings!.Remove(bookPosting);
            _context.SaveChanges();
        }
    }

    public IQueryable<BookPosting> Search(string query)
    {
        query = query.ToLower();
        return Get()
            .Where(x => x.Book.Name.ToLower().Contains(query) || x.Book.Author.ToLower().Contains(query));
    }
}