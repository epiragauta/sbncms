using System;
using System.Linq;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;

namespace Sbn.Cms.Web.Website.Models
{

    /// <summary>
    /// Builds a <see cref="RegisterModel"/> for use on the front-end
    /// </summary>
    public class RegisterModelBuilder : MemberModelBuilderBase
    {
        private string _memberTypeAlias;
        private bool _lookupProperties;
        private bool _usernameIsEmail;
        private string _redirectUrl;

        public RegisterModelBuilder(IMemberTypeService memberTypeService, IShortStringHelper shortStringHelper)
            : base(memberTypeService, shortStringHelper)
        {
        }

        public RegisterModelBuilder WithRedirectUrl(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
            return this;
        }

        public RegisterModelBuilder UsernameIsEmail(bool usernameIsEmail = true)
        {
            _usernameIsEmail = usernameIsEmail;
            return this;
        }

        public RegisterModelBuilder WithMemberTypeAlias(string memberTypeAlias)
        {
            _memberTypeAlias = memberTypeAlias;
            return this;
        }

        public RegisterModelBuilder WithCustomProperties(bool lookupProperties)
        {
            _lookupProperties = lookupProperties;
            return this;
        }

        public RegisterModel Build()
        {
            var providedOrDefaultMemberTypeAlias = _memberTypeAlias ?? Core.Constants.Conventions.MemberTypes.DefaultAlias;
            IMemberType memberType = MemberTypeService.Get(providedOrDefaultMemberTypeAlias);
            if (memberType == null)
            {
                throw new InvalidOperationException($"Could not find a member type with alias: {providedOrDefaultMemberTypeAlias}.");
            }

            var model = new RegisterModel
            {
                MemberTypeAlias = providedOrDefaultMemberTypeAlias,
                UsernameIsEmail = _usernameIsEmail,
                MemberProperties = _lookupProperties ? GetMemberPropertiesViewModel(memberType) : Enumerable.Empty<MemberPropertyModel>().ToList()
            };
            return model;
        }
    }
}
