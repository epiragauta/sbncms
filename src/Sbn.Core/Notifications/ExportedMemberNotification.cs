using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Notifications
{
    public class ExportedMemberNotification : INotification
    {
        public ExportedMemberNotification(IMember member, MemberExportModel exported)
        {
            Member = member;
            Exported = exported;
        }

        public IMember Member { get; }

        public MemberExportModel Exported { get; }
    }
}
