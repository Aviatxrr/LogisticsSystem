using System;
using System.Data.Entity.Core.Common.CommandTrees;
using DispatchRecordSystem.Models;
using LogisticsUX.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LogisticsUX;

public class ViewModelFactory
{

    
    public dynamic GetViewModel(IEntity entity)
    {
        
        switch (entity)
        {
            case DispatchRecord:
                var vm = new DispatchRecordViewModel();
                vm.Entity = entity;
                return vm;
        }

        return new ViewModelBase();
    }
}