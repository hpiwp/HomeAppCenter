using HomeAppCenter.Enums;
using HomeAppCenter.Models;
using HomeAppCenter.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var today = new DateTime(2026, 11, 1);
var center = new RepairCenter();

center.RepairCompleted += (a, oldS, newS) =>
    Console.WriteLine($">> NOTIFY: {a.SerialNumber} repair completed. Call {a.OwnerName}.");
center.OverdueDetected += (a, days) =>
    Console.WriteLine($">> ALERT: {a.SerialNumber} is {days} day(s) overdue!");

var fridge = new Refrigerator("REF-1001", "Sara", "LG", "Not cooling", today.AddDays(-10), today.AddDays(-3), true, true);
var washer = new WashingMachine("WSH-2001", "Omar", "Samsung", "Drum stuck", today.AddDays(-2), today.AddDays(5), 9, true);
var ac = new AirConditioner("ACN-3001", "Lina", "Gree", "Weak cooling", today.AddDays(-1), today.AddDays(6), 24000, true);

fridge.LaborHours = 3; fridge.PartsCost = 120;
washer.LaborHours = 2; washer.PartsCost = 60;
ac.LaborHours = 1.5m; ac.PartsCost = 35;

center.Register(fridge);
center.Register(washer);
center.Register(ac);

Console.WriteLine("=== All appliances ===");
center.PrintAll();

fridge.StartRepair();
washer.StartRepair();
fridge.CompleteRepair();

Console.WriteLine("\n=== Overdue check ===");
center.CheckOverdue(today);

Console.WriteLine("\n=== Statistics ===");
Console.WriteLine($"Pending : {Appliance.PendingCount}");
Console.WriteLine($"Repaired: {Appliance.RepairedCount}");
Console.WriteLine($"Total cost (all)     : {center.TotalCost():C}");
Console.WriteLine($"Total cost (Repaired): {center.TotalCostByStatus(RepairStatus.Repaired):C}");

try { _ = new AirConditioner("bad", "X1", "LG", "Fault", today, today, 12000, false); }
catch (ArgumentException ex) { Console.WriteLine($"\nValidation works: {ex.Message}"); }
