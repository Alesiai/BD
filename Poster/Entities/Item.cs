namespace Poster.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}