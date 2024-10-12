using MachineLearningDomain.Base;

namespace MachineLearningDomain.Responses;

public class EnergyConsumptionResponse : BaseResponse
{
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public int DayOfWeek { get; set; }
    public float PreviousDayUsage { get; set; }
}
