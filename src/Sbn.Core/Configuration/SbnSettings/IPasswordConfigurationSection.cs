namespace Sbn.Cms.Core.Configuration.SbnSettings
{
    public interface IPasswordConfigurationSection : ISbnConfigurationSection
    {
        int RequiredLength { get; }

        bool RequireNonLetterOrDigit { get; }

        bool RequireDigit { get; }

        bool RequireLowercase { get; }

        bool RequireUppercase { get; }

        bool UseLegacyEncoding { get; }

        string HashAlgorithmType { get; }

        int MaxFailedAccessAttemptsBeforeLockout { get; }
    }
}
