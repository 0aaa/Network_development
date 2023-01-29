using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Data;
using System.Windows.Input;
using WpfTcpLstnrClient0.Infrastructure;
using WpfTcpLstnrClient0.Models;

namespace WpfTcpLstnrClient0.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private const string LOCALHOST = "127.0.0.1";
        private const int PORT = 25545;
        private readonly TcpClient _clnt;
        private readonly BinaryFormatter _bnryFrmtr;
        private string _qte;
        private ICommand _loggingCmd;
        private ICommand _quoteRequestCmd;
        private ObservableCollection<string> _responseObsrvbleClctn;
        private ObservableCollection<string> ResponseObsrvbleClctn
        {
            get => _responseObsrvbleClctn;
            set
            {
                _responseObsrvbleClctn = value;
                NotifyOfPropertyChange("ResponseObsrvbleClctn");
            }
        }
        public CollectionView ResponseClctnVw { get; set; }
        public ICommand LoggingCmd
        {
            get => _loggingCmd;
            set
            {
                _loggingCmd = value;
                NotifyOfPropertyChange("LoggingCmd");
            }
        }
        public ICommand QuoteRequestCmd
        {
            get => _quoteRequestCmd;
            set
            {
                _quoteRequestCmd = value;
                NotifyOfPropertyChange("QuoteRequestCmd");
            }
        }
        public ICommand ExitCmd { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public ShellViewModel()
        {
            _clnt = new TcpClient();
            _bnryFrmtr = new BinaryFormatter();
            ResponseObsrvbleClctn = new ObservableCollection<string>();
            ResponseClctnVw = (CollectionView)CollectionViewSource.GetDefaultView(ResponseObsrvbleClctn);

            ExitCmd = new RelayCommand(actn => Exit());
            LoggingCmd = new RelayCommand(actn => Logging(), false);
            QuoteRequestCmd = new RelayCommand(actn => QuoteRequest(), false);
            Pseudo = string.Empty;
            Password = string.Empty;
            _clnt.Connect(new IPEndPoint(IPAddress.Parse(LOCALHOST), PORT));
            ResponseObsrvbleClctn.Add("the client launch");
            string serverReadiness = (string)_bnryFrmtr.Deserialize(_clnt.GetStream());
            if (serverReadiness == "sorry the server is full. please try again later")
            {
                serverReadiness += "\nplease press Exit button";
            }
            else
            {
                LoggingCmd = new RelayCommand(actn => Logging());
            }
            ResponseObsrvbleClctn.Add(serverReadiness);
        }
        private void Logging()
        {
            _bnryFrmtr.Serialize(_clnt.GetStream(), Pseudo);
            _bnryFrmtr.Serialize(_clnt.GetStream(), Password);
            _qte = (string)_bnryFrmtr.Deserialize(_clnt.GetStream());
            if (_qte != "wrong pseudo/password")
            {
                LoggingCmd = new RelayCommand(actn => ToString(), false);
                QuoteRequestCmd = new RelayCommand(actn => QuoteRequest());
            }
            if (_qte == "the limit reached")
            {
                QuoteRequestCmd = new RelayCommand(actn => ToString(), false);
                _qte += "\nplease press Exit button";
            }
            ResponseObsrvbleClctn.Add("the server response:\n" + _qte);
        }

        private void QuoteRequest()
        {
            // Возможным предстаёт применение кодов сообщений на месте передачи их в строковом виде.
            _bnryFrmtr.Serialize(_clnt.GetStream(), "request");
            _qte = (string)_bnryFrmtr.Deserialize(_clnt.GetStream());
            if (_qte == "the limit reached")
            {
                QuoteRequestCmd = new RelayCommand(actn => ToString(), false);
                _qte += "\nplease press Exit button";
            }
            ResponseObsrvbleClctn.Add("the server response:\n" + _qte);
        }

        private void Exit()
        {
            if (LoggingCmd.CanExecute(null) || QuoteRequestCmd.CanExecute(null))
            {
                _bnryFrmtr.Serialize(_clnt.GetStream(), "exit");
            }
            App.Current.Shutdown();
        }
    }
}