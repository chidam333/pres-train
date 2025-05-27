public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}


public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public T GetById(int id) => _dbSet.Find(id);
    public IEnumerable<T> GetAll() => _dbSet.ToList();
    public void Add(T entity) 
    { 
        _dbSet.Add(entity); 
        _context.SaveChanges(); 
    }
    public void Update(T entity) 
    { 
        _dbSet.Attach(entity); 
        _context.Entry(entity).State = EntityState.Modified; 
        _context.SaveChanges(); 
    }
    public void Delete(T entity) 
    { 
        _dbSet.Remove(entity); 
        _context.SaveChanges(); 
    }
}
