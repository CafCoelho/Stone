drop table SaldoAtual
go
create table SaldoAtual
(
idCliente int not null primary key,
saldo decimal(18,2) not null
)

drop table HistoricoSaldo
go
create table HistoricoSaldo
(
IdHistorico int not null primary key,
IdCliente int Not Null, index IdxIdCliente2 NonClustered (idCliente) ,
Valor decimal(18,2) not null,
TipoOperacao char(1) not null,
IdOperacao varChar(50) null
)