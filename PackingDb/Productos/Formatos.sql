CREATE TABLE [dbo].[Formatos]
(
	[IdFormato] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NombreFormato] NVARCHAR(50) NOT NULL, 
    [UnidadesPorFormato] INT NOT NULL
)
