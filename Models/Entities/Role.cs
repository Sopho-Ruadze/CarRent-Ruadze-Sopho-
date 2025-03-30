namespace CarRental_API.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public Role(string name)
        {
            Name = name;
        }
    }
}