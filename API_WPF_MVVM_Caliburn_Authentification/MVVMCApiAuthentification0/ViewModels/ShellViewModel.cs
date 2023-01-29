using ComicLbrry;
using MVVMCApiAuthentification0.Infrastructure;
using MVVMCApiAuthentification0.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

// Не выполнено требование на получение объектов в задаваемом пользователем интервале.

namespace MVVMCApiAuthentification0.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private static readonly int COLUMN_WIDTH = 352;

        private readonly ApiHandlingCls _apiHandling;

        private PersonageCls _selectedPersonage;
        private int _startIndex;

        private readonly ObservableCollection<PersonageCls> _personagesObsrvbleClctn;
        private readonly ObservableCollection<ComicBookCls> _booksObsrvbleClctn;

        private int _heroesColumnWidth;
        private int _comicsColumnWidth;

        public CollectionView ItemsClctnVw { get; set; }
        public CollectionView BooksClctnVw { get; set; }

        public RelayCommand RequestPreviousItemsCmd { get; set; }
        public RelayCommand RequestNextItemsCmd { get; set; }
        public ICommand ShowHideColumnsCmd { get; set; }

        public PersonageCls SelectedPersonage
        {
            get => _selectedPersonage;
            set
            {
                _selectedPersonage = value;
                NotifyOfPropertyChange("SelectedPersonage");
                GetBooks();
                if (_booksObsrvbleClctn.Count > 0)
                {
                    ShowHideColumnsCmd.Execute(null);
                }
            }
        }
        public int HeroesColumnWidth
        {
            get => _heroesColumnWidth;
            set
            {
                _heroesColumnWidth = value;
                NotifyOfPropertyChange("HeroesColumnWidth");
            }
        }
        public int ComicsColumnWidth
        {
            get => _comicsColumnWidth;
            set
            {
                _comicsColumnWidth = value;
                NotifyOfPropertyChange("ComicsColumnWidth");
            }
        }
        public ShellViewModel()
        {
            _apiHandling = new ApiHandlingCls();

            _selectedPersonage = new PersonageCls();
            _personagesObsrvbleClctn = new ObservableCollection<PersonageCls>();
            _booksObsrvbleClctn = new ObservableCollection<ComicBookCls>();

            ItemsClctnVw = (CollectionView)CollectionViewSource.GetDefaultView(_personagesObsrvbleClctn);
            BooksClctnVw = (CollectionView)CollectionViewSource.GetDefaultView(_booksObsrvbleClctn);

            ShowHideColumnsCmd = new RelayCommand(actn => Task.Run(() => ChangeColumnWidths()));
            RequestNextItemsCmd = new RelayCommand(actn =>
            {
                if (_startIndex <= 20)
                {
                    RequestPreviousItemsCmd.CanExecuteFlag = true;
                }
                _startIndex += 20;
                GetItems();
            });
            RequestPreviousItemsCmd = new RelayCommand(actn =>
            {
                _startIndex -= 20;
                if (_startIndex < 20)
                {
                    RequestPreviousItemsCmd.CanExecuteFlag = false;
                }
                GetItems();
            })
            { CanExecuteFlag = false };

            HeroesColumnWidth = COLUMN_WIDTH;
            GetItems();
        }
        private void ChangeColumnWidths()
        {
            int valueToAdd = 1;
            if (ComicsColumnWidth > HeroesColumnWidth)
            {
                valueToAdd = -1;
            }
            for (int i = 0; i < COLUMN_WIDTH; i++)
            {
                HeroesColumnWidth -= valueToAdd;
                ComicsColumnWidth += valueToAdd;
                Thread.Sleep(1);//
            }
        }
        private void GetItems()
        {
            _apiHandling.GetPersonages(_startIndex);
            _personagesObsrvbleClctn.Clear();

            for (int i = 0; i < 20; i++)
            {
                _personagesObsrvbleClctn.Add(_apiHandling.GetPersonages()[_startIndex + i]);
            }
        }
        private void GetBooks()
        {
            _booksObsrvbleClctn.Clear();
            _apiHandling.GetBooks(_selectedPersonage.Id);

            foreach (ComicBookCls book in _apiHandling.GetBooks())
            {
                _booksObsrvbleClctn.Add(book);
            }
        }
    }
}