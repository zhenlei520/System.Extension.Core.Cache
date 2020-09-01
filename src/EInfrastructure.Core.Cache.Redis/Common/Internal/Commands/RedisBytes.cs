﻿
using EInfrastructure.Core.Cache.Redis.Common.Internal.IO;

namespace EInfrastructure.Core.Cache.Redis.Common.Internal.Commands
{
    class RedisBytes : RedisCommand<byte[]>
    {
        public RedisBytes(string command, params object[] args)
            : base(command, args)
        { }

        public override byte[] Parse(RedisReader reader)
        {
            return reader.ReadBulkBytes(true);
        }
    }
}
