using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Index(nameof(ExpireDateTime), nameof(PostDateTime), nameof(OwnerId))]
public class BookPosting
{
    [Key]
    public long Id { get; set; }

    [ForeignKey("OwnerId")]
    public string OwnerId { get; set; } = string.Empty;
    public ApplicationUser Owner { get; set; } = default!;

    [ForeignKey("BookId")]
    public long BookId { get; set; }
    public Book Book { get; set; } = default!;

    public Address Address { get; set; } = default!;

    [MinLength(150)]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public DateTime PostDateTime { get; set; }
    public DateTime ExpireDateTime { get; set; }

    [Url]
    public string? PictureUrl { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, OwnerId: {OwnerId}, Book: {Book}, Address: {Address}, Description: {Description}, PostDateTime: {PostDateTime}, ExpireDateTime: {ExpireDateTime}, PictureUrl: {PictureUrl}";
    }
}