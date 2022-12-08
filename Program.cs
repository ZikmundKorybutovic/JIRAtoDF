using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace JIRAtoDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var jiraConnector = new JiraConnector();

            dynamic requestData;
            dynamic issueData;
            var startAt = 0;
            var totalRequests = 0;

            var counter = 0;

            var tbl = new DataTable();
            tbl.Columns.Add("JIRAId", typeof(string));
            tbl.Columns.Add("TAId", typeof(int));
            tbl.Columns.Add("Status", typeof(string));
            tbl.Columns.Add("Title", typeof(string));

            do
            {
                requestData = jiraConnector.GetDataFromRequest(@"search?jql=project%20%3D%20""RDTS""&maxResults=100&startAt=" + startAt);

                if (requestData != null)
                {
                    foreach (var issue in (IEnumerable<dynamic>)requestData.issues)
                    {
                        counter++;

                        if (Utils.IsStatusRelevant(issue.fields.status.name.ToString()))
                        {
                            Logger.DebugLog($"{issue.key}: {issue.fields.status.name}");

                            issueData = jiraConnector.GetDataFromRequest(@"issue/" + issue.key);
                            if (!string.IsNullOrEmpty(issueData.fields.customfield_10730.ToString()))
                            {
                                var dr = tbl.NewRow();
                                dr["JIRAId"] = issueData.key.ToString();
                                Logger.DebugLog($"jiraid: {issueData.key}");
                                int.TryParse(issueData.fields.customfield_10730.ToString(), out int ta);
                                dr["TAId"] = ta;
                                Logger.DebugLog($"taid: {ta}");
                                dr["Status"] = issue.fields.status.name.ToString();
                                Logger.DebugLog($"status: {issue.fields.status.name}");
                                dr["Title"] = issueData.fields.summary.ToString();
                                Logger.DebugLog($"title: {issueData.fields.summary}");

                                tbl.Rows.Add(dr);

                            }
                        }
                    }

                    if (startAt == 0)
                    {
                        int.TryParse(requestData.total.ToString(), out totalRequests);
                    }
                }
                
                startAt += 100;                
            }
            while (startAt < totalRequests);

            var sqlConnector = new SQLConnector();
            bool success = sqlConnector.BulkInsertDT(tbl, true);

            Logger.DebugLog(success);
            Logger.DebugLog($"Total counter: {counter}");
        }
    }
}
