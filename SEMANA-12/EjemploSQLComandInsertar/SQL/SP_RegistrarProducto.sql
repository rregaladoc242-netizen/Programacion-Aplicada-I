USE Northwind;
GO

CREATE OR ALTER PROCEDURE dbo.SP_AgregarProductoConCategoria
    @ProductName NVARCHAR(40),
    @UnitPrice MONEY,
    @CategoryName NVARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CategoryID INT;

    SELECT @CategoryID = CategoryID 
    FROM dbo.Categories 
    WHERE CategoryName = @CategoryName;

    IF @CategoryID IS NULL
    BEGIN
        INSERT INTO dbo.Categories (CategoryName)
        VALUES (@CategoryName);
        SET @CategoryID = SCOPE_IDENTITY();
    END

    INSERT INTO dbo.Products (ProductName, UnitPrice, CategoryID)
    VALUES (@ProductName, @UnitPrice, @CategoryID);

    -- Usamos alias sin espacios para evitar errores de enlace en C#
    SELECT 
        p.ProductID AS ID,
        p.ProductName AS Producto,
        p.UnitPrice AS Precio,
        c.CategoryID AS IdCategoria,
        c.CategoryName AS Categoria
    FROM dbo.Products p
    INNER JOIN dbo.Categories c ON p.CategoryID = c.CategoryID;
END;
GO


Select * from Categories