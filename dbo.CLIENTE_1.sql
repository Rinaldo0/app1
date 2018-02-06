CREATE TABLE [dbo].[CLIENTE] (
    [ID_CLIENTE] INT          IDENTITY (1, 1) NOT NULL,
    [NOME]       VARCHAR (80) NOT NULL,
    [DATA_NASC]  DATE         NOT NULL,
    [CPF]        VARCHAR (11) NULL,
    [Endereco] VARCHAR(50) NOT NULL, 
    [Telefone] VARCHAR(50) NOT NULL, 
    PRIMARY KEY CLUSTERED ([ID_CLIENTE] ASC)
);

