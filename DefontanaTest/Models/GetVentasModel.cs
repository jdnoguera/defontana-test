using System.Text.Json.Serialization;

namespace DefontanaTest.Models;

public record GetVentasModel (
    Int64 IdVenta,
    int Total,
    DateTime Fecha,
    string ProductoMayorMontoVenta,
    string LocalMayorVenta,
    string MarcaMayorGanancia,
    string ProductoMasVendidoXLocal
    );

public record VentaMayorModel (
    DateTime Fecha,
    int Total
);

public record ProductoMasVendidoLocal
{
    [JsonPropertyName("ID_Local")]
    public long IdLocal { get; set; }
    public string Local { get; set; }
    public string Producto { get; set; }
    public long CantidadVentas { get; set; }
}