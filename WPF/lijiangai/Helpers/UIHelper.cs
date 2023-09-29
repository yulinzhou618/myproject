using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIVisualwfpnew.Helpers
{
    public static class UIHelper
    {
        /// <summary>
        /// 获取某个元素下的所有指定类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetChild<T>(DependencyObject @object) where T : FrameworkElement
        {
            var childrens = LogicalTreeHelper.GetChildren(@object);
            if (childrens == null)
                return null;

            List<T> result = new List<T>();
            if (@object is ContentControl cc)
            {
                var temp = cc.Content;
                if (temp == null || !(temp is DependencyObject dobj))
                    return null;
                return GetChild<T>(dobj);
            }
            foreach (var child in childrens)
            {
                if (child == null || !(child is DependencyObject dobj))
                    continue;

                if (child is T c)
                    result.Add(c);

                var temp = GetChild<T>(dobj);
                if (temp != null)
                    result.AddRange(temp);
            }

            return result;
        }

        /// <summary>
        /// 根据控件类型，获取到第一个符合要求的控件
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="object">父级控件</param>
        /// <returns></returns>
        public static T GetFirstChild<T>(DependencyObject @object) where T : FrameworkElement
        {
            var childrens = LogicalTreeHelper.GetChildren(@object);
            if (childrens == null)
                return null;

            if (@object is ContentControl cc)
            {
                var temp = cc.Content;
                if (temp == null || !(temp is DependencyObject dobj))
                    return null;

                return GetFirstChild<T>(dobj);
            }
            else
            {
                foreach (var child in childrens)
                {
                    if (child == null || !(child is DependencyObject dobj))
                        continue;

                    if (child is T c)
                        return c;

                    var temp = GetFirstChild<T>(dobj);
                    if (temp != null)
                        return temp;
                }
            }


            return null;
        }
    }
}
