﻿//Author Maxim Kuzmin//makc//

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Makc2020.Apps.Automation.Base.App
{
    /// <summary>
    /// Приложение. Хостируемый сервис.
    /// </summary>    
    public class AppHostedService : IHostedService
    {
        #region Properties

        private static AppServer Server => AppServer.Instance;

        private IServiceScope ServiceScope { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AppHostedService(IServiceProvider serviceProvider)
        {
            Server.Configure(serviceProvider);

            ServiceScope = serviceProvider.CreateScope();
        }

        #endregion Constructors

        #region Public methods

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Server.OnStarted();

            Console.WriteLine();
            Console.Write("Press <Enter> to exit");
            Console.ReadLine();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Server.OnStopped();

            ServiceScope.Dispose();

            return Task.CompletedTask;
        }

        #endregion Public methods

        #region Private methods

        private TService GetService<TService>()
        {
            return ServiceScope.ServiceProvider.GetService<TService>();
        }

        #endregion Private methods
    }
}
