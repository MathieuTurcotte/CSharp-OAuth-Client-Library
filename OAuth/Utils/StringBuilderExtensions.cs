using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth.Utils
{
    static class StringBuilderExtensions
    {
        public static StringBuilder HeaderField(this StringBuilder builder, string name, string value)
        {
            return builder.Append(name).Append("=\"").Append(value).Append("\"");
        }

        public static StringBuilder Comma(this StringBuilder builder)
        {
            return builder.Append(',');
        }
    }
}
