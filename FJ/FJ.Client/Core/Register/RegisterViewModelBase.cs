using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace FJ.Client.Core.Register
{
    public abstract class RegisterViewModelBase<TRegisterModel> : RegisterViewModelBase<TRegisterModel, EmptyNavigationArgs>
        where TRegisterModel : RegisterModelBase
    {
    }
    
    public abstract class RegisterViewModelBase<TRegisterModel, TArgument> : ViewModelBase<TArgument>
        where TRegisterModel : RegisterModelBase
        where TArgument : NavigationArgsBase<TArgument>, new()
    {
        public TRegisterModel RegisterModel { get; set; }
        
        public ReactiveCommand<Unit, Unit> ExecuteSearchCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ClearFiltersCommand { get; private set; }
        public ReactiveCommand<RegisterItemModelBase, Unit> NavigateToCardCommand { get; private set; }

        protected sealed override async Task DoPopulateAsync()
        {
            await OnBeforeActivatingAsync();

            if (RegisterModel == null)
            {
                throw new InvalidOperationException($"{GetType().Name}: Register model must be created on ctor!");
            }

            await RegisterModel.OnActivatingAsync();

            ExecuteSearchCommand = ReactiveCommand.CreateFromTask(ExecuteSearchAsync);
            ClearFiltersCommand = ReactiveCommand.CreateFromTask(DoClearFilters);
            NavigateToCardCommand = ReactiveCommand.CreateFromTask<RegisterItemModelBase>(NavigateToCardAsync);
            
            await OnAfterActivatingAsync();

            await OnDoPopulateAsync();
            
            RaisePropertiesChanged();
        }

        protected virtual Task OnBeforeActivatingAsync()
        {
            return Task.CompletedTask;
        }
        
        protected virtual Task OnAfterActivatingAsync()
        {
            return Task.CompletedTask;
        }
        
        protected virtual Task OnDoPopulateAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task ExecuteSearchAsync()
        {
            using (Navigator.ShowLoadingScreen())
            {
                try
                {
                    await OnBeforeExecuteSearchAsync();
                    await RegisterModel.DoExecuteSearchAsync();
                    await OnAfterExecuteSearchAsync();
                }
                catch (Exception e)
                {
                    RegisterModel.DoClearItems();
                    Navigator.ShowErrorMessage(e.Message);
                }
            }
        }

        protected virtual Task OnBeforeExecuteSearchAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterExecuteSearchAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task NavigateToCardAsync(RegisterItemModelBase item)
        {
            await NavigateToCardInternalAsync(item);
        }

        protected virtual async Task NavigateToCardInternalAsync(RegisterItemModelBase item)
        {
            await Task.CompletedTask;
        }

        protected async Task DoClearFilters()
        {
            RegisterModel.DoClearFilters();
            await Task.CompletedTask;
        }
    }
}
