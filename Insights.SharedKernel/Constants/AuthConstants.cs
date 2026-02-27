namespace Insights.SharedKernel.Constants;

public static class AuthConstants
{
    public const string ScopeClaimType = "scope";

    public static class Scopes
    {
        public const string GeoRead = "geo:read";
        public const string AuditRead = "audit:read";
        public const string Admin = "admin";
    }
}