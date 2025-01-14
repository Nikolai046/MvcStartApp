using System.ComponentModel.DataAnnotations.Schema;

namespace MvcStartApp.Models.Entities;

[Table("Requests")]
public class Request
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Url { get; set; }
}