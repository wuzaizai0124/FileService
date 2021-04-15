using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerSerivce
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new IdentityResource[]
           {
                new IdentityResources.OpenId()
           };
    }
    public static IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
            {
                new ApiResource("testApi"){ Scopes={ "testApi"}}
            };
    }

    public static List<ApiScope> GetApiScopes => new List<ApiScope> { new ApiScope("testApi")};
    public static IEnumerable<Client> GetClients()
    {
      return new List<Client>
            {
                new Client
                {
                    ClientId = "test_client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                        //new Secret("secret")
                    },

                    // scopes that client has access to
                    AllowedScopes = { "testApi" }
                }
            };
    }
    public static List<TestUser> GetUsers()
    {
      return new List<TestUser> { 
        new TestUser{SubjectId="1",Username="testuser",Password="12345"}
      };
    }
  }
}
