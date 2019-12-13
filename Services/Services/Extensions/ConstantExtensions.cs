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
            var nonSecrets = constants.Where(x => !x.IsSecret).ToList();
            var encryptedSecrets = constants.Where(x => x.IsSecret && !x.IsEncrypted).Select(x =>
            {
                var constant = x.Adapt<Constant>();
                constant.Value = protector.Protect(x.Value.ToString());
                constant.IsEncrypted = true;
                return constant;
            }).ToList();

            return nonSecrets.Concat(encryptedSecrets);
        }

        public static IEnumerable<Constant> DecryptConstants(this IEnumerable<Constant> constants,
            IDataProtector protector)
        {
            var nonSecrets = constants.Where(x => !x.IsSecret).ToList();
            var encryptedSecrets = constants.Where(x => x.IsSecret && x.IsEncrypted).Select(x =>
            {
                var constant = x.Adapt<Constant>();
                constant.Value = protector.Unprotect(x.Value.ToString());
                constant.IsEncrypted = false;
                return constant;
            }).ToList();

            return nonSecrets.Concat(encryptedSecrets);
        }
    }
}