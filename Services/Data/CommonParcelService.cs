using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services.Data.Interfaces;
using NetTopologySuite.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;
using Coordinate = GeoAPI.Geometries.Coordinate;

namespace MkeAlerts.Web.Services.Data
{
    public class CommonParcelService : EntityWriteService<CommonParcel, Guid>, ICommonParcelService
    {
        public CommonParcelService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IValidator<CommonParcel> validator, ILogger<EntityWriteService<CommonParcel, Guid>> logger) : base(dbContext, userManager, validator, logger)
        {
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyIdFilter(IQueryable<CommonParcel> queryable, Guid id)
        {
            return queryable.Where(x => x.Id == id);
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyReadSecurity(ApplicationUser applicationUser, IQueryable<CommonParcel> queryable)
        {
            return queryable;
        }

        protected override async Task<IQueryable<CommonParcel>> ApplyBounds(IQueryable<CommonParcel> queryable, double northBound, double southBound, double eastBound, double westBound, Polygon bounds)
        {
            return queryable
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
                .Where(x => x.Outline.Intersects(bounds));
        }

        protected override async Task<bool> CanWrite(ApplicationUser applicationUser, CommonParcel dataModel)
        {
            // Site admins can write
            if (await _userManager.IsInRoleAsync(applicationUser, ApplicationRole.SiteAdminRole))
                return true;

            return false;
        }

        public async Task RemoveDuplicates()
        {
            _dbContext.CommonParcels.FromSql(@"
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
)");
        }
    }
}
