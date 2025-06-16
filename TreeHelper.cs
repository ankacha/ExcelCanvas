using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

public static class TreeHelper
{
    public static IReadOnlyList<DependencyObject> GetAncestryPath(DependencyObject? child)
    {
        if (child is null)
        {
            return System.Array.Empty<DependencyObject>();
        }

        var list = new List<DependencyObject>();
        while (child != null)
        {
            list.Add(child);

            DependencyObject? parent = (child is Visual or Visual3D)
                ? VisualTreeHelper.GetParent(child)
                : LogicalTreeHelper.GetParent(child);

            if (parent is null)
            {
                break;
            }

            child = parent;
        }

        list.Reverse();
        return list;
    }
}