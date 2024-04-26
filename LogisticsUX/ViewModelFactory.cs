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
            case DispatchRecord record:
                var vm = new DispatchRecordViewModel
                {
                    Entity = record
                };
                return vm;
        }

        return new ViewModelBase();
    }
}