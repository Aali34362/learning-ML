namespace MachineLearningDomain.TrainingModels;

public class EnergyConsumptionData
{
    public float Temperature { get; set; }      // Outside temperature
    public float Humidity { get; set; }         // Humidity level
    public int DayOfWeek { get; set; }          // Day of the week
    public float PreviousDayUsage { get; set; } // Energy consumption from the previous day
}
