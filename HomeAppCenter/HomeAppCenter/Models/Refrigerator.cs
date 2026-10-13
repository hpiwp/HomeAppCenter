namespace HomeAppCenter.Models;

public class Refrigerator : Appliance
{
    private const decimal CompressorFee = 90m;
    private const decimal FreezerFee = 25m;
    private bool _compressorReplaced;
    private bool _hasFreezer;

    public Refrigerator(string serial, string owner, string brand, string fault,
                        DateTime received, DateTime due, bool compressorReplaced, bool hasFreezer)
        : base(serial, owner, brand, fault, received, due)
    {
        _compressorReplaced = compressorReplaced;
        _hasFreezer = hasFreezer;
    }

    public bool CompressorReplaced { get => _compressorReplaced; set => _compressorReplaced = value; }
    public bool HasFreezer => _hasFreezer;

    public override decimal CalculateRepairCost() =>
        BaseCost() + (_compressorReplaced ? CompressorFee : 0) + (_hasFreezer ? FreezerFee : 0);
}
