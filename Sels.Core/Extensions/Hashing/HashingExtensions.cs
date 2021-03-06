﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sels.Core.Extensions.Hashing
{
    public static class HashingExtensions
    {
        #region GenerateHash
        public static string GenerateHash<THash>(object sourceObject) where THash : HashAlgorithm
        {
            sourceObject.ValidateVariable(nameof(sourceObject));
            var hashType = typeof(THash);
            hashType.ValidateVariable(x => !x.Equals(typeof(HashAlgorithm)), () => $"Please use an implementation of {typeof(HashAlgorithm)}");

            using (var hash = HashAlgorithm.Create(hashType.Name))
            {
                var hashedBytes = hash.ComputeHash(sourceObject.GetBytes());

                return hashedBytes.Select(x => x.ToString("x2")).JoinString(string.Empty);
            }
        }
        #endregion
    }
}
