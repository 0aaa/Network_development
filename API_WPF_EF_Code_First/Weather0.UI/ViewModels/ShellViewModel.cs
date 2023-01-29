using ApiIntro0.BLL.DTO;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Weather0.BLL.Services;

namespace Weather0UI
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private CollectionView _personagesView;
        private PersonageDTO _selectedPersonage;
        private int _currentQuantity;
        private WeatherService PersonagesService { get; set; }
        public ObservableCollection<PersonageDTO> Personages { get; private set; }
        public CollectionView PersonagesView
        {
            get => _personagesView;
            private set
            {
                _personagesView = value;
                NotifyOfPropertyChange("PersonagesView");
            }
        }
        public PersonageDTO SelectedPersonage
        {
            get => _selectedPersonage;
            set
            {
                _selectedPersonage = value;
                NotifyOfPropertyChange("SelectedPersonage");
            }
        }
        public int[] PersonageQuantitiesArr { get; private set; }
        public int CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                _currentQuantity = value;
                GetPersonages();
            }
        }

        public ShellViewModel()
        {
            PersonagesService = new WeatherService();
            PersonageQuantitiesArr = new int[10];
            for (int i = 0; i < PersonageQuantitiesArr.Length; i++)
            {
                PersonageQuantitiesArr[i] = i + 1;
            }
            CurrentQuantity = 1;

            GetPersonages();
        }
        private void GetPersonages()
        {
            Personages = new ObservableCollection<PersonageDTO>(PersonagesService.GetPersonages(CurrentQuantity));
            PersonagesView = (CollectionView)CollectionViewSource.GetDefaultView(Personages);
        }
    }
}