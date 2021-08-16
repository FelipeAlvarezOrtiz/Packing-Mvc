CREATE TABLE [dbo].[Productos]
(
	[IdProducto] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NombreProducto] NVARCHAR(100) NOT NULL, 
    [Vigente] BIT NOT NULL DEFAULT 1, 
    [IdGrupo] INT NOT NULL, 
    [IdFormato] INT NOT NULL, 
    [IdPresentacion] INT NOT NULL, 
    CONSTRAINT [FK_Productos_GrupoProductos] FOREIGN KEY (IdGrupo) REFERENCES GrupoProductos(IdGrupo), 
    CONSTRAINT [FK_Productos_Formatos] FOREIGN KEY (IdFormato) REFERENCES Formatos(IdFormato), 
    CONSTRAINT [FK_Productos_Presentaciones] FOREIGN KEY (IdPresentacion) REFERENCES Presentaciones(IdPresentacion)
)

GO

CREATE INDEX [IndexId] ON [dbo].[Productos] (IdProducto)
