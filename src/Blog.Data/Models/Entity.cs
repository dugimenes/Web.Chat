namespace Blog.Data.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
        }
        public int Id { get; set; }
    }
}