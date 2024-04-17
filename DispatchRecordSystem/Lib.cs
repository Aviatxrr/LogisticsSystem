namespace DispatchRecordSystem;

public interface IEntity
{
    int Id { get; set; }
}

public interface ILocatable
{
    int StationId { get; set; }
}

public interface IHaveCompany
{
    int CompanyId { get; set; }
}
public class Driver : IEntity, ILocatable, IHaveCompany
{
    public int Id { get; set; }
    public string LName{ get; set; }
    public string FName{ get; set; }
    public bool DriverTaken { get; set; }
    public int CompanyId { get; set; }
    public virtual Company Company{ get; set; }
    public int StationId { get; set; }
    public virtual Station Location{ get; set; }
    public DriverStatus Status{ get; set; }

    public enum DriverStatus
    {
        Active,
        Inactive,
        Enroute,
    }
}

public class DispatchRecord : IEntity
{
    public int Id{ get; set; }
    public int OriginId { get; set; }
    public virtual Station Origin{ get; set; }
    public int DestinationId { get; set; }
    public virtual Station Destination{ get; set; }
    public virtual List<Trailer> Trailers{ get; set; }
    public float Mileage{ get; set; }
    public DispatchStatus Status{ get; set; }
    public int? TruckId { get; set; }
    public virtual Truck AssignedTruck{ get; set; }
    
    public DateTime EstArrival{ get; set; }
    public DateTime Departure{ get; set; }

    public enum DispatchStatus
    {
        Unassigned,
        Ready,
        Enroute,
        Arrived,
    }
}

public class Trailer : IEntity, ILocatable
{
    public int Id{ get; set; }
    public int StationId { get; set; }
    public float Weight { get; set; }
    public virtual Station Location{ get; set; }
    public int? DispatchRecordId { get; set; }
    public virtual DispatchRecord Dispatch { get; set; }
    
    public TrailerStatus Status { get; set; }
    
    public enum TrailerStatus
    {
        MT,
        Loading,
        Closed,
        Enroute,
        Unloading,
        Inactive,
    }
}

public class Station : IEntity
{
    public int Id{ get; set; }
    public string Name { get; set; }
    public virtual List<Driver> Drivers{ get; set; }
    public virtual List<Truck> Trucks { get; set; }
    public virtual List<Trailer> Trailers{ get; set; }
    public virtual List<Door> Doors{ get; set; }
    public virtual List<DispatchRecord> Inbound{ get; set; }
    public virtual List<DispatchRecord> Outbound{ get; set; }
    public virtual List<User> Users { get; set; }
    public string Address{ get; set; }
}

public class Parcel : IEntity
{
    public int Id{ get; set; }
    public float Weight{ get; set; }
    public int SenderId { get; set; }
    public virtual Customer Sender{ get; set; }
    public int RecipientId { get; set; }
    public virtual Customer Recipient{ get; set; }
    
    public int OriginId { get; set; }
    public virtual Station Origin{ get; set; }
    public int DestinationId { get; set; }
    public virtual Station Destination{ get; set; }
    public int WaypointId { get; set; }
    public virtual Station Waypoint{ get; set; }
    public ParcelStatus Status{ get; set; }
    public bool NeedSignature{ get; set; }

    public enum ParcelStatus
    {
        LabelCreated,
        Received,
        LoadedAtOrigin,
        Enroute,
        AtWaypoint,
        AtFinalLocation,
        OutForDelivery,
        Delivered,
        DeliveryConfirmed,
        DeliveryFailed,
        DeliveryDenied,
    }
}

public class Door : IEntity, ILocatable
{
    public int Id{ get; set; }
    public int StationId{ get; set; }
    public virtual Station Station { get; set; }
    public DoorType Type{ get; set; }
    public int? TrailerId { get; set; }
    public virtual Trailer Trailer{ get; set; }
    public bool Available{ get; set; }
    public enum DoorType
    {
        Inbound,
        Outbound,
    }    
}

public class Truck : IEntity, ILocatable, IHaveCompany
{
    public int Id{ get; set; }
    public int CompanyId { get; set; }
    public virtual Company Company{ get; set; }
    public int StationId { get; set; }
    public virtual Station Location{ get; set; }
    public int? DriverId { get; set; }
    public virtual Driver Driver{ get; set; }
    public TruckStatus Status{ get; set; }
    

    public enum TruckStatus
    {
        Active,
        Inactive,
        Enroute,
    }

}

public class Company : IEntity
{
    public int Id{ get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public virtual List<Driver> Drivers{ get; set; }
    public virtual List<Truck> Trucks{ get; set; }
}

public class Customer : IEntity
{
    public int Id{ get; set; }
    public string LName{ get; set; }
    public string FName{ get; set; }
    public string Address{ get; set; }
    public string Phone{ get; set; }
    public virtual List<Parcel> Inbound{ get; set; }
    public virtual List<Parcel> Outbound{ get; set; }
}

public class User : IEntity, ILocatable
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int StationId { get; set; }
    public byte[] PwdHash { get; set; }
    public string PwdSalt { get; set; }
    public virtual Station Domistile { get; set; }
}