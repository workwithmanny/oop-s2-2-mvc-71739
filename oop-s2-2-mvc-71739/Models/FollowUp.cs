using System.ComponentModel.DataAnnotations;

namespace oop_s2_2_mvc_71739.Models;

public class FollowUp
{
    public int Id { get; set; }

    [Required]
    public int InspectionId { get; set; }

    public Inspection? Inspection { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }

    [Required]
    public FollowUpStatus Status { get; set; }

    [DataType(DataType.Date)]
    public DateTime? ClosedDate { get; set; }
}
