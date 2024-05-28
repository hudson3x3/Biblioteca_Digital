USE [MeuMundoAvonDev]
GO

--delete LogRaiz
--delete LogAcao
--DELETE logerro

select r.ip, r.TokenLtm, sum(TempoProcessamento) from 
			lograiz r
inner join  logacao a on (r.TokenLtm = a.TokenLtm)
group by r.TokenLtm, r.ip
order by max(r.DataInclusao) desc


select * from LogRaiz order by DataInclusao desc
select * from LogAcao order by DataInclusao desc
select * from logerro order by DataInclusao desc


select * from catalogo
select top 5 * from participantecatalogo order by id desc
select top 5 * from participante order by id desc

update [dbo].[Participante]
set cpf = '00000000000', Login = '00000000000'
where cpf = '30499512812'


--delete participante
--delete participantecatalogo
--delete participanteestrutura
--delete  participanteperfil


http://hml-marketplace-cms-site.grupoltm.com.br/init.html?access_token={0}&amp;return_url=/loja/ponto-frio

update catalogo set UrlCatalogMktPlace = 'http://hml-marketplace-cms-site.grupoltm.com.br/init.html?access_token={0}&return_url=/loja/ponto-frio&callback=www.avon.com.br'




--DBCC CHECKIDENT('[LogTipo]', RESEED, 0)


update catalogo set  MktPlaceCatalogoId = 60270 where id = 1



select * from participantecatalogo