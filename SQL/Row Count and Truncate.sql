/*
--delete from Parcels
--delete from CommonParcels

delete from Addresses
delete from Streets

truncate table Crimes
*/

select count(*) from Parcels
select count(*) from CommonParcels
select count(*) from Addresses
select count(*) from Streets
select count(*) from Crimes
select count(*) from PoliceDispatchCalls
select count(*) from FireDispatchCalls

/*
select MAP_ID, count(*)
from Parcels
group by MAP_ID
having count(*) > 1
*/

/*
select top 50 * 
from CommonParcels cp
	join Parcels p on cp.MAP_ID = p.MAP_ID
*/