namespace Api.Domain.Interfaces
{
    public interface IEntityBase
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}