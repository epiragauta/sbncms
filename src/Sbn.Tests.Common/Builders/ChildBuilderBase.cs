// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Common.Builders
{
    public abstract class ChildBuilderBase<TParent, TType> : BuilderBase<TType>
    {
        private readonly TParent _parentBuilder;

        protected ChildBuilderBase(TParent parentBuilder) => _parentBuilder = parentBuilder;

        public TParent Done() => _parentBuilder;
    }
}
