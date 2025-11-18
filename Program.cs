using BlazingPizza;

var builder = WebApplication.CreateBuilder(args);

// Configure SQLite DB path from environment variable
var dbPath = Environment.GetEnvironmentVariable("SQLITE_PATH") ?? "pizza.db";
Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

// Configure EF Core with SQLite
builder.Services.AddSqlite<PizzaStoreContext>($"Data Source={dbPath}");
builder.Services.AddScoped<OrderState>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

app.Run();
