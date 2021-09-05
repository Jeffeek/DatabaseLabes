using Prism.Mvvm;

namespace First_10.ViewModels
{
    public class InputDialogWindowViewModel<TValue> : BindableBase
    {
        private TValue? _value;

        public TValue? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}