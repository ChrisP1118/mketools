﻿using MkeTools.Web.Models.Data.Places;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeTools.Web.Services.Data.Interfaces
{
    public interface ICommonParcelService : IEntityWriteService<CommonParcel, int>
    {
        //Task RemoveDuplicates();
    }
}
