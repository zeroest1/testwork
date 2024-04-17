var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

var requests = new List<Request>();

app.MapGet("/requests", () =>
{
    var now = DateTime.Now;
    return requests.Where(i => !i.IsResolved)
                    .Select(i => new RequestDTO
                    (
                        i.Id,
                        i.Description,
                        i.SubmissionTime,
                        i.ResolutionDueDate,
                        now > i.ResolutionDueDate || now.AddHours(1) > i.ResolutionDueDate
                    ))
                    .OrderByDescending(i => i.ResolutionDueDate);
})
.WithName("Getrequests")
.WithOpenApi();

app.MapPost("/requests", (RequestDTO RequestDTO) =>
{
    var newRequest = new Request
    {
        Id = requests.Any() ? requests.Max(i => i.Id) + 1 : 1,
        Description = RequestDTO.Description,
        SubmissionTime = DateTime.Now,
        ResolutionDueDate = RequestDTO.ResolutionDueDate,
        IsResolved = false
    };

    requests.Add(newRequest);
    return Results.Created($"/requests/{newRequest.Id}", newRequest);
})
.WithName("CreateRequest")
.WithOpenApi();

app.MapPut("/requests/{id}", (int id) =>
{
    var request = requests.FirstOrDefault(i => i.Id == id);
    if (request is null)
    {
        return Results.NotFound();
    }
    request.IsResolved = true;
    return Results.NoContent();
})
.WithName("ResolveRequest")
.WithOpenApi();

app.Run();

record RequestDTO(int Id, string Description, DateTime SubmissionTime, DateTime ResolutionDueDate, bool IsUrgent);