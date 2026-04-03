using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(option => option.EnableEndpointRouting = true);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Инструментарий"
    });
    string PathFile = Path.Combine(AppContext.BaseDirectory, "KeyPass_Shashin.xml");
    option.IncludeXmlComments(PathFile);
});

var app = builder.Build();
app.UseSwagger();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Инструментарий");
});

app.Run();
