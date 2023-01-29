using MVVMCUdpClnt_dz_udp0.Infrastructure;
using MVVMCUdpClnt_dz_udp0.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVMCUdpClnt_dz_udp0.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private const string IP_ADDRESS = "127.0.0.1";
        private readonly static IPAddress _ip = IPAddress.Parse(IP_ADDRESS);
        public int LocalPort { get; set; }
        public int RemotePort { get; set; }
        public string Pseudo { get; set; }

        private UdpClient _receivingUdpClnt;
        private UdpClient _sendingUdpClnt;
        private IPEndPoint _receivingIpEndPnt;
        private IPEndPoint _sendingIpEndPnt;

        private StreamWriter _messagesRecorderStrmWrtr;
        private byte[] _receivingBtsArr;
        private byte[] _sendingBtsArr;
        private string _receivingLastMessageStr;
        private string _sendingLastMessageStr;
        public string MessageToSendStr { get; set; }

        private ObservableCollection<string> ChatMessagesObsrvbleClctn { get; set; }
        public CollectionView ChatMessagesClctnVw { get; set; }

        public ICommand ConnectReconnectCmd { get; set; }
        public RelayCommand SendMessageCmd { get; set; }
        public bool SameIPEndPoint { get; set; }

        private string _currentBackground;
        private string _currentForeground;
        private int _currentFontSize;
        private string _currentFontStyle;
        private string _currentFontWeight;

        public string[] ColorsArr { get; set; }
        public string[] FontStylesArr { get; set; }
        public string[] FontWeightsArr { get; set; }
        public string CurrentBackground
        {
            get => _currentBackground;
            set
            {
                _currentBackground = value;
                NotifyOfPropertyChange("CurrentBackground");
            }
        }
        public string CurrentForeground
        {
            get => _currentForeground;
            set
            {
                _currentForeground = value;
                NotifyOfPropertyChange("CurrentForeground");
            }
        }
        public int CurrentFontSize
        {
            get => _currentFontSize;
            set
            {
                _currentFontSize = value;
                NotifyOfPropertyChange("CurrentFontSize");
            }
        }
        public string CurrentFontStyle
        {
            get => _currentFontStyle;
            set
            {
                _currentFontStyle = value;
                NotifyOfPropertyChange("CurrentFontStyle");
            }
        }
        public string CurrentFontWeight
        {
            get => _currentFontWeight;
            set
            {
                _currentFontWeight = value;
                NotifyOfPropertyChange("CurrentFontWeight");
            }
        }

        public ShellViewModel()
        {
            ChatMessagesObsrvbleClctn = new ObservableCollection<string>();
            ChatMessagesClctnVw = (CollectionView)CollectionViewSource.GetDefaultView(ChatMessagesObsrvbleClctn);

            ConnectReconnectCmd = new RelayCommand(actn => ConnectionReconnection());
            SendMessageCmd = new RelayCommand(actn => MessageOut()) { CanExecuteFlag = false };

            LocalPort = 25565;
            RemotePort = 23145;
            Pseudo = "user0";

            ColorsArr = new string[] { "Black", "White", "Red" };
            FontStylesArr = new string[] { "Italic", "Normal", "Oblique" };
            FontWeightsArr = new string[] { "Black", "Bold", "Demi-bold", "Extra-black", "Extra-bold", "Extra-light", "Heavy", "Light",
                "Medium", "Normal", "Regular", "Semi-bold", "Thin", "Ultra-black", "Ultra-bold", "UltraLight"};
            CurrentBackground = "White";
            CurrentForeground = "Black";
            CurrentFontSize = 12;
            CurrentFontStyle = "Normal";
            CurrentFontWeight = "Normal";
        }
        // Вызов в параллельном потоке в методе ConnectionReconnection() .
        private void MessageIn(int localPort)
        {
            // Механизм прерывания цикла несёт в себе изъян ибо переменная-флаг способна быть изменена до достижения потоками-предшественниками условия-проверки.
            SameIPEndPoint = true;
            while (SameIPEndPoint)
            {
                _receivingUdpClnt = new UdpClient(localPort);
                _receivingIpEndPnt = null;

                _receivingBtsArr = _receivingUdpClnt.Receive(ref _receivingIpEndPnt);
                _receivingUdpClnt.Close();

                _receivingLastMessageStr = $"{DateTime.Now}" + ' ' + Encoding.Default.GetString(_receivingBtsArr);

                App.Current.Dispatcher.BeginInvoke((ThreadStart)delegate
                {
                    ChatMessagesObsrvbleClctn.Add(_receivingLastMessageStr);
                });
                MessagesRecording(_receivingLastMessageStr);
            }
        }
        private void MessageOut()
        {
            _sendingUdpClnt = new UdpClient();

            _sendingLastMessageStr = Pseudo + ": " + MessageToSendStr;
            _sendingBtsArr = Encoding.Default.GetBytes(_sendingLastMessageStr);

            _sendingUdpClnt.Send(_sendingBtsArr, _sendingBtsArr.Length, _sendingIpEndPnt);
            _sendingUdpClnt.Close();

            _sendingLastMessageStr = $"{DateTime.Now}" + ' ' + _sendingLastMessageStr;

            ChatMessagesObsrvbleClctn.Add(_sendingLastMessageStr);
            MessagesRecording(_sendingLastMessageStr);
        }
        private void MessagesRecording(string lastMessage)
        {
            Task.Run(() =>
            {
                try
                {
                    using (_messagesRecorderStrmWrtr = new StreamWriter($"messages from {RemotePort} records.txt", true))
                    {
                        _messagesRecorderStrmWrtr.WriteLine(lastMessage);
                    }
                }
                catch (Exception excptn)
                {
                    MessageBox.Show(excptn.Message, "Caramba!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }
        private async void ConnectionReconnection()
        {
            try
            {
                SameIPEndPoint = false;
                // Применение await с целью перехода к нижеследующим инструкциям после инициализации объекта IPEndPoint .
                await Task.Run(() => _sendingIpEndPnt = new IPEndPoint(_ip, RemotePort));
                Task.Run(() => MessageIn(LocalPort));
            }
            catch (Exception excptn)
            {
                MessageBox.Show(excptn.Message, "Caramba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            SendMessageCmd.CanExecuteFlag = true;
        }
    }
}