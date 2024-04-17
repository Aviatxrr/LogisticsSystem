using Bogus;
using Bogus.DataSets;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace DispatchRecordSystem;


public class DatabaseTestSeeder
{
    public T Generate<T>() where T : IEntity, new()
    {
        Faker faker = new Faker();
        T entity = new T();
        Repository<Company> cr = new Repository<Company>(new LogisticsDbContext());
        IEnumerable<Company> cl = new List<Company>();
        Repository<Station> sr = new Repository<Station>(new LogisticsDbContext());
        IEnumerable<Station> sl = new List<Station>();
        Repository<Driver> dr = new Repository<Driver>(new LogisticsDbContext());
        IEnumerable<Driver> dl = new List<Driver>();
        Repository<Truck> tr = new Repository<Truck>(new LogisticsDbContext());
        IEnumerable<Truck> tl = new List<Truck>();

        foreach (var property in typeof(T).GetProperties())
        {
            switch(property.Name)
            {
                case "LName":
                    property.SetValue(entity, faker.Name.LastName());
                    break;
                case "FName":
                    property.SetValue(entity, faker.Name.FirstName());
                    break;
                case "Name":
                    if (typeof(T) == typeof(Company))
                    {
                        property.SetValue(entity, faker.Company.CompanyName());
                    }
                    else if (typeof(T) == typeof(Station))
                    {
                        property.SetValue(entity, GetRandomLetters(4));
                    }
                    break;
                case "Address":
                    property.SetValue(entity, faker.Address.StreetAddress() + ", " +
                        faker.Address.City() + ", " +
                        faker.Address.State() + "  " +
                        faker.Address.ZipCode()
                        );
                    break;
                case "Phone":
                    property.SetValue(entity, faker.Phone.PhoneNumber());
                    break;
                case "CompanyId":
                    cl = cr.GetAll();
                    property.SetValue(entity, faker.PickRandom(cl).Id);
                    break;
                case "StationId":
                    sl = sr.GetAll();
                    property.SetValue(entity, faker.PickRandom(sl).Id);
                    break;
                case "Available":
                    property.SetValue(entity, true);
                    break;
                case "Status":
                    if (property.PropertyType.IsEnum)
                    {
                        var statusValues = Enum.GetValues(property.PropertyType);
                        property.SetValue(entity, statusValues.GetValue(0));
                    }
                    break;
                case "Type":
                    if (property.PropertyType.IsEnum)
                    {
                        var statusValues = Enum.GetValues(property.PropertyType);
                        property.SetValue(entity, faker.PickRandom(statusValues));
                    }
                    break;
                case "Weight":
                    property.SetValue(entity, (float)0.0);
                    break;
            }
            Console.WriteLine($"{entity} :: {property.Name} :: {property.GetValue(entity)}");
        }

        return entity;
    }

    public List<T> GetSeed<T>(int amount) where T : IEntity, new()
    {
        List<T> Seed = new List<T>();
        for(int i = 0; i < amount; i++)
        {
            T entity = Generate<T>();
            Seed.Add(entity);
        }

        return Seed;
    }
    
    public static string GetRandomLetters(int n)
    {
        int baseAscii = 'A'; // Or 'a' for lowercase
        char[] letters = new char[n];
        for (int i = 0; i < n; i++)
        {
            letters[i] = (char)(baseAscii + Random.Shared.Next(0, 26));
        }
        return new string(letters);
    }
}