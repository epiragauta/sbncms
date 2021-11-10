// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Security;
using Sbn.Cms.Tests.Common.Builders.Interfaces;

namespace Sbn.Cms.Tests.Common.Builders
{
    public class SbnIdentityRoleBuilder : BuilderBase<SbnIdentityRole>,
            IWithIdBuilder<string>,
            IWithNameBuilder
    {
        private string _id;
        private string _name;

        public SbnIdentityRoleBuilder WithTestName(string id)
        {
            _name = "testname";
            _id = id;
            return this;
        }

        string IWithNameBuilder.Name
        {
            get => _name;
            set => _name = value;
        }

        string IWithIdBuilder<string>.Id
        {
            get => _id;
            set => _id = value;
        }

        public override SbnIdentityRole Build()
        {
            var id = _id;
            var name = _name;

            return new SbnIdentityRole
            {
                Id = id,
                Name = name,
            };
        }
    }
}
