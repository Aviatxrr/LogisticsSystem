using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using DispatchRecordSystem.Models;
using Org.BouncyCastle.Tls;

namespace DispatchRecordSystem;

    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void AddRange(ICollection<T> entities);
        void Update(T entity);
        void Delete(T entity);
    }

    public class LogisticsDbContext : DbContext
    {
        private string connectionString;
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<DispatchRecord> DispatchRecords { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CompanyConfig());
            modelBuilder.ApplyConfiguration(new StationConfig());
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new DispatchConfig());
            modelBuilder.ApplyConfiguration(new DriverConfig());
            modelBuilder.ApplyConfiguration(new TrailerConfig());
            modelBuilder.ApplyConfiguration(new ParcelConfig());
            modelBuilder.ApplyConfiguration(new TruckConfig());
            modelBuilder.ApplyConfiguration(new DoorConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using (StreamReader r = new StreamReader("/home" +
                                                     "/pickle" +
                                                     "/Projects" +
                                                     "/PortfolioProjects" +
                                                     "/TruckerStuff" +
                                                     "/LogisticsSystem" +
                                                     "/DispatchRecordSystem" +
                                                     "/ConnectionString.user"))
            {
                connectionString = r.ReadToEnd();
            }

            optionsBuilder.UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString));
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
    public class Repository<T> : IQueryable<T>, IRepository<T> where T : class, IEntity
    {
        private readonly LogisticsDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LogisticsDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T GetById(int id)
        {
            try
            {
                return _dbSet.First(d => d.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new ApplicationException($"{typeof(T)} with Id {id} not found");
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public IEnumerable<T> Find(ISpecification<T> spec)
        {
            foreach (T entity in _dbSet)
            {
                if (spec.IsSatisfiedBy(entity))
                {
                    yield return entity;
                }
            }
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(ICollection<T> entites)
        {
            foreach (T entity in entites)
            {
                Add(entity);
            }
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public Type ElementType => ((IQueryable<T>)_dbSet).ElementType;
        public Expression Expression => ((IQueryable<T>)_dbSet).Expression;
        public IQueryProvider Provider => ((IQueryable<T>)_dbSet).Provider;
        public IEnumerator<T> GetEnumerator() => ((IQueryable<T>)_dbSet).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IQueryable<T>)_dbSet).GetEnumerator();
    }

    public class LibRepository
    {

        public static void Main(string[] args)
        {
            Repository<Truck> tr = new Repository<Truck>(new LogisticsDbContext());
            EntityInfoView<Truck> efv = new EntityInfoView<Truck>(tr.GetById(7201));

            foreach (var key in efv.Properties.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(efv.Properties[key]);
            }

        }
    }

    public class EntityInfoView<T> where T : IEntity
    {
        public T Entity { get; }

        public Dictionary<string, dynamic> Properties;

        public EntityInfoView(T entity)
        {
            Entity = entity;
            Properties = new Dictionary<string, dynamic>();
            foreach (var property in Entity.GetType().GetProperties())
            {
                Properties.Add(property.Name, property.GetValue(Entity));
            }
        }
    }