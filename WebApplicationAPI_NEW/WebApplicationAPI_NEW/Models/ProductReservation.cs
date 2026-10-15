namespace ApiAsp0.Models;

public class ProductReservation
{
    public int IdReservation { get; set; }
    public int ProductId { get; set; }
    public int UsersId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservationDate { get; set; }
    public bool IsActive { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
