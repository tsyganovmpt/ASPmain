using ApiAsp0.DTO.DTOreservation;
using ApiAsp0.Models;
using ApiAsp0.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApiAsp0.Services;

public sealed record ReservationOperationResult(bool Success, bool NotFound, string? Error, ReservationDto? Reservation = null);

public interface IReservationService
{
    Task<ReservationOperationResult> ReserveAsync(CreateReservationDto dto, CancellationToken ct = default);
    Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ReservationOperationResult> CancelAsync(int id, CancellationToken ct = default);
}

public sealed class ReservationService : IReservationService
{
    private readonly IRepository<Product> _products;
    private readonly IRepository<User> _users;
    private readonly IRepository<ProductReservation> _reservations;
    private readonly IMapper _mapper;

    public ReservationService(
        IRepository<Product> products,
        IRepository<User> users,
        IRepository<ProductReservation> reservations,
        IMapper mapper)
    {
        _products = products;
        _users = users;
        _reservations = reservations;
        _mapper = mapper;
    }

    public async Task<ReservationOperationResult> ReserveAsync(CreateReservationDto dto, CancellationToken ct = default)
    {
        if (dto.Quantity <= 0)
            return new(false, false, "Количество для резервирования должно быть больше нуля.");

        var product = await _products.GetByIdAsync(dto.ProductId, ct);
        if (product is null)
            return new(false, true, "Товар не найден.");

        var user = await _users.GetByIdAsync(dto.UsersId, ct);
        if (user is null)
            return new(false, true, "Пользователь не найден.");

        if (!user.IsActive)
            return new(false, false, "Нельзя резервировать товар для неактивного пользователя.");

        if (product.StockQuantity < dto.Quantity)
            return new(false, false, $"Недостаточно товара на складе. Доступно: {product.StockQuantity}.", null);

        product.StockQuantity -= dto.Quantity;
        var reservation = new ProductReservation
        {
            ProductId = product.IdProduct,
            UsersId = user.IdUsers,
            Quantity = dto.Quantity,
            ReservationDate = DateTime.Now,
            IsActive = true
        };

        await _reservations.AddAsync(reservation, ct);
        // Все репозитории используют один scoped AspfisrtContext:
        // остаток и запись резервирования сохраняются одним вызовом.
        await _reservations.SaveChangesAsync(ct);

        var result = await _reservations.Query("Product", "User")
            .FirstAsync(r => r.IdReservation == reservation.IdReservation, ct);
        return new(true, false, null, _mapper.Map<ReservationDto>(result));
    }

    public async Task<ReservationDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var reservation = await _reservations.Query("Product", "User")
            .FirstOrDefaultAsync(r => r.IdReservation == id, ct);
        return reservation is null ? null : _mapper.Map<ReservationDto>(reservation);
    }

    public async Task<ReservationOperationResult> CancelAsync(int id, CancellationToken ct = default)
    {
        var reservation = await _reservations.Query("Product", "User")
            .FirstOrDefaultAsync(r => r.IdReservation == id && r.IsActive, ct);
        if (reservation is null)
            return new(false, true, "Активное резервирование с таким ID не найдено.");

        var product = await _products.GetByIdAsync(reservation.ProductId, ct);
        if (product is null)
            return new(false, true, "Товар, связанный с резервированием, не найден.");

        product.StockQuantity += reservation.Quantity;
        reservation.IsActive = false;
        // Возврат остатка и закрытие события сохраняются одним вызовом.
        await _reservations.SaveChangesAsync(ct);
        return new(true, false, null, _mapper.Map<ReservationDto>(reservation));
    }
}
