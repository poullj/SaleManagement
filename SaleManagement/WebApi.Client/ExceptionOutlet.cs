using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Client
{
    public delegate void ExceptionOutletDelegate(bool expectedException, ApiException apiException);
    public static class ExceptionOutlet
    {
        public static event ExceptionOutletDelegate ExceptionOutletEvent;
        public static void PreThrow(bool expectedException, ApiException apiException)
        {
            if (ExceptionOutletEvent != null)
                ExceptionOutletEvent(expectedException, apiException);
        }
    }
}
