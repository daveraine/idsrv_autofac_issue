using System.Collections.Generic;
using Thinktecture.IdentityServer.Core.Models;

namespace IdSrv
{
    static class Scopes
    {
        public static List<Scope> Get()
        {
            var list = new List<Scope>
            {
                new Scope
                {
                    Name = "api1"
                }
            };

            list.Add(StandardScopes.OfflineAccess);
            list.Add(StandardScopes.AllClaims);
            list.Add(StandardScopes.OpenId);
            list.Add(StandardScopes.Profile);
            list.AddRange(StandardScopes.All);

            return list;
        }
    }
}