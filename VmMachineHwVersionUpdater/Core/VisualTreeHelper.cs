using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public static class VisualTreeHelper
    {
        /// <summary>
        ///     Get visual tree children of a type
        /// </summary>
        /// <typeparam name="T">Visual tree children type</typeparam>
        /// <param name="current">A DependencyObject reference</param>
        /// <param name="children">A collection of one visual tree children type</param>
        private static void GetVisualChildren<T>(DependencyObject current, ICollection<T> children)
            where T : DependencyObject
        {
            if (current == null)
            {
                return;
            }

            if (current.GetType() == typeof(T))
            {
                children.Add((T) current);
            }

            for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(current); i++)
            {
                GetVisualChildren(System.Windows.Media.VisualTreeHelper.GetChild(current, i), children);
            }
        }

        /// <summary>
        ///     Get visual tree children of a type
        /// </summary>
        /// <typeparam name="T">Visual tree children type</typeparam>
        /// <param name="current">A DependencyObject reference</param>
        /// <returns>Returns a collection of one visual tree children type</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static Collection<T> GetVisualChildren<T>(DependencyObject current)
            where T : DependencyObject
        {
            if (current == null)
            {
                return null;
            }

            var children = new Collection<T>();

            GetVisualChildren(current, children);

            return children;
        }

        /// <summary>
        ///     Get the first visual child from a FrameworkElement Template
        /// </summary>
        /// <typeparam name="TP">FrameworkElement type</typeparam>
        /// <typeparam name="T">FrameworkElement type</typeparam>
        /// <param name="templatedParent">A FrameworkElement type-of P</param>
        /// <returns>Returns a FrameworkElement visual child type-of T if found one; returns null otherwise</returns>
        // ReSharper disable once IdentifierTypo
        // ReSharper disable once UnusedMember.Global
        public static T GetVisualChild<T, TP>(TP templatedParent)
            where T : FrameworkElement
            where TP : FrameworkElement
        {
            var children = GetVisualChildren<T>(templatedParent);

            foreach (var child in children)
            {
                if (child.TemplatedParent == templatedParent)
                {
                    return child;
                }
            }

            return null;
        }
    }
}