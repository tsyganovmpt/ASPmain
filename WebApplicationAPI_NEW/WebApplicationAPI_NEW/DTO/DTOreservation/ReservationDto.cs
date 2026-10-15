namespace ApiAsp0.DTO.DTOreservation;

public class ReservationDto
{
    public int IdReservation { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int UsersId { get; set; }
    public string? UserLogin { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservationDate { get; set; }
    public bool IsActive { get; set; }
}
