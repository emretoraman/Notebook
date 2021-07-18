using System;

namespace Notebook.SharedKernel.Extensions
{
    public static class Extensions
    {
        public static T Select<TIn, T>(this TIn from, Func<TIn, T> selector)
        {
            return selector(from);
        }
    }
}
