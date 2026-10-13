using HomeAppCenter.Models;
using HomeAppCenter.Enums;
using HomeAppCenter.Models;

namespace HomeAppCenter.Events;
/// <summary>تفويض للإشعار بتغيّر حالة جهاز.</summary>
public delegate void StatusChangedHandler(Appliance appliance, RepairStatus oldStatus, RepairStatus newStatus);
/// <summary>تفويض للإشعار بتأخر تسليم جهاز عن موعده.</summary>
public delegate void OverdueHandler(Appliance appliance, int daysLate);
