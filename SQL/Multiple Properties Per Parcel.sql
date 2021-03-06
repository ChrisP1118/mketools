/*
-- Multiple addresses, same parcel
select *
from (
	select cast(Outline as nvarchar(max)) OutlineString, count(*) as Ct
	from Properties p
		join Parcels r on p.TAXKEY = r.TAXKEY
	where STREET = 'WEBSTER'
		and HOUSE_NR_LO >= 2000
		and HOUSE_NR_HI < 2100
	group by cast(Outline as nvarchar(max))
	having count(cast(Outline as nvarchar(max))) > 1
) x
	join Parcels r on cast(r.Outline as nvarchar(max)) = x.OutlineString
	join Properties p on r.TAXKEY = p.TAXKEY

-- Same address, same parcel
select *
from (
	select cast(Outline as nvarchar(max)) OutlineString, count(*) as Ct
	from Properties p
		join Parcels r on p.TAXKEY = r.TAXKEY
	where STREET = 'BELLEVIEW'
		and HOUSE_NR_LO >= 2222
		and HOUSE_NR_HI < 2223
	group by cast(Outline as nvarchar(max))
	having count(cast(Outline as nvarchar(max))) > 1
) x
	join Parcels r on cast(r.Outline as nvarchar(max)) = x.OutlineString
	join Properties p on r.TAXKEY = p.TAXKEY

-- Examples of both
select p.HOUSE_NR_LO, p.STREET, *
from (
	select cast(Outline as nvarchar(max)) OutlineString, count(*) as Ct
	from Properties p
		join Parcels r on p.TAXKEY = r.TAXKEY
	where STREET = 'BELLEVIEW'
	group by cast(Outline as nvarchar(max))
	having count(cast(Outline as nvarchar(max))) > 1
) x
	join Parcels r on cast(r.Outline as nvarchar(max)) = x.OutlineString
	join Properties p on r.TAXKEY = p.TAXKEY
order by p.HOUSE_NR_LO, p.STREET
*/

/*
select count(*) from Parcels
select count(*) from Properties
*/

/*
select top 35
	cast(Outline as nvarchar(max)) as OutlineString,
	HASHBYTES('SHA2_256', cast(Outline as nvarchar(max)))
from Parcels

select top 35 *
from Parcels
*/

select x.*, r.*
from (
	select top 500
		cast(Outline as nvarchar(max)) as OutlineString,
		HASHBYTES('SHA2_256', left(cast(Outline as nvarchar(max)), 4000)) as OutlineHash
	from Parcels
) x
	join Parcels r on cast(Outline as nvarchar(max)) = x.OutlineString