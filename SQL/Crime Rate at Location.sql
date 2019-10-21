/*
select count(*) from Properties
select count(*) from Parcels
select count(*) from Addresses
select count(*) from Streets
select count(*) from Crimes

select count(*) from PoliceDispatchCalls
select count(*) from FireDispatchCalls
*/

declare @p geography
declare @MinLat decimal(5, 2)
declare @MaxLat decimal(5, 2)
declare @MinLng decimal(5, 2)
declare @MaxLng decimal(5, 2)

select @p = Outline, @MinLat = MinLat, @MaxLat = MaxLat, @MinLng = MinLng, @MaxLng = MaxLng
from Parcels
where taxkey = '3190472000'

set @MinLat = @MinLat - 0.02
set @MaxLat = @MaxLat + 0.02
set @MinLng = @MinLng - 0.02
set @MaxLng = @MaxLng + 0.02

select @MinLat, @MaxLat, @MinLng, @MaxLng

select count(*) --Point.STDistance(@p), *
from Crimes c
where 
	(
		(c.MinLat <= @MinLat and c.MaxLat >= @MinLat) or
		(c.MinLat <= @MaxLat and c.MaxLat >= @MaxLat) or
		(c.MinLat >= @MinLat and c.MaxLat <= @MaxLat) or
		(c.MinLat <= @MaxLat and c.MaxLat <= @MinLat)
	) and (
		(c.MinLng <= @MinLng and c.MaxLng >= @MinLng) or
		(c.MinLng <= @MaxLng and c.MaxLng >= @MaxLng) or
		(c.MinLng >= @MinLng and c.MaxLng <= @MaxLng) or
		(c.MinLng <= @MaxLng and c.MaxLng <= @MinLng)
	) and Point.STIntersects (@p) = 1
--Point.STDistance(@P) < 1000


/*
                .Where(x =>
                    (x.MinLat <= northBound && x.MaxLat >= northBound) ||
                    (x.MinLat <= southBound && x.MaxLat >= southBound) ||
                    (x.MinLat >= northBound && x.MaxLat <= southBound) ||
                    (x.MinLat >= southBound && x.MaxLat <= northBound))
                .Where(x =>
                    (x.MinLng <= westBound && x.MaxLng >= westBound) ||
                    (x.MinLng <= eastBound && x.MaxLng >= eastBound) ||
                    (x.MinLng >= westBound && x.MaxLng <= eastBound) ||
                    (x.MinLng >= eastBound && x.MaxLng <= westBound))
                .Where(x => x.Point.Intersects(bounds));

			*/