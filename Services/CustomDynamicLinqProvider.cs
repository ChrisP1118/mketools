using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Services
{
    class CustomDynamicLinqProvider : AbstractDynamicLinqCustomTypeProvider, IDynamicLinkCustomTypeProvider
    {
        public HashSet<Type> GetCustomTypes()
        {
            return new HashSet<Type>()
            {
                typeof(DbFunctionsExtensions),
                typeof(EF),
            };
        }

        public Type ResolveType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return ResolveType(assemblies, typeName);
        }

        public Type ResolveTypeBySimpleName(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return ResolveTypeBySimpleName(assemblies, typeName);
        }
    }
}
