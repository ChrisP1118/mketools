
--select * from AspNetUsers

/*
update AspNetUsers
set UserName = 'admin@mkealerts.com', NormalizedUserName = 'admin@mkealerts.com', Email = 'admin@mkealerts.com', NormalizedEmail = 'admin@mkealerts.com'
where Id = '85F00D40-D578-4988-9F22-4D023175F852'
*/

--delete from DispatchCallSubscriptions
--select * from DispatchCallSubscriptions

/*
select *
from Addresses
where Addresses.STREET = 'JAMES LOVELL'
	and HSE_NBR = 951

select *
from Properties
where TAXKEY = '3611685100'

select *
from Parcels
where MinLat = 0

select top 10 *
from Crimes
*/

/*
CREATE TABLE [dbo].[PoliceDispatchCallType](	
	[NatureOfCall] [nvarchar](20) NOT NULL,
	[ManualNatureOfCall] [nvarchar](20) NOT NULL,
	[MinOfficers] [int] NOT NULL,
	[MaxOfficers] [int] NOT NULL,
	[Sargent] [int] NOT NULL,
	[Sheriff] [int] NOT NULL
) ON [PRIMARY]	

CREATE TABLE [dbo].[PoliceDispatchCallTypes](	
	[NatureOfCall] [nvarchar](20) NOT NULL,
	[IsCritical] [bit] NOT NULL,
	[IsViolent] [bit] NOT NULL,
	[IsProperty] [bit] NOT NULL,
	[IsDrug] [bit] NOT NULL,
	[IsTraffic] [bit] NOT NULL
) ON [PRIMARY]	
*/

/*
select p.NatureOfCall, t.NatureOfCall, count(*)
from PoliceDispatchCalls p
	left join PoliceDispatchCallType t on p.NatureOfCall = t.NatureOfCall
group by p.NatureOfCall, t.NatureOfCall
order by p.NatureOfCall
*/

/*
select *
from (
	select p.NatureOfCall, count(*) as Ct, max(cast(t.IsCritical as int)) as IsCritical, max(cast(t.IsViolent as int)) as IsViolent, max(cast(t.IsProperty as int)) as IsProperty, max(cast(t.IsDrug as int)) as IsDrug, max(cast(t.IsTraffic as int)) as IsTraffic
	from PoliceDispatchCalls p
		join PoliceDispatchCallTypes t on p.NatureOfCall = t.NatureOfCall
	group by p.NatureOfCall, t.NatureOfCall

	union

	select p.NatureOfCall, count(*), max(cast(t.IsCritical as int)), max(cast(t.IsViolent as int)), max(cast(t.IsProperty as int)), max(cast(t.IsDrug as int)), max(cast(t.IsTraffic as int))
	from PoliceDispatchCalls p
		left join PoliceDispatchCallTypes t on p.NatureOfCall = t.NatureOfCall
	where t.NatureOfCall is null
	group by p.NatureOfCall, t.NatureOfCall

	union

	select t.NatureOfCall, 0, t.IsCritical, t.IsViolent, t.IsProperty, t.IsDrug, t.IsTraffic
	from PoliceDispatchCallTypes t
		left join PoliceDispatchCalls p on p.NatureOfCall = t.NatureOfCall
	where p.NatureOfCall is null
) x
order by x.IsCritical desc, x.IsViolent desc, x.IsProperty desc, x.IsDrug desc, x.IsTraffic desc, x.NatureOfCall asc
*/

select NatureOfCall, count(*)
from FireDispatchCalls
group by NatureOfCall

/*
delete
from FireDispatchCalls
where NatureOfCall = ''
*/

select top 5 *
from Crimes
where TypeOfCrime is not null