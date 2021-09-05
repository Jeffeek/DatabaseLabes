using System.Windows;
using DatabaseLabes.SharedKernel.DI;
using DatabaseLabes.SharedKernel.Shared;
using First_10.BusinessLogic;
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
            containerRegistry.AddDbContextFactory(SharedDbContextOptions.GetOptions("server=DESKTOP-1MKQIKU\\MSSQLSERVER01;initial catalog=First_10;Trusted_Connection=True;multipleactiveresultsets=True;application name=EntityFramework"));
            containerRegistry.AddDbContext("server=DESKTOP-1MKQIKU\\MSSQLSERVER01;initial catalog=First_10;Trusted_Connection=True;multipleactiveresultsets=True;application name=EntityFramework", false);
            containerRegistry.AddAutoMapper();
            containerRegistry.RegisterDialog<CustomDialog, CustomDialogViewModel>();
            containerRegistry.Register<FileDialogService>();
            containerRegistry.Register<ImageService>();
            //containerRegistry.RegisterForNavigation<ProductWindowViewModel, ProductWindow>("ProductWindow");
        }

        /// <inheritdoc />
        protected override Window CreateShell() =>
            Container.Resolve<MainWindow>();

        #endregion
    }
}
