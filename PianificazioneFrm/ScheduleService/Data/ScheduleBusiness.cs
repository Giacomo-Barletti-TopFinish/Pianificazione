using ScheduleService;
using ScheduleService.Data.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleServices.Data
{
    public class ScheduleBusiness : ScheduleBusinessBase
    {
        public ScheduleBusiness() : base() { }

        [DataContext]
        public void FillSchedule_SCHEDULER(ScheduleDS ds)
        {
            ScheduleAdapter a = new ScheduleAdapter(DbConnection, DbTransaction);
            a.FillSchedule_SCHEDULER(ds);
        }

        [DataContext(true)]
        public void UpdateSchedule_SCHEDULER(ScheduleDS ds)
        {
            ScheduleAdapter a = new ScheduleAdapter(DbConnection, DbTransaction);
            a.UpdateScheduleDSTable(ds.MONITOR_SCHEDULER.TableName, ds);
        }
    }
}
