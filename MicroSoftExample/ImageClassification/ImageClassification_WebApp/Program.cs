using ImageClassification_WebApp.ImageClassification.Shared.DataModels;
using Microsoft.Extensions.ML;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddControllers();
builder.Services.AddRazorPages();

/////////////////////////////////////////////////////////////////////////////
// Register the PredictionEnginePool as a service in the IoC container for DI.
//
builder.Services.AddPredictionEnginePool<InMemoryImageData, ImagePrediction>()
        .FromFile(builder.Configuration["MLModel:MLModelFilePath"]);

// (Optional) Get the pool to initialize it and warm it up.       
//WarmUpPredictionEnginePool(services);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();



static void WarmUpPredictionEnginePool(IServiceCollection services)
{
    //#1 - Simply get a Prediction Engine
    var predictionEnginePool = services.BuildServiceProvider().GetRequiredService<PredictionEnginePool<InMemoryImageData, ImagePrediction>>();
    var predictionEngine = predictionEnginePool.GetPredictionEngine();
    predictionEnginePool.ReturnPredictionEngine(predictionEngine);

    // #2 - Predict
    // Get a sample image
    //
    //Image img = Image.FromFile(@"TestImages/BlackRose.png");
    //byte[] imageData;
    //IFormFile imageFile;
    //using (MemoryStream ms = new MemoryStream())
    //{
    //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
    //    //To byte[] (#1)
    //    imageData = ms.ToArray();

    //    //To FormFile (#2)
    //    imageFile = new FormFile((Stream)ms, 0, ms.Length, "BlackRose", "BlackRose.png");
    //}

    //var imageInputData = new InMemoryImageData(image: imageData, label: null, imageFileName: null);

    //// Measure execution time.
    //var watch = System.Diagnostics.Stopwatch.StartNew();

    //var prediction = predictionEnginePool.Predict(imageInputData);

    //// Stop measuring time.
    //watch.Stop();
    //var elapsedMs = watch.ElapsedMilliseconds;
}

static string GetAbsolutePath(string relativePath)
{
    var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
    string assemblyFolderPath = _dataRoot.Directory.FullName;

    string fullPath = Path.Combine(assemblyFolderPath, relativePath);
    return fullPath;
}
