using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.PropertyEditors
{
    public interface IDataValueEditorFactory
    {
        TDataValueEditor Create<TDataValueEditor>(params object[] args)
            where TDataValueEditor : class, IDataValueEditor;
    }
}
