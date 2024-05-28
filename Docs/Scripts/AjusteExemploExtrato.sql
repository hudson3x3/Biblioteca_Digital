
update indicacaoheaderextrato 
set 
	ReferralRepresentativeNumberHeader = 58460835 --participante novo
	MktPlaceCatalogoId = 40292
where ReferralRepresentativeNumberHeader = 67998420 --Participante existente

select Id from participante where Login = 58460835;

update indicacaodetailextrato 
set ReferralRepresentativeNumberDetail = 58460835
where indicacaoheaderextratoid = 1