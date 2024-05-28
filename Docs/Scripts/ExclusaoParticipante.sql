--Exclusão de usuário caso mude de apontamento MktPlace Dev/Hml/Prd

DELETE FROM ParticipanteCatalogo WHERE ParticipanteId = 11;
DELETE FROM ParticipantePerfil WHERE ParticipanteId = 11;
DELETE FROM ParticipanteEstrutura WHERE ParticipanteID = 11;
DELETE FROM Participante WHERE Login = 58460835;