using Sbn.Cms.Core.Models.PublishedContent;

namespace Sbn.Cms.Core.Models
{
    /// <summary>
    /// The basic view model returned for front-end Sbn controllers
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IContentModel"/> exists in order to unify all view models in Sbn, whether it's a normal template view or a partial view macro, or
    /// a user's custom model that they have created when doing route hijacking or custom routes.
    /// </para>
    /// <para>
    /// By default all front-end template views inherit from SbnViewPage which has a model of <see cref="IPublishedContent"/> but the model returned
    /// from the controllers is <see cref="IContentModel"/> which in normal circumstances would not work. This works with SbnViewPage because it
    /// performs model binding between IContentModel and IPublishedContent. This offers a lot of flexibility when rendering views. In some cases if you
    /// are route hijacking and returning a custom implementation of <see cref="IContentModel"/> and your view is strongly typed to this model, you can still
    /// render partial views created in the back office that have the default model of IPublishedContent without having to worry about explicitly passing
    /// that model to the view.
    /// </para>
    /// </remarks>
    public interface IContentModel
    {
        /// <summary>
        /// Gets the <see cref="IPublishedContent"/>
        /// </summary>
        IPublishedContent Content { get; }
    }
}
