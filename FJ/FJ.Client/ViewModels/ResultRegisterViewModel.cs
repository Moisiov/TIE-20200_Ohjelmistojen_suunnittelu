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
        public ObservableCollection<ResultRegisterItemModel> Results { get; set; }  // TODO: Bind from model with attribute?

        public ResultRegisterViewModel(ILatestFinlandiaResultsService latestFinlandiaResultsService)
        {
            m_model = new ResultRegisterModel(latestFinlandiaResultsService);

            TestCommand = ReactiveCommand.CreateFromTask(TestCall);
            Results = new ObservableCollection<ResultRegisterItemModel>();
        }

        public async Task TestCall()
        {
            using (Navigator.ShowLoadingScreen())
            {
                var res = await m_model.GetLatestFinlandiaResultsAsync();
                Results = new ObservableCollection<ResultRegisterItemModel>(res);
                RaisePropertyChanged(nameof(Results));
            }
        }

        protected override void DoRefreshInternal()
        {
            Results = new ObservableCollection<ResultRegisterItemModel>();
            RaisePropertyChanged(nameof(Results));
        }
    }
}
