namespace DefontanaTest;

public static class SqlQuery
{
    public static string GetTotalVentasQuery() => @"SELECT V.ID_Venta IdVenta, V.Total, V.Fecha,
                            (
                                SELECT TOP 1 P.Nombre
                                FROM VentaDetalle VD
                                    JOIN Venta V ON VD.ID_Venta = V.ID_Venta
                                    JOIN Producto P ON P.ID_Producto = VD.ID_Producto
                                WHERE V.Fecha >= DATEADD(DAY, -30, GETDATE()) AND V.Fecha <= GETDATE()
                                ORDER BY VD.TotalLinea DESC
                            ) AS ProductoMayorMontoVenta,
                            (
                                SELECT TOP 1 L.Nombre
                                FROM Venta V
                                    JOIN Local L ON L.ID_Local = V.ID_Local
                                WHERE v.Fecha >= DATEADD(DAY, -30, GETDATE()) AND V.Fecha <= GETDATE()
                                ORDER BY V.Total DESC
                            ) AS LocalMayorVenta,
                            (
                                SELECT TOP 1 M.Nombre AS Marca
                                FROM VentaDetalle VD
                                    JOIN Producto P ON P.ID_Producto = VD.ID_Producto
                                    JOIN Marca M ON M.ID_Marca = P.ID_Marca
                                GROUP BY M.Nombre
                                ORDER BY MAX((VD.Precio_Unitario - P.Costo_Unitario) * VD.Cantidad) DESC
                            ) AS MarcaMayorGanancia,
                            (
                                SELECT L.ID_Local, L.Nombre AS Local, P.Nombre AS Producto, CantidadVentas
                                FROM (
                                    SELECT L.ID_Local, P.ID_Producto, P.Nombre,
                                        COUNT(*) AS CantidadVentas,
                                        ROW_NUMBER() OVER (PARTITION BY L.ID_Local ORDER BY COUNT(*) DESC) AS RowNum
                                    FROM VentaDetalle VD
                                        JOIN Venta V ON VD.ID_Venta = V.ID_Venta
                                        JOIN Local L ON V.ID_Local = L.ID_Local
                                        JOIN Producto P ON VD.ID_Producto = P.ID_Producto
                                    WHERE v.Fecha >= DATEADD(DAY, -30, GETDATE()) AND V.Fecha <= GETDATE()
                                    GROUP BY L.ID_Local, P.ID_Producto, P.Nombre
                                ) AS Subquery
                                    JOIN Local L ON Subquery.ID_Local = L.ID_Local
                                    JOIN Producto P ON Subquery.ID_Producto = P.ID_Producto
                                WHERE RowNum = 1
                                FOR JSON PATH
                            ) AS ProductoMasVendidoXLocal
                    FROM Venta V
                    WHERE v.Fecha >= DATEADD(DAY, -30, GETDATE()) AND V.Fecha <= GETDATE()
                    ORDER BY V.Fecha DESC;";
}