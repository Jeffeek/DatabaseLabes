using System.Windows;
using DatabaseLabes.SharedKernel.DI;
using First_10.ViewModels;
using First_10.ViewModels.Models;
using First_10.Views;
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
            containerRegistry.AddDbContext("server=localhost;initial catalog=First_10;Trusted_Connection=True;multipleactiveresultsets=True;application name=EntityFramework", false);
            containerRegistry.AddAutoMapper();
            containerRegistry.RegisterForNavigation<ProductWindowViewModel, ProductWindow>("ProductWindow");
        }

        /// <inheritdoc />
        protected override Window CreateShell() =>
            Container.Resolve<MainWindow>();

        #endregion
    }
}
