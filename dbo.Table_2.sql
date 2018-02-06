CREATE TABLE [dbo].[Table] (
    [Id]        INT           NOT NULL,
    [Nome]      VARCHAR (50)  NOT NULL,
    [Data_Nasc] DATE          NOT NULL,
    [CPF]       VARCHAR (50)  NULL,
    [Telefone ] NUMERIC (18)  NOT NULL,
    [Endereço]  NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

