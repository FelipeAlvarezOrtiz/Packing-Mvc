CREATE PROCEDURE [dbo].[InsertarCabeceraPedido]
	@guidPedido uniqueidentifier,
	@idEmpresa int,
	@observacion text = null,
	@idEstado int
AS
BEGIN
	INSERT INTO Pedidos(GuidPedido,IdEmpresa,Observacion,IdEstado,FechaSolicitud,FechaUltimaActualizacion)
	VALUES(@guidPedido,@idEmpresa,@observacion,@idEstado,GETDATE(),GETDATE());
END
