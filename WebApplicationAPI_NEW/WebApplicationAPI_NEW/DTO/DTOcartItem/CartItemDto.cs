namespace ApiAsp0.DTO.DTOcartItem
{
    public class CartItemDto
    {
        public int IdCartItem { get; set; }

        public int ProductId { get; set; }

        public int CartId { get; set; }

        public int Quantity { get; set; }
            public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? CartOwnerLogin { get; set; }
    }
}
