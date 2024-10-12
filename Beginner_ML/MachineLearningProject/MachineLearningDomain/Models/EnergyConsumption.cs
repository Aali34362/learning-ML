using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class EnergyConsumption : BaseModel
{
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public int DayOfWeek { get; set; }
    public float PreviousDayUsage { get; set; }
}
