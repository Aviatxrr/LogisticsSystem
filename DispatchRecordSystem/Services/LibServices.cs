using System.Security.Cryptography;
using System.Text;
using DispatchRecordSystem;
using DispatchRecordSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DispatchRecordSystem.Services;
public class TruckService(
    Repository<Truck> trucks,
    Repository<DispatchRecord> dispatchRecords
    
    )
{
    public void MoveTruck(Truck truck, Station station)
    {
            truck.StationId = station.Id;
    }
    public void AddDriver(Truck truck, Driver driver)
    {
        if (truck.Status is Truck.TruckStatus.Inactive or Truck.TruckStatus.Enroute)
        {
            throw new ApplicationException("Truck is Inactive/Enroute");
            
        }
        if (driver.Status is Driver.DriverStatus.Inactive or Driver.DriverStatus.Enroute)
        {
            throw new ApplicationException("Driver is Inactive/Enroute");
        }
        if (truck.CompanyId != driver.CompanyId)
        {
            throw new ApplicationException("Truck and driver company id does not match.");
        }
        if (driver.DriverTaken)
        {
            throw new ApplicationException($"Driver: {driver.Id}" +
                                           $" is already in truck" +
                                           $" {trucks.First(t => t.DriverId == driver.Id)}."
                                           );
        }
        truck.DriverId = driver.Id;
        trucks.Update(truck);
    }
    public void RemoveDriver(Truck truck)
    {
        try
        {
            if (truck.DriverId == null)
            {
                throw new ApplicationException("Truck is already empty");
            }

            truck.DriverId = null;
            trucks.Update(truck);
        }
        catch (InvalidOperationException)
        {
            
        }
    }
    public void SetStatus(Truck truck, Truck.TruckStatus status)
    {
        truck.Status = status;
        trucks.Update(truck);
    }

    public void AcceptTrip(Truck truck)
    {
        if (truck.Status
            is Truck.TruckStatus.Inactive
            or Truck.TruckStatus.Enroute)
        {
            throw new ApplicationException("Truck is Inactive/Enroute");
        }
        DispatchRecord nextDispatch = dispatchRecords.GetAll()
            .Where(d => d.TruckId == truck.Id && d.Status is not DispatchRecord.DispatchStatus.Arrived)
            .OrderBy(d => d.Id)
            .First();
        if (nextDispatch.Status is DispatchRecord.DispatchStatus.Enroute)
        {
            throw new ApplicationException("Arrive current trip before accepting a new one");
        }
        nextDispatch.Status = DispatchRecord.DispatchStatus.Enroute;
        nextDispatch.Departure = DateTime.Now;
        dispatchRecords.Update(nextDispatch);
    }

    public void ArriveTrip(Truck truck)
    {
        DispatchRecord currentDispatch = dispatchRecords.GetAll()
            .Where(d => d.TruckId == truck.Id)
            .First(d => d.Status is DispatchRecord.DispatchStatus.Enroute);
        
        currentDispatch.Status = DispatchRecord.DispatchStatus.Arrived;
        
        dispatchRecords.Update(currentDispatch);
    }
}

public class DriverService(Repository<Driver> drivers)
{
    public void MoveDriver(Driver driver, Station station)
    {
        driver.StationId = station.Id;
    }
    public void SetStatus(Driver driver, Driver.DriverStatus status)
    {
        driver.Status = status;
        drivers.Update(driver);
    }
}

public class DispatchRecordService(Repository<DispatchRecord> dispatchRecords)
{
    public DispatchRecord BuildTrip(
        Station origin,
        Station destination
    )
    {
        DispatchRecord dispatch = new DispatchRecord
        {
            OriginId = origin.Id,
            DestinationId = destination.Id,
            Trailers = []
        };
        dispatchRecords.Add(dispatch);
        return dispatchRecords.GetById(dispatch.Id);
    }

    public DispatchRecord AddTrailers(List<Trailer> trailers, DispatchRecord dispatch)
    {
        if (trailers.Count + dispatch.Trailers.Count > 2)
        {
            throw new ApplicationException("Too many trailers");
        }
        if (trailers
            .Any(t => t.Status
                is Trailer.TrailerStatus.Inactive))
        {
            throw new ApplicationException($"" +
                                           $"Trailer(s)" +
                                           $" {trailers.Where(t => t.Status
                                               is Trailer.TrailerStatus.Inactive)}" +
                                           " are Inactive");
        }
        foreach (Trailer trailer in trailers)
        {
            trailer.DispatchRecordId = dispatch.Id;
        }
        dispatchRecords.Update(dispatch);
        return dispatchRecords.GetById(dispatch.Id);
    }

    public void EnqueueDispatch(Truck truck, DispatchRecord dispatch)
    {
        if (truck.Status is Truck.TruckStatus.Inactive)
        {
            throw new ApplicationException("Truck is Inactive");
        }
        dispatch.TruckId = truck.Id;
        dispatch.Status = DispatchRecord.DispatchStatus.Ready;
        dispatchRecords.Update(dispatch);
    }
}

public class StationService(
    Repository<Door> doors,
    Repository<Trailer> trailers
    )
{
    public void DockTrailer(Trailer trailer, Door door)
    {
        if (trailer.StationId != door.StationId)
        {
            throw new ApplicationException(
                $"Trailer is not at station {door.StationId}"
                );
        }
        door.TrailerId = trailer.Id;
        doors.Update(door);
    }
}

public class ParcelService(
    Repository<Customer> customers
    )
{
    
}

public class UserService(
    Repository<User> users
)
{
    public void CreateUser(string userName, string userPwd)
    {
        if (users.GetAll().Select(u => u.UserName).Contains(userName))
        {
            throw new ApplicationException("Username already exists");
        }
        byte[] pwdSaltBytes = generatePwdSalt();
        string pwdSalt = Convert.ToBase64String(pwdSaltBytes);
        string saltedPwd = userPwd + pwdSalt;
        User newUser = new User();
        newUser.UserName = userName;
        newUser.StationId = 40;
        newUser.PwdSalt = pwdSalt;
        newUser.PwdHash = SHA256.HashData(Encoding.ASCII.GetBytes(saltedPwd));
        users.Add(newUser);
        
       
    }

    private byte[] generatePwdSalt()
    {
        int baseAscii = 'A';
        byte[] letters = new byte[20];
        for (int i = 0; i < 20; i++)
        {
            letters[i] = (byte)(baseAscii + Random.Shared.Next(0, 26));
        }
        return letters;
    }

    public bool ConfirmLogin(string userName, string pwd)
    {
        try
        {
            User desiredUser = users.Find(new ByUserName(userName)).First();
            string pwdSalt = desiredUser.PwdSalt;
            string saltedPwd = pwd + pwdSalt;
            byte[] checkPwd = SHA256.HashData(Encoding.ASCII.GetBytes(saltedPwd));
            byte[] userPwd = desiredUser.PwdHash;

            return checkPwd.SequenceEqual(userPwd);
        }
        catch (InvalidOperationException)
        {
            throw new UserNotFoundException(userName);
        }
    }
}
































