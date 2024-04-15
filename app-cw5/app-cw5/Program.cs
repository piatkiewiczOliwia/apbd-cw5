using app_cw5.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var _animals = new List<Animal>
{
    new Animal{IdAnimal = 1, Name = "Coco", Category = "Dog", Weight = 4.5, Color = "White"},
    new Animal{IdAnimal = 2, Name = "Rio", Category = "Cat", Weight = 3.5, Color = "Grey"},
    new Animal{IdAnimal = 3, Name = "Pepe", Category = "Dog", Weight = 5.0, Color = "White"},
    new Animal{IdAnimal = 4, Name = "Hank", Category = "Horse", Weight = 110.0, Color = "Brown"}
};

app.MapGet("/api/animals", () => Results.Ok(_animals))
    .WithName("GetAnimals")
    .WithOpenApi();

app.MapGet("/api/animals/{id:int}", (int id) =>
    {
        var animal = _animals.FirstOrDefault(a => a.IdAnimal == id);
        return animal == null ? Results.NotFound($"Animal with id {id} was not found") : Results.Ok(animal);
    })
    .WithName("GetAnimal")
    .WithOpenApi();

app.MapPost("/api/animals", (Animal animal) =>
    {
        _animals.Add(animal);
        return Results.StatusCode(StatusCodes.Status201Created);
    })
    .WithName("CreateAnimal")
    .WithOpenApi();

app.MapPut("/api/animals/{id:int}", (int id, Animal animal) =>
    {
        var animalToEdit = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animalToEdit == null)
        {
            return Results.NotFound($"Animal with id {id} was not found");
        }

        _animals.Remove(animalToEdit);
        _animals.Add(animal);
        return Results.NoContent();
    })
    .WithName("UpdateAnimal")
    .WithOpenApi();

app.MapDelete("/api/animals/{id:int}", (int id) =>
    {
        var animalToDelete = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animalToDelete == null)
        {
            return Results.NoContent();
        }

        _animals.Remove(animalToDelete);
        return Results.NoContent();
    })
    .WithName("DeleteAnimal")
    .WithOpenApi();

var _visits = new List<Visit>
{
    new Visit{Date = DateTime.Now.AddDays(7), Animal = _animals[0], Description = "Vaccination", Price = 60.2},
    new Visit{Date = DateTime.Now.AddDays(1), Animal = _animals[1], Description = "Vaccination", Price = 70.2},
    new Visit{Date = DateTime.Now.AddDays(21), Animal = _animals[2], Description = "Checkup", Price = 29.9},
    new Visit{Date = DateTime.Now.AddDays(3), Animal = _animals[3], Description = "Feeding", Price = 33.0},
    new Visit{Date = DateTime.Now.AddDays(6), Animal = _animals[0], Description = "Treatment", Price = 90.5},
};

app.MapGet("api/animals/{id:int}/visits", (int id) =>
    {
        var animal = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animal == null)
        {
            return Results.NotFound($"Animal with id {id} was not found");
        }

        var visitsForAnimal = new List<Visit>();
        foreach (var visit in _visits)
        {
            if (visit.Animal.IdAnimal == id)
            {
                visitsForAnimal.Add(visit);
            }
        }

        return Results.Ok(visitsForAnimal);
    })
    .WithName("ShowVisitsForAnimal")
    .WithOpenApi();

app.MapPost("api/animals/{id:int}/visits", (int id, Visit visit) =>
    {
        var animal = _animals.FirstOrDefault(a => a.IdAnimal == id);
        if (animal == null)
        {
            return Results.NotFound($"Animal with id {id} was not found");
        }

        _visits.Add(visit);
        return Results.StatusCode(StatusCodes.Status201Created);
    })
    .WithName("AddNewVisitForAnimal")
    .WithOpenApi();

app.Run();



