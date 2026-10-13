namespace HomeAppCenter.Enums;

/// <summary>مراحل دورة حياة الجهاز داخل مركز الصيانة (بالترتيب).</summary>
public enum RepairStatus
{
    Received = 0,
    InProgress = 1,
    Repaired = 2,
    Delivered = 3
}
