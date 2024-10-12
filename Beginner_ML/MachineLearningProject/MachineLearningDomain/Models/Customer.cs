using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class Customer : BaseModel
{
    public float MonthlyCharges { get; set; }
    public float TotalCharges { get; set; }
    public int Tenure { get; set; }
    public bool HasInternetService { get; set; }
    public bool HasPhoneService { get; set; }
}
