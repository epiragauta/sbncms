// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Moq;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Tests.Common.Builders
{
    public class DataValueEditorBuilder<TParent> : ChildBuilderBase<TParent, IDataValueEditor>
    {
        private string _configuration;
        private string _view;
        private bool? _hideLabel;
        private string _valueType;

        public DataValueEditorBuilder(TParent parentBuilder)
            : base(parentBuilder)
        {
        }

        public DataValueEditorBuilder<TParent> WithConfiguration(string configuration)
        {
            _configuration = configuration;
            return this;
        }

        public DataValueEditorBuilder<TParent> WithView(string view)
        {
            _view = view;
            return this;
        }

        public DataValueEditorBuilder<TParent> WithHideLabel(bool hideLabel)
        {
            _hideLabel = hideLabel;
            return this;
        }

        public DataValueEditorBuilder<TParent> WithValueType(string valueType)
        {
            _valueType = valueType;
            return this;
        }

        public override IDataValueEditor Build()
        {
            var configuration = _configuration ?? null;
            var view = _view ?? null;
            var hideLabel = _hideLabel ?? false;
            var valueType = _valueType ?? Guid.NewGuid().ToString();

            return new DataValueEditor(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IShortStringHelper>(),
                Mock.Of<IJsonSerializer>())
            {
                Configuration = configuration,
                View = view,
                HideLabel = hideLabel,
                ValueType = valueType,
            };
        }
    }
}
