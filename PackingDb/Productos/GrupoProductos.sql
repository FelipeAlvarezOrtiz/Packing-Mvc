CREATE TABLE [dbo].[GrupoProductos]
(
	[IdGrupo] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NombreGrupo] NVARCHAR(75) NOT NULL, 
    [Imagen] VARBINARY(MAX) NULL
)
