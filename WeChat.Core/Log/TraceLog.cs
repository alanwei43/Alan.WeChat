using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChat.Core.Log
{
    public class TraceLog : ILog
    {
        public void Write(string id, DateTime date, string category, string request, string response, string note)
        {
            Trace.WriteLine("Id: " + id);
            Trace.WriteLine("Date: " + date.ToString("yyyy-MM-dd HH-mm-ss"));
            Trace.WriteLine("Category: " + category);
            Trace.WriteLine("Request: " + request);
            Trace.WriteLine("Response: " + response);
            Trace.WriteLine("Note: " + note);
            Trace.WriteLine(String.Join("", Enumerable.Repeat("-", 50)));
        }

    }
}
