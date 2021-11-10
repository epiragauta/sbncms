using Sbn.Cms.Core.Actions;

namespace Sbn.Cms.Core.Trees
{
    public class MenuItemCollectionFactory: IMenuItemCollectionFactory
    {
        private readonly ActionCollection _actionCollection;

        public MenuItemCollectionFactory(ActionCollection actionCollection)
        {
            _actionCollection = actionCollection;
        }

        public MenuItemCollection Create()
        {
            return new MenuItemCollection(_actionCollection);
        }

    }
}
