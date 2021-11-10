using System;

namespace Sbn.Cms.Core.Events
{
    [Serializable]
    public delegate void TypedEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs e);
}
