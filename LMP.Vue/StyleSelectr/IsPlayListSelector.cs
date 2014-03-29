using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LMP.Metier.ListItem;

namespace LMP.Vue.StyleSelectr
{
    public class IsPlayListSelector:StyleSelector
    {
        public Style HidenStyle { get; set; }
        public Style ShowStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is PlayList)
                return ShowStyle;
            return HidenStyle;
        }
    }
}
