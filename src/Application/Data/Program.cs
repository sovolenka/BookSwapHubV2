// See https://aka.ms/new-console-template for more information
using Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
optionsBuilder.UseNpgsql("<connectionString>");
var context = new PostgresContext(optionsBuilder.Options);

var canConnect = context.Database.CanConnect();

Console.WriteLine(canConnect);
