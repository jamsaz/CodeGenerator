using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Documents;
using Prism.Regions;
using Telerik.Windows.Controls;
using $saferootprojectname$.Core.Controls;

namespace $safeprojectname$.Prism
{
    public class DockingAdapter : RegionAdapterBase<RadPaneGroup>
    {
        public DockingAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {

        }

        protected override void Adapt(IRegion region, RadPaneGroup regionTarget)
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

        private void RemoveViewFromRegion(object view, RadPaneGroup regionTarget)
        {
            var form = view as Form;
            if (form?.Name != null)
            {
                var findPane = regionTarget.Items.Cast<RadPane>().FirstOrDefault(x => x.Name == form.Name);
                //regionTarget.Items.Remove(findPane);
                findPane?.RemoveFromParent();
            }
        }

        private void AddViewToRegion(object view, RadPaneGroup regionTarget)
        {
            var form = view as Form;
            if (form?.Name != null)
            {
                if (regionTarget.Items.Cast<RadPane>().All(x => x.Name != form.Name))
                {
                    var pane = new RadPane
                    {
                        Content = form.Content,
                        Header = form.Header,
                        CanUserPin = false,
                        CanUserClose = true,
                        CanFloat = false,
                        Name = form.Name,
                        DataContext = form.DataContext
                    };
                    regionTarget.Items.Add(pane);
                }
            }
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
