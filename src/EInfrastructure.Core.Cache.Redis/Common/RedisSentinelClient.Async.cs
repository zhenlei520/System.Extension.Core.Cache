﻿using System;
using System.Threading.Tasks;
using EInfrastructure.Core.Cache.Redis.Common.Internal;
using EInfrastructure.Core.Cache.Redis.Common.Internal.Commands;

namespace EInfrastructure.Core.Cache.Redis.Common
{
    public partial class RedisSentinelClient
    {
        /// <summary>
        /// Connect to the remote host
        /// </summary>
        /// <returns>True if connected</returns>
        public async Task<bool> ConnectAsync()
        {
            return  await _connector.ConnectAsync();
        }

        /// <summary>
        /// Call arbitrary Sentinel command (e.g. for a command not yet implemented in this library)
        /// </summary>
        /// <param name="command">The name of the command</param>
        /// <param name="args">Array of arguments to the command</param>
        /// <returns>Redis unified response</returns>
        public async Task<object> CallAsync(string command, params string[] args)
        {
            return  await WriteAsync(new RedisObject(command, args));
        }

       async Task<T> WriteAsync<T>(RedisCommand<T> command)
        {
            return  await _connector.CallAsync(command);
        }

        #region sentinel
        /// <summary>
        /// Ping the Sentinel server
        /// </summary>
        /// <returns>Status code</returns>
        public async Task<string> PingAsync()
        {
            return  await WriteAsync(RedisCommands.Ping());
        }

        /// <summary>
        /// Get a list of monitored Redis masters
        /// </summary>
        /// <returns>Redis master info</returns>
        public async Task<RedisMasterInfo[]> MastersAsync()
        {
            return  await WriteAsync(RedisCommands.Sentinel.Masters());
        }

        /// <summary>
        /// Get information on the specified Redis master
        /// </summary>
        /// <param name="masterName">Name of the Redis master</param>
        /// <returns>Master information</returns>
        public async Task<RedisMasterInfo> MasterAsync(string masterName)
        {
            return  await WriteAsync(RedisCommands.Sentinel.Master(masterName));
        }

        /// <summary>
        /// Get a list of other Sentinels known to the current Sentinel
        /// </summary>
        /// <param name="masterName">Name of monitored master</param>
        /// <returns>Sentinel hosts and ports</returns>
        public async Task<RedisSentinelInfo[]> SentinelsAsync(string masterName)
        {
            return  await WriteAsync(RedisCommands.Sentinel.Sentinels(masterName));
        }


        /// <summary>
        /// Get a list of monitored Redis slaves to the given master
        /// </summary>
        /// <param name="masterName">Name of monitored master</param>
        /// <returns>Redis slave info</returns>
        public async Task<RedisSlaveInfo[]> SlavesAsync(string masterName)
        {
            return await  WriteAsync(RedisCommands.Sentinel.Slaves(masterName));
        }

        /// <summary>
        /// Get the IP and port of the current master Redis server
        /// </summary>
        /// <param name="masterName">Name of monitored master</param>
        /// <returns>IP and port of master Redis server</returns>
        public async Task<Tuple<string, int>> GetMasterAddrByNameAsync(string masterName)
        {
            return  await WriteAsync(RedisCommands.Sentinel.GetMasterAddrByName(masterName));
        }

        /// <summary>
        /// Get master state information
        /// </summary>
        /// <param name="ip">Host IP</param>
        /// <param name="port">Host port</param>
        /// <param name="currentEpoch">Current epoch</param>
        /// <param name="runId">Run ID</param>
        /// <returns>Master state</returns>
        public async Task<RedisMasterState> IsMasterDownByAddrAsync(string ip, int port, long currentEpoch, string runId)
        {
            return await  WriteAsync(RedisCommands.Sentinel.IsMasterDownByAddr(ip, port, currentEpoch, runId));
        }

        /// <summary>
        /// Clear state in all masters with matching name
        /// </summary>
        /// <param name="pattern">Master name pattern</param>
        /// <returns>Number of masters that were reset</returns>
        public async Task<long> ResetAsync(string pattern)
        {
            return  await WriteAsync(RedisCommands.Sentinel.Reset(pattern));
        }

        /// <summary>
        /// Force a failover as if the master was not reachable, and without asking for agreement from other sentinels
        /// </summary>
        /// <param name="masterName">Master name</param>
        /// <returns>Status code</returns>
        public async Task<string> FailoverAsync(string masterName)
        {
            return await  WriteAsync(RedisCommands.Sentinel.Failover(masterName));
        }

        /// <summary>
        /// Start monitoring a new master
        /// </summary>
        /// <param name="name">Master name</param>
        /// <param name="port">Master port</param>
        /// <param name="quorum">Quorum count</param>
        /// <returns>Status code</returns>
        public async Task<string> MonitorAsync(string name, int port, int quorum)
        {
            return  await WriteAsync(RedisCommands.Sentinel.Monitor(name, port, quorum));
        }

        /// <summary>
        /// Remove the specified master
        /// </summary>
        /// <param name="name">Master name</param>
        /// <returns>Status code</returns>
        public async Task<string> RemoveAsync(string name)
        {
            return await  WriteAsync(RedisCommands.Sentinel.Remove(name));
        }

        /// <summary>
        /// Change configuration parameters of a specific master
        /// </summary>
        /// <param name="masterName">Master name</param>
        /// <param name="option">Config option name</param>
        /// <param name="value">Config option value</param>
        /// <returns>Status code</returns>
        public async Task<string> SetAsync(string masterName, string option, string value)
        {
            return await  WriteAsync(RedisCommands.Sentinel.Set(masterName, option, value));
        }
        #endregion
    }
}
