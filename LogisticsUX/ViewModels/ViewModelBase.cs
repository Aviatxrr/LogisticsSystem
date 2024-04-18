using System;
using System.ComponentModel;
using System.Reflection;
using Castle.DynamicProxy;
using DispatchRecordSystem;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class ViewModelBase : ReactiveObject
{
    
    public virtual ViewModelBase GetViewModelForEntity(IEntity entity)
    {
        string viewModelName = $"LogisticsUX.ViewModels.{ProxyUtil.GetUnproxiedType(entity).Name}ViewModel";
        string viewModelAssembly = $"{Assembly.GetAssembly(GetType())}";
        Type viewModelType = Type.GetType($"{viewModelName}, {viewModelAssembly}")!;
        return Activator.CreateInstance(viewModelType) as ViewModelBase;
    }
    
}