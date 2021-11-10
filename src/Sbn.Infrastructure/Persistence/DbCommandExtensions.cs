using System.Data;

namespace Sbn.Cms.Infrastructure.Persistence
{
    internal static class DbCommandExtensions
    {
        /// <summary>
        /// Unwraps a database command.
        /// </summary>
        /// <remarks>SbnDatabase wraps the original database connection in various layers (see
        /// OnConnectionOpened); this unwraps and returns the original database command.</remarks>
        public static IDbCommand UnwrapSbn(this IDbCommand command)
        {
            IDbCommand unwrapped;

            var c = command;
            do
            {
                unwrapped = c;

                var profiled = unwrapped as StackExchange.Profiling.Data.ProfiledDbCommand;
                if (profiled != null) unwrapped = profiled.InternalCommand;

            } while (c != unwrapped);

            return unwrapped;
        }
    }
}
