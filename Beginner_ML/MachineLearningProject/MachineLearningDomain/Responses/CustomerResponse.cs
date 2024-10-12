using MachineLearningDomain.Base;

namespace MachineLearningDomain.Responses;

internal class CustomerResponse : BaseResponse
{
    public float MonthlyCharges { get; set; }
    public float TotalCharges { get; set; }
    public int Tenure { get; set; }
    public bool HasInternetService { get; set; }
    public bool HasPhoneService { get; set; }
}
