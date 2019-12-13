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
            return constants.Select(x => x.Adapt<Constant>()).ToList().Select(x =>
            {
                if (!x.IsSecret || x.IsEncrypted)
                {
                    return x;
                }

                x.Value = protector.Protect(x.Value.ToString());
                x.IsEncrypted = true;
                return x;
            }).ToList();
        }

        public static IEnumerable<Constant> DecryptConstants(this IEnumerable<Constant> constants,
            IDataProtector protector)
        {
            return constants.Select(x => x.Adapt<Constant>()).ToList().Select(x =>
            {
                if (!x.IsSecret || !x.IsEncrypted)
                {
                    return x;
                }

                x.Value = protector.Unprotect(x.Value.ToString());
                x.IsEncrypted = false;
                return x;
            }).ToList();
        }
    }
}