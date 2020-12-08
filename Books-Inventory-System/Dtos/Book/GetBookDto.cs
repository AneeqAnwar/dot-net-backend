namespace Books_Inventory_System.Dtos.Book
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Linchpin";
        public string Description { get; set; } = "Are you indispensable";
        public string Author { get; set; } = "Seth Godin";
        public double Price { get; set; } = 995;
        public int CategoryId { get; set; } = 1;
    }
}
