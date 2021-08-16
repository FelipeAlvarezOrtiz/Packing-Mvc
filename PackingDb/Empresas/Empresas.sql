CREATE TABLE [dbo].[Empresas]
(
	[IdEmpresa] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NombreEmpresa] NVARCHAR(50) NOT NULL, 
    [RazonSocial] NVARCHAR(50) NOT NULL, 
    [RutEmpresa] NVARCHAR(13) NOT NULL, 
    [Direccion] NVARCHAR(250) NOT NULL, 
    [PersonaContacto] NVARCHAR(75) NOT NULL
)
