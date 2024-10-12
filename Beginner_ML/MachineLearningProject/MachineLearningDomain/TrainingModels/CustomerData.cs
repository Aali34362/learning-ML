namespace MachineLearningDomain.TrainingModels;

public class CustomerData
{
    public float MonthlyCharges { get; set; }     // Monthly bill of the customer
    public float TotalCharges { get; set; }       // Total amount paid by the customer
    public int Tenure { get; set; }               // Number of months the customer has stayed with the provider
    public bool HasInternetService { get; set; }  // Whether the customer has internet service
    public bool HasPhoneService { get; set; }     // Whether the customer has phone service
}
