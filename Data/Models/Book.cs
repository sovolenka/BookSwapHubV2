using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Index(nameof(Genre), nameof(Language))]
public class Book
{
    [Key]
    public long Id { get; set; }

    [MinLength(2)]
    [MaxLength(2000)]
    public string Name { get; set; } = string.Empty;

    [MinLength(2)]
    [MaxLength(2000)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public int Year { get; set; }

    [MinLength(2)]
    [MaxLength(2000)]
    public string Genre { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public string Publisher { get; set; } = string.Empty;

    [BindProperty]
    public int PublicationYear { get; set; }

    public override string ToString()
    {
        return $"Book: {Name}, Author: {Author}, Year: {Year}, Genre: {Genre}, Language: {Language}, Publisher: {Publisher}, Publication Date: {PublicationYear}";
    }
}
