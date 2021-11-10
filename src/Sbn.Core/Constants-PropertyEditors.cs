using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Cms.Core
{
    public static partial class Constants
    {
        /// <summary>
        /// Defines property editors constants.
        /// </summary>
        public static class PropertyEditors
        {
            /// <summary>
            /// Used to prefix generic properties that are internal content properties
            /// </summary>
            public const string InternalGenericPropertiesPrefix = "_umb_";

            public static class Legacy
            {
                public static class Aliases
                {
                    public const string Textbox = "Sbn.Textbox";
                    public const string Date = "Sbn.Date";
                    public const string ContentPicker2 = "Sbn.ContentPicker2";
                    public const string MediaPicker2 = "Sbn.MediaPicker2";
                    public const string MemberPicker2 = "Sbn.MemberPicker2";
                    public const string MultiNodeTreePicker2 = "Sbn.MultiNodeTreePicker2";
                    public const string TextboxMultiple = "Sbn.TextboxMultiple";
                    public const string RelatedLinks2 = "Sbn.RelatedLinks2";
                    public const string RelatedLinks = "Sbn.RelatedLinks";

                }
            }

            /// <summary>
            /// Defines Sbn built-in property editor aliases.
            /// </summary>
            public static class Aliases
            {
                /// <summary>
                /// Block List.
                /// </summary>
                public const string BlockList = "Sbn.BlockList";

                /// <summary>
                /// CheckBox List.
                /// </summary>
                public const string CheckBoxList = "Sbn.CheckBoxList";

                /// <summary>
                /// Color Picker.
                /// </summary>
                public const string ColorPicker = "Sbn.ColorPicker";

                /// <summary>
                /// Eye Dropper Color Picker.
                /// </summary>
                public const string ColorPickerEyeDropper = "Sbn.ColorPicker.EyeDropper";

                /// <summary>
                /// Content Picker.
                /// </summary>
                public const string ContentPicker = "Sbn.ContentPicker";

                /// <summary>
                /// DateTime.
                /// </summary>
                public const string DateTime = "Sbn.DateTime";

                /// <summary>
                /// DropDown List.
                /// </summary>
                public const string DropDownListFlexible = "Sbn.DropDown.Flexible";

                /// <summary>
                /// Grid.
                /// </summary>
                public const string Grid = "Sbn.Grid";

                /// <summary>
                /// Image Cropper.
                /// </summary>
                public const string ImageCropper = "Sbn.ImageCropper";

                /// <summary>
                /// Integer.
                /// </summary>
                public const string Integer = "Sbn.Integer";

                /// <summary>
                /// Decimal.
                /// </summary>
                public const string Decimal = "Sbn.Decimal";

                /// <summary>
                /// ListView.
                /// </summary>
                public const string ListView = "Sbn.ListView";

                /// <summary>
                /// Media Picker.
                /// </summary>
                public const string MediaPicker = "Sbn.MediaPicker";

                /// <summary>
                /// Media Picker v.3.
                /// </summary>
                public const string MediaPicker3 = "Sbn.MediaPicker3";

                /// <summary>
                /// Multiple Media Picker.
                /// </summary>
                public const string MultipleMediaPicker = "Sbn.MultipleMediaPicker";

                /// <summary>
                /// Member Picker.
                /// </summary>
                public const string MemberPicker = "Sbn.MemberPicker";

                /// <summary>
                /// Member Group Picker.
                /// </summary>
                public const string MemberGroupPicker = "Sbn.MemberGroupPicker";

                /// <summary>
                /// MultiNode Tree Picker.
                /// </summary>
                public const string MultiNodeTreePicker = "Sbn.MultiNodeTreePicker";

                /// <summary>
                /// Multiple TextString.
                /// </summary>
                public const string MultipleTextstring = "Sbn.MultipleTextstring";

                /// <summary>
                /// Label.
                /// </summary>
                public const string Label = "Sbn.Label";

                /// <summary>
                /// Picker Relations.
                /// </summary>
                public const string PickerRelations = "Sbn.PickerRelations";

                /// <summary>
                /// RadioButton list.
                /// </summary>
                public const string RadioButtonList = "Sbn.RadioButtonList";

                /// <summary>
                /// Slider.
                /// </summary>
                public const string Slider = "Sbn.Slider";

                /// <summary>
                /// Tags.
                /// </summary>
                public const string Tags = "Sbn.Tags";

                /// <summary>
                /// Textbox.
                /// </summary>
                public const string TextBox = "Sbn.TextBox";

                /// <summary>
                /// Textbox Multiple.
                /// </summary>
                public const string TextArea = "Sbn.TextArea";

                /// <summary>
                /// TinyMCE
                /// </summary>
                public const string TinyMce = "Sbn.TinyMCE";

                /// <summary>
                /// Boolean.
                /// </summary>
                public const string Boolean = "Sbn.TrueFalse";

                /// <summary>
                /// Markdown Editor.
                /// </summary>
                public const string MarkdownEditor = "Sbn.MarkdownEditor";

                /// <summary>
                /// User Picker.
                /// </summary>
                public const string UserPicker = "Sbn.UserPicker";

                /// <summary>
                /// Upload Field.
                /// </summary>
                public const string UploadField = "Sbn.UploadField";

                /// <summary>
                /// Email Address.
                /// </summary>
                public const string EmailAddress = "Sbn.EmailAddress";

                /// <summary>
                /// Nested Content.
                /// </summary>
                public const string NestedContent = "Sbn.NestedContent";

                /// <summary>
                /// Alias for the multi URL picker editor.
                /// </summary>
                public const string MultiUrlPicker = "Sbn.MultiUrlPicker";
            }

            /// <summary>
            /// Defines Sbn build-in datatype configuration keys.
            /// </summary>
            public static class ConfigurationKeys
            {
                /// <summary>
                /// The value type of property data (i.e., string, integer, etc)
                /// </summary>
                /// <remarks>Must be a valid <see cref="ValueTypes"/> value.</remarks>
                public const string DataValueType = "sbnDataValueType";
            }

            /// <summary>
            /// Defines Sbn's built-in property editor groups.
            /// </summary>
            public static class Groups
            {
                public const string Common = "Common";

                public const string Lists = "Lists";

                public const string Media = "Media";

                public const string People = "People";

                public const string Pickers = "Pickers";

                public const string RichContent = "Rich Content";
            }
        }
    }
}
