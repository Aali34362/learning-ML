using Microsoft.ML;
using MLNetCrashCourse.Model;
using MLNetCrashCourse.Response;

namespace MLNetCrashCourse.MicroSoftExamples;

public static class HouseDataPrediction
{
    public static void HouseDataPredictionMain()
    {
        MLContext mlContext = new MLContext();

        // 1. Import or create training data
        HouseData[] houseData = {
               new HouseData() { Size = 1.1F, Price = 1.2F },
               new HouseData() { Size = 1.9F, Price = 2.3F },
               new HouseData() { Size = 2.8F, Price = 3.0F },
               new HouseData() { Size = 3.4F, Price = 3.7F } };

        IDataView trainingData = mlContext.Data.LoadFromEnumerable(houseData);
        /*
         * IDataView is the primary data structure used in ML.NET for data manipulation and processing. 
         * It is an efficient, lazy-loading, columnar format designed to handle large datasets that might not fit in memory all at once. 
         * It abstracts data in a way that allows for efficient transformations and machine learning operations, 
         * without eagerly loading all data into memory. 
         * Here's an in-depth explanation of IDataView, its role, what LoadFromEnumerable does, 
         * and other supported formats and methods to work with data in ML.NET:
         * 1. What is IDataView?
         * IDataView is similar to a table in a relational database. It provides a schema (like column names and types) and the actual data. 
         * It is designed to handle datasets that are too large to fit in memory by operating in a streaming fashion.
         *  Schema: Defines the column names and their types (e.g., TextColumn as string and Label as bool). 
         *  Data: The actual data rows, accessed lazily (i.e., not loaded into memory all at once). 
         *  IDataView is immutable and read-only, meaning you can't change its contents once created, 
         *  but you can apply transformations to produce new IDataView instances.
         *  
         *  2. What Does Data.LoadFromEnumerable Do? 
         *  The LoadFromEnumerable method converts an in-memory collection of objects (like a List<T>) into an IDataView object. 
         *  This is useful when you already have data loaded in memory 
         *  (e.g., from an external source like a JSON file, a database query, or manually created data).
         *  
         *  This code performs the following steps: 
         *  Infers the Schema: 
         *  Reads the properties from the HouseData class and creates columns in the IDataView corresponding to those properties. 
         *  Lazily Loads the Data: 
         *  The data from the houseData list is not loaded into memory immediately. 
         *  Instead, IDataView wraps it in a streaming manner so that the data is processed as needed (e.g., during training or transformations).
         *  
         */

        // 2. Specify data preparation and model training pipeline
        var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Size" })
            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));

        // 3. Train model
        var model = pipeline.Fit(trainingData);

        // 4. Make a prediction
        var size = new HouseData() { Size = 2.5F };
        var price = mlContext.Model.CreatePredictionEngine<HouseData, Prediction>(model).Predict(size);

        Console.WriteLine($"Predicted price for size: {size.Size * 1000} sq ft= {price.Price * 100:C}k");

        // Predicted price for size: 2500 sq ft= $261.98k

    }
}
