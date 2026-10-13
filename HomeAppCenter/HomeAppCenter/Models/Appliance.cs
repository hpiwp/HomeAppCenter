using HomeAppCenter.Enums;
using HomeAppCenter.Events;
using HomeAppCenter.Interfaces;
using HomeAppCenter.Utilities;

namespace HomeAppCenter.Models;

/// <summary>الفئة المجردة: أساس كل الأجهزة، تطبّق الواجهة IRepairable.</summary>

public abstract class Appliance : IRepairable
{
    // أجر ساعة العمل
    protected const decimal HourlyRate = 12m;

    private static readonly int[] StatusCounters = new int[Enum.GetValues<RepairStatus>().Length];

    private readonly string _serialNumber;
    private readonly DateTime _receivedDate;
    private string _ownerName;
    private string _brand;
    private string _faultDescription;
    private DateTime _dueDate;
    private decimal _partsCost;
    private decimal _laborHours;
    private RepairStatus _status;

    /// <summary>يُطلق عند أي تغيّر في حالة الجهاز.</summary>

    public event StatusChangedHandler? StatusChanged;

    protected Appliance(string serialNumber, string ownerName, string brand,
                        string faultDescription, DateTime receivedDate, DateTime dueDate)
    {
        if (!Validator.IsValidSerial(serialNumber))
            throw new ArgumentException("Serial must look like ABC-1234.", nameof(serialNumber));
        if (!Validator.IsValidDueDate(receivedDate, dueDate))
            throw new ArgumentException("Due date cannot be before received date.", nameof(dueDate));

        _serialNumber = serialNumber;
        _receivedDate = receivedDate;
        _dueDate = dueDate;
        _ownerName = string.Empty;
        _brand = string.Empty;
        _faultDescription = string.Empty;
        OwnerName = ownerName;
        Brand = brand;
        FaultDescription = faultDescription;

        _status = RepairStatus.Received;
        StatusCounters[(int)_status]++;
    }

    public string SerialNumber => _serialNumber;
    public DateTime ReceivedDate => _receivedDate;
    public RepairStatus Status => _status;

    public string OwnerName
    {
        get => _ownerName;
        set => _ownerName = Validator.IsValidName(value)
            ? value.Trim() : throw new ArgumentException("Invalid owner name.");
    }

    public string Brand
    {
        get => _brand;
        set => _brand = Validator.IsValidName(value)
            ? value.Trim() : throw new ArgumentException("Invalid brand.");
    }

    public string FaultDescription
    {
        get => _faultDescription;
        set => _faultDescription = Validator.IsValidName(value)
            ? value.Trim() : throw new ArgumentException("Invalid fault description.");
    }

    public DateTime DueDate
    {
        get => _dueDate;
        set => _dueDate = Validator.IsValidDueDate(_receivedDate, value)
            ? value : throw new ArgumentException("Due date cannot be before received date.");
    }

    public decimal PartsCost
    {
        get => _partsCost;
        set => _partsCost = Validator.IsNonNegative(value)
            ? value : throw new ArgumentException("Parts cost cannot be negative.");
    }

    public decimal LaborHours
    {
        get => _laborHours;
        set => _laborHours = Validator.IsNonNegative(value)
            ? value : throw new ArgumentException("Labor hours cannot be negative.");
    }

    // ----- عدد الأجهزة حسب الحالة -----

    public static int PendingCount =>
        StatusCounters[(int)RepairStatus.Received] + StatusCounters[(int)RepairStatus.InProgress];

    public static int RepairedCount =>
        StatusCounters[(int)RepairStatus.Repaired] + StatusCounters[(int)RepairStatus.Delivered];

    public static int CountByStatus(RepairStatus status) => StatusCounters[(int)status];

    // -----  كل جهاز يحسب تكلفته بطريقته -----

    public abstract decimal CalculateRepairCost();

    protected decimal BaseCost() => _partsCost + _laborHours * HourlyRate;

    public void StartRepair() => ChangeStatus(RepairStatus.InProgress);
    public void CompleteRepair() => ChangeStatus(RepairStatus.Repaired);
    public void Deliver() => ChangeStatus(RepairStatus.Delivered);

    public bool IsOverdue(DateTime today) => _status < RepairStatus.Repaired && today.Date > _dueDate.Date;

    public virtual string GetSummary() =>
        $"[{_serialNumber}] {GetType().Name,-15} {_brand,-8} Owner: {_ownerName,-8} " +
        $"Status: {_status,-10} Cost: {CalculateRepairCost():C}";

    // تغيير الحالة مسموح فقط بخطوة واحدة للأمام
    private void ChangeStatus(RepairStatus newStatus)
    {
        if ((int)newStatus != (int)_status + 1)
            throw new InvalidOperationException($"Cannot move from {_status} to {newStatus}.");

        RepairStatus old = _status;
        StatusCounters[(int)old]--;
        StatusCounters[(int)newStatus]++;
        _status = newStatus;
        StatusChanged?.Invoke(this, old, newStatus);
    }
}
