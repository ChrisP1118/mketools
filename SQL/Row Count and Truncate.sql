/*
--delete from Addresses
--delete from Parcels
--delete from CommonParcels

--delete from Streets

--truncate table Crimes
*/

-- 257950
select count(*) from CommonParcels
-- 278925
select count(*) from Parcels
-- 452432
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