﻿//Author Maxim Kuzmin//makc//

using Makc2020.Apps.Api.Base.App.Samples.Core.Base.Resources;
using Makc2020.Apps.Api.Base.App.Samples.Mods.DummyMain.Base.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Makc2020.Apps.Api.Base.App
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

            var samples = new AppSample[]
            {
                GetService<AppSampleCoreBaseResourceErrors>(),
                GetService<AppSampleModDummyMainBaseJobItemGet>(),
                GetService<AppSampleModDummyMainBaseJobListGet>()
            };

            foreach (var sample in samples)
            {
                sample.Run();
            }

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
