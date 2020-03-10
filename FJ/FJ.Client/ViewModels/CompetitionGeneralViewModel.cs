using System;
using FJ.Client.Models;
using FJ.Client.UICore;

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
