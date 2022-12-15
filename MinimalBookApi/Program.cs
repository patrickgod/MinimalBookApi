using MinimalBookApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/book", async (DataContext context) =>
    await context.Books.ToListAsync());

app.MapGet("/book/{id}", async (DataContext context, int id) =>
    await context.Books.FindAsync(id) is Book book ?
        Results.Ok(book) :
        Results.NotFound("Sorry, book not found. :("));

app.MapPost("/book", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Books.ToListAsync());
});

app.MapPut("/book/{id}", async (DataContext context, Book updatedBook, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesn't exist.");

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.MapDelete("/book/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound("Sorry, this book doesn't exist.");

    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Books.ToListAsync());
});

app.Run();

public class Book
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
}

