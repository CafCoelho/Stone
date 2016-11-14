USE [Stone]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER dbo.tr_SumarizaSaldo ON dbo.SaldoHistorico
FOR INSERT, UPDATE, DELETE
NOT FOR REPLICATION
AS

IF @@rowcount = 0
	RETURN

SET NOCOUNT ON
----------------------------------------------------------------------------------------------------------------------------------
--	Parte da Insercao
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT IM.IdCliente,im.Valor,im.TipoOperacao FROM Inserted IM LEFT OUTER JOIN SaldoAtual SA ON (IM.IdCliente = SA.idCliente) WHERE SA.idCliente IS NULL)
BEGIN
	INSERT INTO SaldoAtual (IdCliente,saldo) 
	 (SELECT IM.IdCliente, im.Valor FROM Inserted IM)
END
else
BEGIN
    UPDATE SA set SA.Saldo = (CASE WHEN IM.TipoOperacao= '+' THEN SA.Saldo + IM.valor ELSE SA.Saldo - IM.valor END)
	from Inserted IM
	JOIN SaldoAtual SA ON (IM.IdCliente = SA.IdCliente)
END
----------------------------------------------------------------------------------------------------------------------------------
--	Parte da Delecao
--	Só executa se for delete mesmo, nao se for update
----------------------------------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT IM.IdCliente,im.Valor,im.TipoOperacao FROM Inserted IM LEFT OUTER JOIN SaldoAtual SA ON (IM.IdCliente = SA.idCliente) WHERE SA.idCliente IS NULL)
BEGIN
    UPDATE SA set SA.Saldo = SA.Saldo - DL.valor
	from deleted DL
	JOIN SaldoAtual SA ON (DL.IdCliente = SA.IdCliente)
end
GO

