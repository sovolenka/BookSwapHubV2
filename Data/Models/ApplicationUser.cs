using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public ICollection<BookPosting> BookPosting { get; set; } = default!;

    public ICollection<SwapProposal> SwapProposals { get; set; } = default!;
}
