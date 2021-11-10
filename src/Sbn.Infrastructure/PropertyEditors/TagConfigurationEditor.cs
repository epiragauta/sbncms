// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors.Validators;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents the configuration editor for the tag value editor.
    /// </summary>
    public class TagConfigurationEditor : ConfigurationEditor<TagConfiguration>
    {
        public TagConfigurationEditor(ManifestValueValidatorCollection validators, IIOHelper ioHelper, ILocalizedTextService localizedTextService) : base(ioHelper)
        {
            Field(nameof(TagConfiguration.Group)).Validators.Add(new RequiredValidator(localizedTextService));
            Field(nameof(TagConfiguration.StorageType)).Validators.Add(new RequiredValidator(localizedTextService));
        }

        public override Dictionary<string, object> ToConfigurationEditor(TagConfiguration configuration)
        {
            var dictionary = base.ToConfigurationEditor(configuration);

            // the front-end editor expects the string value of the storage type
            if (!dictionary.TryGetValue("storageType", out var storageType))
                storageType = TagsStorageType.Json; //default to Json
            dictionary["storageType"] = storageType.ToString();

            return dictionary;
        }

        public override TagConfiguration FromConfigurationEditor(IDictionary<string, object> editorValues, TagConfiguration configuration)
        {
            // the front-end editor returns the string value of the storage type
            // pure Json could do with
            // [JsonConverter(typeof(StringEnumConverter))]
            // but here we're only deserializing to object and it's too late

            editorValues["storageType"] = Enum.Parse(typeof(TagsStorageType), (string) editorValues["storageType"]);
            return base.FromConfigurationEditor(editorValues, configuration);
        }
    }
}
