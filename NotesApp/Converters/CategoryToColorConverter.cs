using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using NotesApp.Models;
using System.Globalization;

namespace NotesApp.Converters
{
    public class CategoryToColorConverter : IValueConverter
    {
        // Same color mapping as in NotesGroup
        private static readonly Dictionary<string, string> CategoryColorMap = new();
        private static readonly List<string> CategoryColors = new()
        {
            "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7",
            "#DDA0DD", "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E9",
            "#F8C471", "#82E0AA", "#F1948A", "#85C1E9", "#D2B4DE"
        };

        private static int colorIndex = 0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string categoryName && !string.IsNullOrEmpty(categoryName))
            {
                if (!CategoryColorMap.ContainsKey(categoryName))
                {
                    CategoryColorMap[categoryName] = CategoryColors[colorIndex % CategoryColors.Count];
                    colorIndex++;
                }
                return Color.FromArgb(CategoryColorMap[categoryName]);
            }

            return Color.FromArgb("#CCCCCC"); // Default gray color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isCompleted && isCompleted) ? TextDecorations.Strikethrough : TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
