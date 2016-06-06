// THIS FILE MODIFIED FOR USE IN REDAKT CMS FROM THE ORIGINAL AT https://github.com/mongodb/mongo-csharp-driver/blob/master/src/MongoDB.Bson/ObjectModel/ObjectId.cs
/* Copyright 2010-2016 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

namespace Redakt.Model
{
    internal static class ObjectId
    {
        private static readonly DateTime __unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly int __staticMachine = (GetMachineHash() + GetAppDomainId()) & 0x00ffffff;
        private static readonly short __staticPid = GetPid();
        private static int __staticIncrement = (new Random()).Next();

        // public static methods
        /// <summary>
        /// Generates a new ObjectId with a unique value.
        /// </summary>
        /// <returns>An ObjectId.</returns>
        public static string GenerateNewId()
        {
            return GenerateNewId(GetTimestampFromDateTime(DateTime.UtcNow));
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the timestamp component based on a given DateTime).
        /// </summary>
        /// <param name="timestamp">The timestamp component (expressed as a DateTime).</param>
        /// <returns>An ObjectId.</returns>
        public static string GenerateNewId(DateTime timestamp)
        {
            return GenerateNewId(GetTimestampFromDateTime(timestamp));
        }

        /// <summary>
        /// Generates a new ObjectId with a unique value (with the given timestamp).
        /// </summary>
        /// <param name="timestamp">The timestamp component.</param>
        /// <returns>An ObjectId.</returns>
        public static string GenerateNewId(int timestamp)
        {
            int increment = Interlocked.Increment(ref __staticIncrement) & 0x00ffffff; // only use low order 3 bytes
            return ToHexString(Pack(timestamp, __staticMachine, __staticPid, increment));
        }

        /// <summary>
        /// Packs the components of an ObjectId into a byte array.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="machine">The machine hash.</param>
        /// <param name="pid">The PID.</param>
        /// <param name="increment">The increment.</param>
        /// <returns>A byte array.</returns>
        public static byte[] Pack(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("machine", "The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException("increment", "The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            byte[] bytes = new byte[12];
            bytes[0] = (byte)(timestamp >> 24);
            bytes[1] = (byte)(timestamp >> 16);
            bytes[2] = (byte)(timestamp >> 8);
            bytes[3] = (byte)(timestamp);
            bytes[4] = (byte)(machine >> 16);
            bytes[5] = (byte)(machine >> 8);
            bytes[6] = (byte)(machine);
            bytes[7] = (byte)(pid >> 8);
            bytes[8] = (byte)(pid);
            bytes[9] = (byte)(increment >> 16);
            bytes[10] = (byte)(increment >> 8);
            bytes[11] = (byte)(increment);
            return bytes;
        }

        // private static methods
        private static int GetAppDomainId()
        {
#if NET45
            return AppDomain.CurrentDomain.Id;
#else
            return 1;
#endif
        }

        /// <summary>
        /// Gets the current process id.  This method exists because of how CAS operates on the call stack, checking
        /// for permissions before executing the method.  Hence, if we inlined this call, the calling method would not execute
        /// before throwing an exception requiring the try/catch at an even higher level that we don't necessarily control.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetCurrentProcessId()
        {
#if NET45
            return Process.GetCurrentProcess().Id;
#else
            return 1;
#endif
        }

        private static int GetMachineHash()
        {
            // use instead of Dns.HostName so it will work offline
            var machineName = GetMachineName();
            return 0x00ffffff & machineName.GetHashCode(); // use first 3 bytes of hash
        }

        private static string GetMachineName()
        {
#if NET45
            return Environment.MachineName;
#else
            return Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "";
#endif
        }

        private static short GetPid()
        {
            try
            {
                return (short)GetCurrentProcessId(); // use low order two bytes only
            }
            catch (SecurityException)
            {
                return 0;
            }
        }

        private static int GetTimestampFromDateTime(DateTime timestamp)
        {
            var secondsSinceEpoch = (long)Math.Floor((ToUniversalTime(timestamp) - __unixEpoch).TotalSeconds);
            if (secondsSinceEpoch < int.MinValue || secondsSinceEpoch > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timestamp");
            }
            return (int)secondsSinceEpoch;
        }

        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}
