using System.Windows;

namespace First_10.BusinessLogic
{
    public class MessageBoxService
    {
        public void ShowError(string message) => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        public void ShowWarning(string message) => MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

        public bool ShowWarningWithAnswer(string message)
        {
            var result = MessageBox.Show(message,
                                         "Warning",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Information,
                                         MessageBoxResult.No);

            return result == MessageBoxResult.Yes;
        }

        public bool AreYouSure(string message)
        {
            var result = MessageBox.Show(message,
                                         "Are you sure?",
                                         MessageBoxButton.OKCancel,
                                         MessageBoxImage.Information,
                                         MessageBoxResult.Cancel);

            return result == MessageBoxResult.OK;
        }

        public void ShowInformation(string message) => MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}