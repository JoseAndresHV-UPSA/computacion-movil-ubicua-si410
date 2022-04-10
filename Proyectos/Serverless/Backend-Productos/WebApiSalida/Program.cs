
using Newtonsoft.Json;
using WebApiSalida.Models;
using WebApiSalida.Services;

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

app.MapGet("/products/get", async () =>
{
    string message = await QueueReceiver.Receive();
    if (string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new Response<string>
        {
            Data = null,
            Method = "GET",
            Success = false
        });
    }

    var result = JsonConvert.DeserializeObject<Response<List<Product>>>(message);
    return Results.Ok(result);
});

app.MapGet("/products/getbyid", async () =>
{
    string message = await QueueReceiver.Receive();
    if (string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new Response<string>
        {
            Data = null,
            Method = "GETBYID",
            Success = false
        });
    }

    var result = JsonConvert.DeserializeObject<Response<Product>>(message);
    return Results.Ok(result);
});

app.MapGet("/products/post", async () =>
{
    string message = await QueueReceiver.Receive();
    if (string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new Response<string>
        {
            Data = null,
            Method = "POST",
            Success = false
        });
    }

    var result = JsonConvert.DeserializeObject<Response<Product>>(message);
    return Results.Ok(result);
});

app.MapGet("/products/put", async () =>
{
    string message = await QueueReceiver.Receive();
    if (string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new Response<string>
        {
            Data = null,
            Method = "PUT",
            Success = false
        });
    }

    var result = JsonConvert.DeserializeObject<Response<Product>>(message);
    return Results.Ok(result);
});

app.MapGet("/products/delete", async () =>
{
    string message = await QueueReceiver.Receive();
    if (string.IsNullOrEmpty(message))
    {
        return Results.BadRequest(new Response<string>
        {
            Data = null,
            Method = "DELETE",
            Success = false
        });
    }

    var result = JsonConvert.DeserializeObject<Response<string>>(message);
    return Results.Ok(result);
});

app.Run();