CREATE TABLE [dbo].[Pedidos]
(
	[GuidPedido] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [IdEmpresa] INT NOT NULL, 
    [FechaSolicitud] DATE NOT NULL, 
    [FechaUltimaActualizacion] DATE NOT NULL, 
    [NumeroSeguimiento] BIGINT NULL, 
    [Observacion] TEXT NULL, 
    [IdEstado] INT NOT NULL, 
    CONSTRAINT [FK_Pedidos_Estado] FOREIGN KEY (IdEstado) REFERENCES EstadoPedido(IdEstado), 
    CONSTRAINT [FK_Pedidos_Empresas] FOREIGN KEY (IdEmpresa) REFERENCES Empresas(IdEmpresa)
)
