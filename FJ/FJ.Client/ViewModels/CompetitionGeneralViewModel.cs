using System;
using FJ.Client.Models;

namespace FJ.Client.ViewModels
{
    public class CompetitionGeneralViewModel : ViewModelBase
    {
        private CompetitionGeneralModel m_model;

        public CompetitionGeneralViewModel()
        {
            m_model = new CompetitionGeneralModel();
        }
    }
}
