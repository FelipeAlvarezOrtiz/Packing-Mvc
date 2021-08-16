CREATE TABLE [dbo].[EstadoPedido]
(
	[IdEstado] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NombreEstado] NVARCHAR(50) NOT NULL, 
    [DescripcionEstado] NVARCHAR(150) NULL
)
