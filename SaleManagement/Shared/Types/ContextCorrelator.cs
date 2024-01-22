using Serilog.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Types
{
    public class ContextCorrelator
    {
        private readonly AsyncLocal<Dictionary<string, object>> _contextItems = new AsyncLocal<Dictionary<string, object>>();
        private Dictionary<string, object> ContextItems
        {
            get => _contextItems.Value ?? (_contextItems.Value = new Dictionary<string, object>());
            set => _contextItems.Value = value;
        }

        public string GetValue(string key)
        {
            if (ContextItems.TryGetValue(key, out var returnValue))
            {
                return returnValue.ToString();
            }
            else
            {
                return "";
            }
        }

        public IDisposable BeginCorrelationScope(string key, object value)
        {
            CorrelationScope scope = new CorrelationScope(ContextItems, LogContext.PushProperty(key, value), this);
            if (ContextItems.ContainsKey(key))
            {
                string existingCorrelationID = ContextItems[key].ToString();
                Log.Warning("{key} is already being correlated with value {existingCorrelationID}. Overwriting with {NewCorrelationid} ", key, existingCorrelationID, value.ToString());
                ContextItems[key] = value;
            }
            else
            {
                ContextItems = new Dictionary<string, object>(ContextItems)
                {
                    { key, value }
                };
            }
            return scope;
        }

        public sealed class CorrelationScope : IDisposable
        {
            private readonly Dictionary<string, object> bookmark;
            private readonly IDisposable logContextPop;
            private readonly ContextCorrelator contextCorrelator;

            public CorrelationScope(Dictionary<string, object> bookmark, IDisposable logContextPop, ContextCorrelator contextCorrelator)
            {
                this.bookmark = bookmark ?? throw new ArgumentNullException(nameof(bookmark));
                this.logContextPop = logContextPop ?? throw new ArgumentNullException(nameof(logContextPop));
                this.contextCorrelator = contextCorrelator;
            }

            public void Dispose()
            {
                logContextPop.Dispose();
                contextCorrelator.ContextItems = bookmark;
            }
        }
    }
}
