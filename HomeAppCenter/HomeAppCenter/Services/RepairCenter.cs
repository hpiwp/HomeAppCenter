using HomeAppCenter.Enums;
using HomeAppCenter.Events;
using HomeAppCenter.Models;

namespace HomeAppCenter.Services;

public class RepairCenter
{
    private readonly List<Appliance> _appliances = new();

    public event OverdueHandler? OverdueDetected;

    public event StatusChangedHandler? RepairCompleted;

    public IReadOnlyList<Appliance> Appliances => _appliances;

    public void Register(Appliance appliance)
    {
        if (_appliances.Any(a => a.SerialNumber == appliance.SerialNumber))
            throw new InvalidOperationException($"Serial {appliance.SerialNumber} already exists.");

        _appliances.Add(appliance);
        appliance.StatusChanged += OnApplianceStatusChanged;
    }

    public Appliance? FindBySerial(string serial) =>
        _appliances.FirstOrDefault(a => a.SerialNumber == serial);

    public decimal TotalCost() => _appliances.Sum(a => a.CalculateRepairCost());

    public decimal TotalCostByStatus(RepairStatus status) =>
        _appliances.Where(a => a.Status == status).Sum(a => a.CalculateRepairCost());

    public void PrintAll()
    {
        foreach (Appliance a in _appliances)
            Console.WriteLine(a.GetSummary());
    }

    public void CheckOverdue(DateTime today)
    {
        foreach (Appliance a in _appliances.Where(a => a.IsOverdue(today)))
            OverdueDetected?.Invoke(a, (today.Date - a.DueDate.Date).Days);
    }

    private void OnApplianceStatusChanged(Appliance appliance, RepairStatus oldStatus, RepairStatus newStatus)
    {
        if (newStatus == RepairStatus.Repaired)
            RepairCompleted?.Invoke(appliance, oldStatus, newStatus);
    }
}
