namespace HomeAppCenter.Models;

public class WashingMachine : Appliance
{
    private const decimal HeavyDrumSurcharge = 30m;
    private double _drumCapacityKg;
    private bool _motorFault;

    public WashingMachine(string serial, string owner, string brand, string fault,
                          DateTime received, DateTime due, double drumCapacityKg, bool motorFault)
        : base(serial, owner, brand, fault, received, due)
    {
        if (drumCapacityKg <= 0) throw new ArgumentException("Capacity must be positive.");
        _drumCapacityKg = drumCapacityKg;
        _motorFault = motorFault;
    }

    public double DrumCapacityKg => _drumCapacityKg;
    public bool MotorFault { get => _motorFault; set => _motorFault = value; }

    // الغسالة: زيادة 10% إذا كان الخلل في المحرك + رسم إضافي للسعات الكبيرة
    public override decimal CalculateRepairCost()
    {
        decimal cost = BaseCost();
        if (_motorFault) cost *= 1.10m;
        if (_drumCapacityKg > 8) cost += HeavyDrumSurcharge;
        return cost;
    }
}
