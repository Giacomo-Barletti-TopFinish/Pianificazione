using ScheduleService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleService.Data.Core;

namespace ScheduleServices.Data
{
    public class ScheduleAdapter : ScheduleAdapterBase
    {
        public ScheduleAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillSchedule_SCHEDULER(ScheduleDS ds)
        {
            string select = @"select * from MONITOR_SCHEDULER WHERE ESEGUITA = 'N'";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MONITOR_SCHEDULER);
            }
        }

        public void UpdateScheduleDSTable(string tablename, ScheduleDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.ContinueUpdateOnError = false;
                DataTable dt = ds.Tables[tablename];
                DbCommandBuilder cmd = BuildCommandBuilder(a);
                a.UpdateCommand = cmd.GetUpdateCommand();
                a.DeleteCommand = cmd.GetDeleteCommand();
                a.InsertCommand = cmd.GetInsertCommand();
                a.Update(dt);
            }
        }
    }
}
