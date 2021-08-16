CREATE TABLE [dbo].[DetallesPedidos]
(
	[IdDetalle] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GuidSolicitudCabecera] UNIQUEIDENTIFIER NOT NULL, 
    [IdProducto] INT NOT NULL, 
    [Cantidad] INT NOT NULL, 
    CONSTRAINT [FK_DetallesPedidos_Productos] FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto), 
    CONSTRAINT [FK_DetallesPedidos_Pedido] FOREIGN KEY (GuidSolicitudCabecera) REFERENCES Pedidos(GuidPedido)
)
