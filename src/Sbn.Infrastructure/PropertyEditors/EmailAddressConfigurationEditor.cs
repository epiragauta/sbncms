// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.IO;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the email address value editor.
    /// </summary>
    public class EmailAddressConfigurationEditor : ConfigurationEditor<EmailAddressConfiguration>
    {
        public EmailAddressConfigurationEditor(IIOHelper ioHelper) : base(ioHelper)
        {
        }
    }
}
