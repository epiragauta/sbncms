using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Editors;
using Sbn.Cms.Core.PropertyEditors.ValueConverters;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;
using Sbn.Extensions;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Represents a media picker property editor.
    /// </summary>
    [DataEditor(
        Constants.PropertyEditors.Aliases.MediaPicker3,
        EditorType.PropertyValue,
        "Media Picker",
        "mediapicker3",
        ValueType = ValueTypes.Json,
        Group = Constants.PropertyEditors.Groups.Media,
        Icon = Constants.Icons.MediaImage)]
    public class MediaPicker3PropertyEditor : DataEditor
    {
        private readonly IIOHelper _ioHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPicker3PropertyEditor" /> class.
        /// </summary>
        public MediaPicker3PropertyEditor(
            IDataValueEditorFactory dataValueEditorFactory,
            IIOHelper ioHelper,
            EditorType type = EditorType.PropertyValue)
            : base(dataValueEditorFactory, type)
        {
            _ioHelper = ioHelper;
        }


        /// <inheritdoc />
        protected override IConfigurationEditor CreateConfigurationEditor() => new MediaPicker3ConfigurationEditor(_ioHelper);

        /// <inheritdoc />
        protected override IDataValueEditor CreateValueEditor() => DataValueEditorFactory.Create<MediaPicker3PropertyValueEditor>(Attribute);

        internal class MediaPicker3PropertyValueEditor : DataValueEditor, IDataValueReference
        {
            private readonly IJsonSerializer _jsonSerializer;

            public MediaPicker3PropertyValueEditor(
                ILocalizedTextService localizedTextService,
                IShortStringHelper shortStringHelper,
                IJsonSerializer jsonSerializer,
                IIOHelper ioHelper,
                DataEditorAttribute attribute)
                : base(localizedTextService, shortStringHelper, jsonSerializer, ioHelper, attribute)
            {
                _jsonSerializer = jsonSerializer;
            }

            public override object ToEditor(IProperty property, string culture = null, string segment = null)
            {
                var value = property.GetValue(culture, segment);

                return Deserialize(_jsonSerializer, value);
            }

            ///<remarks>
            /// Note: no FromEditor() and ToEditor() methods
            /// We do not want to transform the way the data is stored in the DB and would like to keep a raw JSON string
            /// </remarks>

            public IEnumerable<SbnEntityReference> GetReferences(object value)
            {
                foreach (var dto in Deserialize(_jsonSerializer, value))
                {
                    yield return new SbnEntityReference(Udi.Create(Constants.UdiEntityType.Media, dto.MediaKey));
                }
            }

            internal static IEnumerable<MediaWithCropsDto> Deserialize(IJsonSerializer jsonSerializer,object value)
            {
                var rawJson = value is string str ? str : value?.ToString();
                if (string.IsNullOrWhiteSpace(rawJson))
                {
                    yield break;
                }

                if (!rawJson.DetectIsJson())
                {
                    // Old comma seperated UDI format
                    foreach (var udiStr in rawJson.Split(Constants.CharArrays.Comma))
                    {
                        if (UdiParser.TryParse(udiStr, out GuidUdi udi))
                        {
                            yield return new MediaWithCropsDto
                            {
                                Key = Guid.NewGuid(),
                                MediaKey = udi.Guid,
                                Crops = Enumerable.Empty<ImageCropperValue.ImageCropperCrop>(),
                                FocalPoint = new ImageCropperValue.ImageCropperFocalPoint
                                {
                                    Left = 0.5m,
                                    Top = 0.5m
                                }
                            };
                        }
                    }
                }
                else
                {
                    // New JSON format
                    foreach (var dto in jsonSerializer.Deserialize<IEnumerable<MediaWithCropsDto>>(rawJson))
                    {
                        yield return dto;
                    }
                }
            }


            /// <summary>
            /// Model/DTO that represents the JSON that the MediaPicker3 stores.
            /// </summary>
            [DataContract]
            internal class MediaWithCropsDto
            {
                [DataMember(Name = "key")]
                public Guid Key { get; set; }

                [DataMember(Name = "mediaKey")]
                public Guid MediaKey { get; set; }

                [DataMember(Name = "crops")]
                public IEnumerable<ImageCropperValue.ImageCropperCrop> Crops { get; set; }

                [DataMember(Name = "focalPoint")]
                public ImageCropperValue.ImageCropperFocalPoint FocalPoint { get; set; }
            }
        }
    }
}
