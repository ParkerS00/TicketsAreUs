using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RazorClassLib.Data;

[Table("Ticket")]
public partial class Ticket
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [ForeignKey(typeof(Occasion))]
    public int? OccasionId { get; set; }

    public Guid? Guid { get; set; }

    public bool? IsUsed { get; set; }

    [ManyToOne]
    public virtual Occasion? Occasion { get; set; }
}
