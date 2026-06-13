    using ExternalApiCache.Repositories;

    var builder = WebApplication.CreateBuilder(args);

    // SERVICES
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Dependency Injection
    builder.Services.AddScoped<PostRepository>();

    // HttpClient for external API calls
    builder.Services.AddHttpClient();

    var app = builder.Build();

    // GLOBAL ERROR HANDLING
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Something went wrong on the server. Please try again later."
            });
        });
    });

    // PIPELINE
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();