using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PD411_Books.API.Settings;
using PD411_Books.BLL.Services;
using PD411_Books.DAL;
using PD411_Books.DAL.Initializer;
using PD411_Books.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add repositories
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<GenreRepository>();

// Add services
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<GenreService>();

// Add automapper
string automapperKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg5NTE2ODAwIiwiaWF0IjoiMTc1ODAwNDY5MiIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazU4a3hoZXN2ZWI3aDZncms2MHBrYXJrIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.OMUeI0YxSQYUSUYehr5O6yevTWgsGamrSrCFSZ7Sd3fNsl01WU-pr6M6wusxNSxoQ6w8-lqrjOk6gj8KShQQhmvz91wRuRm_rObvAaDQEBRDit7iSUe6J7EH8lDmpqlUuJQ8zN0lCTgIDwaHDaI9h4FcSVy6qmi68oETGI876KCUf5ifCCwDSpZjirIws5XvO6IpQEkCp8FWd2UkTWvrHaaJWFbxOWfKbx_j5AeHPE1o5Piiz7qF6QKX8MzOj44f0yRExRKMCeQSauqRBgO33CooOm0mxbU2-Mx5tb3PPHdaFe7YxPKdRYSJ1TsRn3DELSrxnKsPE11X4eIXYuJh6w";
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = automapperKey;
}, AppDomain.CurrentDomain.GetAssemblies());

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? localDb = builder.Configuration.GetConnectionString("LocalDb");
    string? aivenDb = builder.Configuration.GetConnectionString("AivenDb");
    options.UseNpgsql(aivenDb);
});

builder.Services.AddControllers();

// CORS - ���������� ������ ������ ������ �� ��� ���
string corsName = "allowAll";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(corsName, cfg =>
    {
        cfg.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS - ���������� ������ ������ ������ �� ��� ���
app.UseCors(corsName);

app.UseHttpsRedirection();

// Static files
string root = app.Environment.ContentRootPath;
string storagePath = Path.Combine(root, StaticFilesSettings.StorageDir);

// Books
string booksPath = Path.Combine(storagePath, StaticFilesSettings.BooksDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(booksPath),
    RequestPath = StaticFilesSettings.BookUrl
});

//Authors
string authorsPath = Path.Combine(storagePath, StaticFilesSettings.AuthorsDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(authorsPath),
    RequestPath = StaticFilesSettings.AuthorUrl
});

// Share
string sharePath = Path.Combine(storagePath, StaticFilesSettings.ShareDir);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(sharePath),
    RequestPath = StaticFilesSettings.ShareUrl
});

app.UseAuthorization();

app.MapControllers();

app.SeedAsync().Wait();

app.Run();


