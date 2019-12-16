using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Nodegem.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static void Remove<T>(this DbSet<T> set, Expression<Func<T, bool>> predicate) where T : class
        {
            set.Remove(set.Single(predicate));
        }
    }
}