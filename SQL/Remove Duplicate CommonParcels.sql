/*
select count(*) from Properties
select count(*) from Parcels
select count(*) from CommonParcels
*/

/*
delete from Parcels
delete from CommonParcels
*/

/*
select c.id, count(*)
from Parcels p
	join CommonParcels c on c.Id = p.CommonParcelId
group by c.Id
having count(*) > 1

select *
from Parcels p
	join CommonParcels c on c.Id = p.CommonParcelId
	join Properties r on r.TAXKEY = p.Taxkey
where c.id = '1E3D8E55-7B6D-4A58-B291-2D44F14D9FF5'
*/


select count(*) from Parcels
select count(*) from CommonParcels


declare @Dupes table (
	OutlineString nvarchar(max),
	OneId uniqueidentifier,
	Count int
)

insert into @Dupes
select cast(Outline as nvarchar(max)), max(Id), count(*) as Ct
from CommonParcels
group by cast(Outline as nvarchar(max))
having count(*) > 1
order by Ct desc

declare @Replacements table (
	OutlineString nvarchar(max),
	OldId uniqueidentifier,
	OneId uniqueidentifier,
	IsOneId bit
)

insert into @Replacements
select d.OutlineString, cp.Id, d.OneId, case when cp.Id = d.OneId then 1 else 0 end as IsOneId
from CommonParcels cp
	join @Dupes d on cast(cp.Outline as nvarchar(max)) = d.OutlineString
order by d.OutlineString

update p
set p.CommonParcelId = r.OneId
from Parcels p
	join @Replacements r on p.CommonParcelId = r.OldId
where r.IsOneId = 0

delete
from CommonParcels
where Id in (
	select OldId
	from @Replacements
	where IsOneId = 0
)

select count(*) from Parcels
select count(*) from CommonParcels
