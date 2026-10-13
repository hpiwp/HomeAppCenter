using HomeAppCenter.Models;

namespace HomeAppCenter.Models;

public class AirConditioner : Appliance
{
    private const decimal GasRefillFee = 45m;
    private const decimal OnSiteFee = 20m;
    private int _btu;
    private bool _needsGasRefill;

    public AirConditioner(string serial, string owner, string brand, string fault,
                          DateTime received, DateTime due, int btu, bool needsGasRefill)
        : base(serial, owner, brand, fault, received, due)
    {
        if (btu <= 0) throw new ArgumentException("BTU must be positive.");
        _btu = btu;
        _needsGasRefill = needsGasRefill;
    }

    public int Btu => _btu;
    public bool NeedsGasRefill { get => _needsGasRefill; set => _needsGasRefill = value; }

    // المكيف: تعبئة غاز + رسم خدمة إضافي للمكيفات الكبيرة (أكثر من 18000 BTU)

    public override decimal CalculateRepairCost() =>
        BaseCost() + (_needsGasRefill ? GasRefillFee : 0) + (_btu > 18000 ? OnSiteFee : 0);

    public override string GetSummary() => base.GetSummary() + $" ({_btu} BTU)";
}
