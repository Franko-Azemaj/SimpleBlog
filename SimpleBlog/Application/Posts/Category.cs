namespace SimpleBlog.Application.Posts
{
    public class Category
    {
        public static Category Create(int id, string name)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(name);
            return new Category(id, name);
        }

        public int Id { get; set; } 
        public string Name { get; set; }

        private Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }  
}