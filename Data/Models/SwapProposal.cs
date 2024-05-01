using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class SwapProposal
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey("SenderId")]
    public string SenderId { get; set; } = string.Empty;
    public ApplicationUser Sender { get; set; } = default!;
    
    [ForeignKey("ReceiverId")]
    public string ReceiverId { get; set; } = string.Empty;
    public ApplicationUser Receiver { get; set; } = default!;
    
    public long BookPostingId { get; set; }
    public BookPosting BookPosting { get; set; } = default!;
    
    public string Message { get; set; } = string.Empty;
    
    public ProposalStatus ProposalStatus { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, SenderId: {SenderId}, ReceiverId: {ReceiverId}, BookPostingId: {BookPostingId}, Message: {Message}, ProposalStatus: {ProposalStatus}";
    }
}

public enum ProposalStatus
{
    Pending,
    Accepted,
    Rejected
}