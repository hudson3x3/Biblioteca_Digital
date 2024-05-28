
alter table catalogo add AppIdMktPlace varchar(256) null

insert into catalogo 
(
MktPlaceCatalogoId,
UrlCatalogMktPlace,
PrimeiroAcesso,
Ativo,
DataInicio,
DataFim,
IdCampanha,
IdOrigem,
IdEmpresa,
Nome,
AppIdMktPlace
)
select 
40322,
'https://catalogo2017.meumundoavon.com.br/init.html?access_token={0}',
1,
1,
'01-01-2000',
'01-01-2020',
30316,
2022908920,
306,
'Catalogo 5',
'MDExZjE1MTctMmIzMC00OTgxLWIwZTgtZmFkYmNhNTNmOTFhOmZ3ZmtnY2N1'


insert into catalogocp
select '7/2017',50722,3,1
union
select '8/2017',50723,3,1
union
select '9/2017',50724,3,1
union
select '10/2017',50725,3,1
union
select '11/2017',50726,3,1
union
select '12/2017',50727,3,1
union
select '1/2018',50728,3,1
union
select '2/2018',50729,3,1
union
select '3/2018',50730,3,1
union
select '4/2018',50731,3,1
union
select '5/2018',50732,3,1
union
select '13/2017',50733,3,1
union
select '14/2017',50734,3,1
union
select '15/2017',50735,3,1
union
select '16/2017',50736,3,1
union
select '17/2017',50737,3,1
union
select '18/2017',50738,3,1
union
select '19/2017',50739,3,1
union
select '20/2017',50740,3,1

update catalogo set AppIdMktPlace = 'MWU2OGE1NDFkMjk3NDlhOTgyZDYwMjliNTFiOWZmMGI6QHZvbiEyIzQl' where id in (1,2) 