﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NewLife.Agent
{
    /// <summary>服务主机</summary>
    public interface IHost
    {
        /// <summary>服务集合</summary>
        IList<IHostedService> Services { get; }

        /// <summary>开始</summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>结束</summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StopAsync(CancellationToken cancellationToken = default);

        Boolean? IsInstalled(String serviceName);
        Boolean? IsRunning(String serviceName);
        void Install(IHostedService service);
        void Uninstall(String serviceName);
        void Start(String serviceName);
        void Stop(String serviceName);
    }

    /// <summary>服务主机。用于管理控制服务</summary>
    public abstract class Host : DisposeBase, IHost
    {
        /// <summary>服务集合</summary>
        public IList<IHostedService> Services { get; } = new List<IHostedService>();

        /// <summary>销毁</summary>
        /// <param name="disposing"></param>
        protected override void Dispose(Boolean disposing)
        {
            base.Dispose(disposing);

            foreach (var item in Services)
            {
                item.TryDispose();
            }
        }

        /// <summary>开始</summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StartAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in Services)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await item.StartAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>结束</summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task StopAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in Services)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await item.StopAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual Boolean? IsInstalled(String serviceName) => null;

        public virtual Boolean? IsRunning(String serviceName) => null;
        public virtual void Install(IHostedService service) { }
        public virtual void Uninstall(String serviceName) { }
        public virtual void Start(String serviceName) { }
        public virtual void Stop(String serviceName) { }
    }
}