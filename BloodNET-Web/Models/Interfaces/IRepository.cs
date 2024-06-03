namespace BloodNET_Web.Models.Interfaces
{
    public interface IRepository<TEntity>
    {
        //TEntity FindById(int id);
        public void Add(TEntity entity);
        public void Delete(int id);
        public TEntity Get(int id);
        public List<TEntity> GetAll();
    }
}
