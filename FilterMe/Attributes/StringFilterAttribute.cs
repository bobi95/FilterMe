using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterMe.Attributes
{
    public sealed class StringFilterAttribute : FilterAttribute
    {
        public StringAction Action { get; set; }

        public StringFilterAttribute()
        {
            Action = StringAction.Contains;
        }
    }
}
