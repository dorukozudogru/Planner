using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Helpers
{
    public class SessionWrapper
    {
        public static T GetSessionItem<T>(string key)
        {
            object obj = HttpContext.Current.Session[key];

            if (obj == null)
                return default(T);


            return (T)obj;
        }

        public static T GetSessionItem<T>(string key, T defaultValue)
        {
            object obj = HttpContext.Current.Session[key];

            if (obj == null)
            {
                HttpContext.Current.Session[key] = defaultValue;
                return defaultValue;
            }
            return (T)obj;
        }

        public static void SetSessionItem<T>(string key, T value)
        {
            if (value == null)
                HttpContext.Current.Session.Remove(key);
            else
                HttpContext.Current.Session[key] = value;
        }

        public static void Abandon()
        {
            HttpContext.Current.Session.RemoveAll();
            HttpContext.Current.Session.Abandon();
        }
    }
}