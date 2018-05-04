using System;
using System.Collections.Generic;
using System.Text;

namespace Polytech.Common.Extension
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Gets a base 64 string from a <see langword="long"/>
        /// </summary>
        /// <param name="num">The number to convert.</param>
        /// <returns>A base 64 representation of the number.</returns>
        public static string GetBase64String(this long num)
        {
            byte[] bytes = BitConverter.GetBytes(num);

            string result = Convert.ToBase64String(bytes);

            return result;
        }
    }
}
