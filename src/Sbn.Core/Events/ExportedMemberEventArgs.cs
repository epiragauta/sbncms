using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.Events
{
    public class ExportedMemberEventArgs : EventArgs
    {
        public IMember Member { get; }
        public MemberExportModel Exported { get; }

        public ExportedMemberEventArgs(IMember member, MemberExportModel exported)
        {
            Member = member;
            Exported = exported;
        }
    }
}
