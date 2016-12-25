using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Regions;
using Telerik.Windows.Controls;
using Menu = $saferootprojectname$.Core.Controls.Menues.Menu;

namespace $safeprojectname$.Prism
{
    public class PanelBarAdapter : RegionAdapterBase<RadPanelBar>
    {
        public PanelBarAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, RadPanelBar regionTarget)
        {
            region.Views.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var view in e.NewItems)
                        {
                            AddViewToRegion(view, regionTarget);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var view in e.OldItems)
                        {
                            RemoveViewFromRegion(view, regionTarget);
                        }
                        break;
                }
            };

        }

        private void RemoveViewFromRegion(object view, RadPanelBar regionTarget)
        {
            var menuItem = view as Menu;
            if (menuItem != null)
            {
                foreach (var findedMenu in regionTarget.Items.Cast<RadPanelBarItem>().Select(item => menuItem.Items.FirstOrDefault(x => x.UniqId == (Guid)item.Tag)).Where(findedMenu => findedMenu != null))
                {
                    regionTarget.Items.Remove(findedMenu);
                }
            }
        }

        private void AddViewToRegion(object view, RadPanelBar regionTarget)
        {
            var menuItem = view as Menu;
            if (menuItem != null)
            {
                foreach (var menu in menuItem.Items)
                {
                    var panelItem = new RadPanelBarItem
                    {
                        Header = menu.Title,
                        Command = menu.Command,
                        CommandParameter = menu.CommandParameter,
                        Tag = menu.UniqId
                    };
                    foreach (var child in menu.Children)
                    {
                        var childItem = new RadPanelBarItem
                        {
                            Header = new { child.Title, child.IconPath },
                            HeaderTemplate = menuItem.ItemTemplate,
                            Command = child.Command,
                            CommandParameter = child.CommandParameter,
                            Tag = child.UniqId
                        };
                        panelItem.Items.Add(childItem);
                    }
                    regionTarget.Items.Add(panelItem);
                }
            }
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
