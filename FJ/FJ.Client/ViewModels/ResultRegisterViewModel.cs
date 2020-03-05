using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using FJ.Client.Models;
using FJ.ServiceInterfaces.FinlandiaHiihto;
using ReactiveUI;

namespace FJ.Client.ViewModels
{
    public class ResultRegisterViewModel : ViewModelBase
    {
        private ResultRegisterModel m_model;

        public ReactiveCommand<Unit, Unit> TestCommand { get; }
        public ObservableCollection<string> Results { get; set; }

        public ResultRegisterViewModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            m_model = new ResultRegisterModel(latestFinlandiaResultsService);

            TestCommand = ReactiveCommand.CreateFromTask(TestCall);
            Results = new ObservableCollection<string>();
        }

        public async Task TestCall()
        {
            var res = await OnShowLoadingScreen(m_model.GetLatestFinlandiaResultsAsSortedStringsAsync);
            Results = new ObservableCollection<string>(res);
            RaisePropertyChanged(nameof(Results));
        }

        protected override void DoRefreshInternal()
        {
            Results = new ObservableCollection<string>();
            RaisePropertyChanged(nameof(Results));
        }
    }
}
