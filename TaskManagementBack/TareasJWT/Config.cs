using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace TaskManagement
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                // Agrega más recursos si es necesario
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("taskmanagement.api", "Task Management API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "taskmanagement_client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("SuperSecreto".Sha256())
                    },
                    AllowedScopes = { "taskmanagement.api" }
                }
                // Agrega más clientes si es necesario
            };
    }
}
