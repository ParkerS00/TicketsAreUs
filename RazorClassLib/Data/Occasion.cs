using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RazorClassLib.Data;

[Table("Occasion")]
public partial class Occasion
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    public string? OccasionName { get; set; }

    public DateTime? TimeOfDay { get; set; }

    [OneToMany]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
