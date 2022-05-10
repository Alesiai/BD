namespace Poster.Entities
{
    public class ItemsInOrder
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public Item Item { get; set; }
        public int Count { get; set; }
    }
}