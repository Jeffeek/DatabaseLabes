using System.IO;
using System.Text;
using System.Windows;
using DatabaseLabes.SharedKernel.DI;
using DatabaseLabes.SharedKernel.Shared;
using First_10.BusinessLogic;
using First_10.Settings;
using First_10.ViewModels;
using First_10.ViewModels.Models;
using First_10.Views;
using Newtonsoft.Json;
using Prism;
using Prism.Ioc;
using Prism.Unity;

namespace First_10
{
    public partial class App : PrismApplicationBase
    {
        #region Overrides of PrismApplicationBase

        /// <inheritdoc />
        protected override IContainerExtension CreateContainerExtension() =>
            new UnityContainerExtension();

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterScoped<AppConfiguration>(() =>
                                                               {
                                                                   using var stream =
                                                                       new StreamReader(File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
                                                                                                          "appconfig.json")),
                                                                                        Encoding.ASCII);

                                                                   return JsonConvert.DeserializeObject<AppConfiguration>(stream.ReadToEnd());
                                                               });

            var config = Container.Resolve<AppConfiguration>();
            containerRegistry.AddDbContextFactory(SharedDbContextOptions.GetOptions(config.ConnectionString));
            containerRegistry.AddDbContext(config.ConnectionString, false);
            containerRegistry.AddAutoMapper();
            containerRegistry.RegisterDialog<CustomDialog, CustomDialogViewModel>();
            containerRegistry.Register<FileDialogService>();
            containerRegistry.Register<ImageService>();
        }

        /// <inheritdoc />
        protected override Window CreateShell() =>
            Container.Resolve<MainWindow>();

        #endregion
    }
}
