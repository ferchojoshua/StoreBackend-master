IF OBJECT_ID('GetProductosInventario', 'P') IS NOT NULL
    DROP PROCEDURE GetProductosInventario;
GO

CREATE PROCEDURE GetProductosInventario
    @ProductID INT = NULL,
    @StoreID INT = NULL,
    @TipoNegocioID INT = NULL
AS
BEGIN
    -- Crear una tabla temporal para calcular los totales
    CREATE TABLE #TempTotales (
        ProductoID INT,
        TotalDetalle DECIMAL(18, 2),
        TotalMayor DECIMAL(18, 2),
        CostoTotal DECIMAL(18, 2)
    );

    INSERT INTO #TempTotales (ProductoID, TotalDetalle, TotalMayor, CostoTotal)
    SELECT
        prod.Id AS ProductoID,
        SUM(ext.Existencia * ext.PrecioVentaDetalle) AS TotalDetalle,
        SUM(ext.Existencia * ext.PrecioVentaMayor) AS TotalMayor,
        SUM(ext.Existencia * ext.PrecioCompra) AS CostoTotal
    FROM dbo.Productos prod
    INNER JOIN dbo.Existences ext ON prod.Id = ext.ProductoId
    WHERE
        (@ProductID IS NULL OR prod.Id = @ProductID)
    GROUP BY prod.Id;

    SELECT
        prod.Id,
        prod.Description AS nombre_producto,
        alm.Name AS nombre_almacen,
        ext.Existencia AS existencia,
        ext.PrecioVentaDetalle AS precio_detalle,
        ext.PrecioVentaMayor AS precio_xmayor,
        ext.PrecioCompra AS costo_unitario,
        tt.TotalDetalle,
        tt.TotalMayor,
        tt.CostoTotal
    FROM dbo.Productos prod
    INNER JOIN dbo.Familias fam ON prod.FamiliaId = fam.Id
    INNER JOIN dbo.TipoNegocios tng ON prod.TipoNegocioId = tng.Id
    INNER JOIN dbo.Existences ext ON prod.Id = ext.ProductoId
    INNER JOIN dbo.Almacen alm ON ext.AlmacenId = alm.Id
    LEFT JOIN #TempTotales tt ON prod.Id = tt.ProductoID
    WHERE
        (@ProductID IS NULL OR prod.Id = @ProductID)
        AND (@StoreID IS NULL OR alm.Id = @StoreID)
        AND (@TipoNegocioID IS NULL OR tng.Id = @TipoNegocioID);

    -- Eliminar la tabla temporal
    DROP TABLE #TempTotales;
END;

-- Para ejecutarlo.
-- EXEC GetProductosInventario @ProductID = NULL, @StoreID = NULL, @TipoNegocioID = NULL;
