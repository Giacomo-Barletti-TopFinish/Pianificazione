using Pianificazione.Data.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priorita.Data
{
    public class PrioritaAdapterBase : AdapterBase
    {
        public PrioritaAdapterBase(IDbConnection connection, IDbTransaction transaction) :
          base(connection, transaction)
        {
        }

        protected void AddConditionAndParam(ref string query, string fieldName, string parameterName, string parameterValue, ParamSet ps, bool useLike)
        {
            if (string.IsNullOrEmpty(parameterValue)) return;

            if (parameterValue == "%" || (parameterValue.Length == parameterValue.Count(c => c == '%')))
            {
                query += string.Format(CultureInfo.InvariantCulture, " AND NULLIF({0},'') IS NOT NULL ", fieldName);
                return;
            }

            if (!string.IsNullOrEmpty(parameterValue))
            {
                if (useLike)
                {
                    query += string.Format(CultureInfo.InvariantCulture, " AND {0} LIKE $P<{1}> ", fieldName, parameterName);
                    ps.AddParam(parameterName, DbType.String, parameterValue + "%");
                }
                else
                {
                    query += string.Format(CultureInfo.InvariantCulture, " AND {0} = $P<{1}> ", fieldName, parameterName);
                    ps.AddParam(parameterName, DbType.String, parameterValue);
                }
            }
        }

        protected void AddConditionAndParam(ref string query, string[] fieldName, string parameterName, string parameterValue, ParamSet ps, bool useLike)
        {
            string command = string.Empty;
            if (!string.IsNullOrEmpty(parameterValue))
            {
                command = " AND (";

                for (int i = 0; i < fieldName.Length; i++)
                {
                    if (i > 0)
                        command += " OR ";

                    string param = parameterName + i.ToString();
                    if (useLike)
                    {
                        command += string.Format(CultureInfo.InvariantCulture, " {0} LIKE $P<{1}> ", fieldName[i], param);
                        ps.AddParam(param, DbType.String, parameterValue + "%");
                    }
                    else
                    {
                        command += string.Format(CultureInfo.InvariantCulture, " {0} = $P<{1}> ", fieldName[i], param);
                        ps.AddParam(param, DbType.String, parameterValue);
                    }
                }


                command += ")";
            }
            query += command;
        }

        protected void AddConditionAndParam(ref string query, string fieldName, string parameterName, int? parameterValue, ParamSet ps)
        {
            if (parameterValue.HasValue)
            {
                query += string.Format(CultureInfo.InvariantCulture, " AND {0} = $P<{1}> ", fieldName, parameterName);
                ps.AddParam(parameterName, DbType.Int32, parameterValue.Value);
            }
        }

        protected void AddConditionAndParam(ref string query, string fieldName, string parameterName, decimal? parameterValue, ParamSet ps)
        {
            if (parameterValue.HasValue)
            {
                query += string.Format(CultureInfo.InvariantCulture, " AND {0} = $P<{1}> ", fieldName, parameterName);
                ps.AddParam(parameterName, DbType.Decimal, parameterValue.Value);
            }
        }

        protected void AddConditionAndParam(ref string query, string fieldName, string parameterName, long? parameterValue, ParamSet ps)
        {
            if (parameterValue.HasValue)
            {
                query += string.Format(CultureInfo.InvariantCulture, " AND {0} = $P<{1}> ", fieldName, parameterName);
                ps.AddParam(parameterName, DbType.Int64, parameterValue.Value);
            }
        }

        protected string ConvertToStringForInCondition(List<long> idElement)
        {
            List<long> items = idElement.Distinct().ToList();
            string result = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                result += items[i].ToString();
                if (i != items.Count - 1)
                    result += ",";
            }
            return result;
        }

        protected string ConvertToStringForInCondition(List<decimal> idElement)
        {
            List<decimal> items = idElement.Distinct().ToList();
            string result = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                result += items[i].ToString();
                if (i != items.Count - 1)
                    result += ",";
            }
            return result;
        }

        protected string ConvertToStringForInCondition(List<long?> idElement)
        {
            List<long?> items = idElement.Distinct().ToList();
            string result = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].HasValue)
                {
                    if (i > 0 && !string.IsNullOrEmpty(result))
                        result += string.Format(CultureInfo.InvariantCulture, ",{0}", items[i].ToString());
                    else
                        result += items[i].ToString();
                }
            }
            return result;
        }

        protected string ConvertToStringForInCondition(List<int> idElement)
        {
            List<int> items = idElement.Distinct().ToList();
            string result = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                result += items[i].ToString();
                if (i != items.Count - 1)
                    result += ",";
            }
            return result;
        }

        protected string ConvertToStringForInCondition(List<string> idElement)
        {
            List<string> items = idElement.Distinct().ToList();
            string result = string.Empty;
            for (int i = 0; i < items.Count; i++)
            {
                if (string.IsNullOrEmpty(items[i]))
                    continue;

                result += string.Format(CultureInfo.InvariantCulture, "'{0}'", items[i]);
                if (i != items.Count - 1)
                    result += ",";
            }
            return result;
        }

        public long GetID()
        {
            string select = @" SELECT CDC_SEQUENCE.NEXTVAL FROM DUAL";
            using (IDbCommand da = BuildCommand(select))
            {
                long lnNextVal = Convert.ToInt64(da.ExecuteScalar());
                return lnNextVal;
            }
        }
    }
}
