using System.Collections.Generic;
using Thinktecture.IdentityServer.Core.Models;

namespace IdSrv
{
    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                // human is involved
                new Client
                {
                    ClientName = "Silicon on behalf of Carbon Client",
                    ClientId = "carbon",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Reference,

                    Flow = Flows.ResourceOwner,
                    ClientSecrets = new List<ClientSecret>
                    {
                        new ClientSecret("secret".Sha256())
                    }
                }
            };
        }
    }
}