CREATE PROCEDURE [dbo].[ObtenerDatosPedido]
	@idEmpresa int
AS
BEGIN
	SELECT P.Observacion,DP.Cantidad,P.FechaSolicitud 
	FROM Pedidos P, DetallesPedidos DP, Empresas E, Productos P 
	LEFT JOIN DetallesPedidos ON Pedidos.GuidPedido = DetallesPedidos.GuidSolicitudCabecera 
	LEFT JOIN Empresas ON Pedidos.IdEmpresa = Empresas.IdEmpresa
	WHERE dbo.[Pedidos].[IdEmpresa] = @idEmpresa;
END;
