using HomeAppCenter.Enums;

namespace HomeAppCenter.Interfaces;

/// <summary>العمليات المشتركة بين كل الأجهزة القابلة للإصلاح.</summary>
public interface IRepairable
{
    string SerialNumber { get; }
    RepairStatus Status { get; }

    decimal CalculateRepairCost();
    void StartRepair();
    void CompleteRepair();
    void Deliver();
    string GetSummary();
}
