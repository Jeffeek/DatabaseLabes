using Prism.Mvvm;

namespace First_10.ViewModels
{
    public class NumberDialogViewModel : BindableBase
    {
        private int? _number;

        public int? Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
    }
}