using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Nodegem.Common.Data;

namespace Nodegem.Services.Extensions
{
    public static class ConstantExtensions
    {
        public static IEnumerable<Constant> EncryptConstants(this IEnumerable<Constant> constants,
            IDataProtector protector)
        {
            return constants.Select(x =>
            {
                if (!x.IsSecret || x.IsEncrypted)
                {
                    return x;
                }

                var constant = x.Adapt<Constant>();
                constant.Value = protector.Protect(x.Value.ToString());
                constant.IsEncrypted = true;
                return constant;
            });
        }

        public static IEnumerable<Constant> DecryptConstants(this IEnumerable<Constant> constants,
            IDataProtector protector)
        {
            return constants.Select(x =>
            {
                if (!x.IsSecret || !x.IsEncrypted)
                {
                    return x;
                }

                var constant = x.Adapt<Constant>();
                constant.Value = protector.Unprotect(x.Value.ToString());
                constant.IsEncrypted = false;
                return constant;
            });
        }
    }
}