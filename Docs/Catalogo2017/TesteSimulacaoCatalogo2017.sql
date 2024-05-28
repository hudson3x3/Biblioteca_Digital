--6/2017 - emulando 7/2017
--select * from catalogocp where id in(38,21)
update catalogocp set profileid = 50722, catalogoid = 3 where id = 21


-- 72504605 - 5/2017 - catálogo 2016 - Primeiro acesso
delete ParticipantePerfil where ParticipanteId = 1746
delete ParticipanteEstrutura where ParticipanteId = 1746
delete ParticipanteCatalogo where ParticipanteId = 1746
delete ParticipanteCP where ParticipanteId = 1746
delete from participante where login = 72504605

-- select * from participante where login = 72504605

-- http://localhost:54763/avonAuthentication/40292/catalogo/?a=72504605&encryptedKey=67E6308A486C1BD2E125E268A97425AA
-- http://localhost:54763/avonAuthentication/40292/extrato/?a=72504605&encryptedKey=67E6308A486C1BD2E125E268A97425AA



-- 70310211 - 6/2017 - emulando 7/2017 - catálogo 2017 - Primeiro acesso
delete ParticipantePerfil where ParticipanteId = 1
delete ParticipanteEstrutura where ParticipanteId = 1
delete ParticipanteCatalogo where ParticipanteId = 1
delete ParticipanteCP where ParticipanteId = 1
delete from participante where login = 70310211

-- http://localhost:54763/avonAuthentication/40322/catalogo/?a=70310211&encryptedKey=3EF0E3F6F6C7079832EF704EEA0E0C39
-- http://localhost:54763/avonAuthentication/40322/extrato/?a=70310211&encryptedKey=3EF0E3F6F6C7079832EF704EEA0E0C39



--Teste de extrato com pontuação 
--http://localhost:54763/avonAuthentication/40292/extrato/?a=59310274&encryptedKey=172D89E8F7B3AEE3FFCDCFC53BE3BD87