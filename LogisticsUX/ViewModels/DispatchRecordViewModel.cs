using System;
using DispatchRecordSystem;
using DispatchRecordSystem.Models;
using DispatchRecordSystem.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace LogisticsUX.ViewModels;

public class DispatchRecordViewModel : EntityInfoViewModel<DispatchRecord>
{
   public int TruckId
   {
      //if record has a truck, get it, else 0
      get => Entity.TruckId ?? 0;
      set
      {
         int prevId = TruckId;
         try
         {
            //call service layer
            _serviceProvider
               .GetRequiredService<DispatchRecordService>()
               .EnqueueDispatch(_serviceProvider
                  .GetRequiredService<Repository<Truck>>()
                  .GetById(value), Entity);
            
            //notify property change
            this.RaisePropertyChanged();
         }
         catch (ApplicationException ex)
         {
            //if truck doesnt exist, display notification, and reset textbox to original ID
            TruckId = prevId;
            _serviceProvider.GetRequiredService<UserMessage>()
               .Contents = ex.Message;
         }
      }
   }
}