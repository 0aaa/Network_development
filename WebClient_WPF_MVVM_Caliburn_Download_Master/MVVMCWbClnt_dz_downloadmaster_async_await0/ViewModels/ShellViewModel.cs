using MVVMCWbClnt_dz_downloadmaster_async_await0.Infrastructure;
using MVVMCWbClnt_dz_downloadmaster_async_await0.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MVVMCWbClnt_dz_downloadmaster_async_await0.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private ObservableCollection<DownloadUrlPercentageStrctre> _downloadsObsrvbleClctn;
        private WebClient _wbClnt;
        public string UrlToAdd { get; set; }
        public string TitleToAdd { get; set; }
        public ICommand AddDownloadCmd { get; set; }
        public CollectionView DownloadsClctnVw { get; set; }
        private ObservableCollection<DownloadUrlPercentageStrctre> DownLoadsObsrvbleClctn
        {
            get => _downloadsObsrvbleClctn;
            set
            {
                _downloadsObsrvbleClctn = value;
                NotifyOfPropertyChange("DownLoadsObsrvbleClctn");
            }
        }
        public ShellViewModel()
        {
            _wbClnt = new WebClient();
            DownLoadsObsrvbleClctn = new ObservableCollection<DownloadUrlPercentageStrctre>();
            DownloadsClctnVw = (CollectionView)CollectionViewSource.GetDefaultView(DownLoadsObsrvbleClctn);
            AddDownloadCmd = new RelayCommand(actn => AddDownload());
        }
        private void AddDownload()
        {
            DownLoadsObsrvbleClctn.Add(new DownloadUrlPercentageStrctre
            {
                Title = TitleToAdd,
                Url = UrlToAdd,
                Percentage = "0"
            });
            try
            {
                Task.Run(() =>
                {
                    int index = DownLoadsObsrvbleClctn.Count - 1;
                    _wbClnt = new WebClient();
                    _wbClnt.DownloadFileAsync(new Uri(DownLoadsObsrvbleClctn[index].Url), DownLoadsObsrvbleClctn[index].Title);
                    _wbClnt.DownloadProgressChanged += WbClnt_DownloadProgressChanged;
                    _wbClnt.DownloadFileCompleted += WbClnt_DownloadFileCompleted;
                    void WbClnt_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
                    {
                        DownloadUrlPercentageStrctre updateStrctre = DownLoadsObsrvbleClctn[index];
                        updateStrctre.Percentage = $"{e.ProgressPercentage}";
                        App.Current.Dispatcher.BeginInvoke((ThreadStart)delegate
                        {
                            DownLoadsObsrvbleClctn[index] = updateStrctre;
                        });
                    }
                    void WbClnt_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
                    {
                        DownloadUrlPercentageStrctre updateStrctre = DownLoadsObsrvbleClctn[index];
                        updateStrctre.Percentage = "Completed";
                        App.Current.Dispatcher.BeginInvoke((ThreadStart)delegate
                        {
                            DownLoadsObsrvbleClctn[index] = updateStrctre;
                        });
                    }
                });
            }
            catch (Exception excptn)
            {
                DownLoadsObsrvbleClctn.RemoveAt(DownLoadsObsrvbleClctn.Count - 1);
                MessageBox.Show(excptn.Message, "Caramba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}