
using System.Text.Json;
using DefontanaTest;
using DefontanaTest.Infrastructure.Entities;
using DefontanaTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

//Obtenemos los datos
var datos = GetVentas();
var total30Dias = datos.Sum(v => v.Total);
var cantidadVentas = datos.Count;
var ventaMasAlta = datos.OrderByDescending(v => v.Total)
    .Select(x => new VentaMayorModel(x.Fecha, x.Total))
    .First();

var (_, _, _, productoMayorMontoVenta, localMayorVenta, marcaMayorGanancia, _) = datos[0];
var productoxLocal = JsonSerializer.Deserialize<List<ProductoMasVendidoLocal>>(datos[0].ProductoMasVendidoXLocal);
   
//Imprimimos los resultados
Console.WriteLine($"- El total de ventas de los últimos 30 días: \n Monto: {total30Dias} Cantidad de ventas: {cantidadVentas}");
Console.WriteLine($"- Venta mas alta: \n Fecha: {ventaMasAlta.Fecha}, Monto: {ventaMasAlta.Total}");
Console.WriteLine("- Producto con mayor monto total de ventas: " +productoMayorMontoVenta);
Console.WriteLine("- Local con mayor monto de ventas: "+localMayorVenta);
Console.WriteLine("- Marca con mayor margen de ganancias: "+marcaMayorGanancia);
Console.WriteLine("- Producto que más se vende en cada local: ");
productoxLocal?.ForEach(p=> 
    Console.WriteLine($"Local: {p.Local} \t Producto: {p.Producto} \t Cantidad: {p.CantidadVentas}")
);

List<GetVentasModel> GetVentas()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var optionsBuilder = new DbContextOptionsBuilder<PruebaContext>();
    optionsBuilder.UseSqlServer(connectionString);

    using var context = new PruebaContext(optionsBuilder.Options);
    var query = SqlQuery.GetTotalVentasQuery();
    return context.Set<GetVentasModel>().FromSqlRaw(query).ToList();
} 