﻿using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Neolog
{
    public sealed class AppSettings
    {
        private static IsolatedStorageSettings isolatedStore;
        private static volatile AppSettings instance;
        private static object syncRoot = new Object();

        #region Constructor
        private AppSettings()
        {
            try
            {
                AppSettings.isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            }
            catch (Exception e)
            {
                AppSettings.LogThis("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AppSettings();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Variables
        public static string AppName = "Neolog";

        public static string DBConnectionString = "Data Source=isostore:/Neolog.sdf";

        public static string ServicesURL = "http://www.neolog.bg/service.php";
        public static bool InDebug = true;

        public static string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        //public static string BackgroundColor = "#FF29435E";
        public static Color BackgroundColor = Color.FromArgb(255, 41, 67, 94);

        public static string[] CyrillicLetters = new string[] {"А", "Б", "В", "Г", "Д", "Е", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ю", "Я"};

        public static string FacebookAppID = "";
        public static string FacebookAppSecret = "";

        public static string TwitterRequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public static string TwitterAuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static string TwitterAccessTokenUri = "https://api.twitter.com/oauth/access_token";
        public static string TwitterCallbackUri = "oob";
        public static string TwitterStatusUpdateUrl { get { return "http://api.twitter.com"; } }
        public static string TwitterConsumerKey = "";
        public static string TwitterConsumerKeySecret = "";
        public static string TwitterOAuthVersion = "1.0a";
        #endregion

        #region Helpers
        public static void LogThis(params string[] logs)
        {
            if (AppSettings.InDebug)
            {
                Debugger.Log(3, "Warning", "[____Neolog-Log] " + string.Join(" ", logs));
                Debug.WriteLine("[____Neolog-Log] " + string.Join(" ", logs));
            }
        }

        public static string DoLongDate(DateTime dt)
        {
            if (dt == null)
                return "";
            string date = "";
            date += dt.ToString("HH:mm");
            date += " ";
            date += AppResources.ResourceManager.GetString("weekday_" + ((int)dt.DayOfWeek + 1));
            date += ", ";
            date += dt.Day;
            date += " ";
            date += AppResources.ResourceManager.GetString("monthFull_" + dt.Month);
            return date;
        }

        public static string DoShortDate(DateTime dt)
        {
            if (dt == null)
                return "";
            string date = "";
            date += dt.ToString("HH:mm");
            date += " ";
            date += AppResources.ResourceManager.GetString("weekday_" + ((int)dt.DayOfWeek + 1));
            date += ", ";
            date += dt.Day;
            date += " ";
            date += AppResources.ResourceManager.GetString("monthShort_" + dt.Month);
            return date;
        }

        public static string Hyperlinkify(string strvar)
        {
            string final = strvar;

            Regex regex = new Regex(@"<nolink>(.*?)</nolink>",
                          RegexOptions.IgnoreCase | RegexOptions.Singleline |
                          RegexOptions.CultureInvariant |
                          RegexOptions.IgnorePatternWhitespace |
                          RegexOptions.Compiled);

            MatchCollection theMatches = regex.Matches(strvar);

            for (int index = 0; index < theMatches.Count; index++)
                final = final.Replace(theMatches[index].ToString(), theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));

            regex = new Regex(@"<a(.*?)</a>", RegexOptions.IgnoreCase |
                    RegexOptions.Singleline | RegexOptions.CultureInvariant |
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            theMatches = regex.Matches(final);

            for (int index = 0; index < theMatches.Count; index++)
            {
                final = final.Replace(theMatches[index].ToString(),
                        theMatches[index].ToString().Replace(".", "[[[pk:period]]]"));
            }

            final = Regex.Replace(final, @"(?<=\d)\.(?=\d)", "[[[pk:period]]]");

            Regex tags = new Regex(@"([a-zA-Z0-9\:/\-]*[a-zA-Z0-9\-_]\" +
                         @".[a-zA-Z0-9\-_][a-zA-Z0-9\-_][a-zA-Z0-9\?\" +
                         @"=&#_\-/\.]*[^<>,;\.\s\)\(\]\[\""])");

            final = tags.Replace(final, "<a href=\"http://$&\">$&</a>");
            final = final.Replace("http://https://", "https://");
            final = final.Replace("http://http://", "http://");

            final = final.Replace("[[[pk:period]]]", ".");
            final = final.Replace("<nolink>", "");
            final = final.Replace("</nolink>", "");

            return final;
        }

        public static bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }
        #endregion

        #region Settings
        const string ConfNameWordSync = "ConfNameWordSync";
        const string ConfNamePrivateData = "ConfNamePrivateData";
        const string ConfNamePDEmail = "ConfNamePDEmail";
        const string ConfNamePDName = "ConfNamePDName";
        const string ConfNamePDURL = "ConfNamePDURL";

        const bool ConfDefaultWordSync = true;
        const bool ConfDefaultPrivateData = true;
        const string ConfDefaultPDEmail = "";
        const string ConfDefaultPDName = "";
        const string ConfDefaultPDURL = "";

        public static bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;
            if (isolatedStore.Contains(Key))
            {
                if (AppSettings.isolatedStore[Key] != value)
                {
                    AppSettings.isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                AppSettings.isolatedStore.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        public static valueType GetValueOrDefault<valueType>(string Key, valueType defaultValue)
        {
            valueType value;
            if (AppSettings.isolatedStore.Contains(Key))
                value = (valueType)AppSettings.isolatedStore[Key];
            else
                value = defaultValue;
            return value;
        }

        public static void Save()
        {
            isolatedStore.Save();
        }

        public static bool ConfWordSync
        {
            get
            {
                return GetValueOrDefault<bool>(ConfNameWordSync, ConfDefaultWordSync);
            }
            set
            {
                AddOrUpdateValue(ConfNameWordSync, value);
                Save();
            }
        }

        public static bool ConfPrivateData
        {
            get
            {
                return GetValueOrDefault<bool>(ConfNamePrivateData, ConfDefaultPrivateData);
            }
            set
            {
                AddOrUpdateValue(ConfNamePrivateData, value);
                Save();
            }
        }

        public static string ConfPDEmail
        {
            get
            {
                return GetValueOrDefault<string>(ConfNamePDEmail, ConfDefaultPDEmail);
            }
            set
            {
                AddOrUpdateValue(ConfNamePDEmail, value);
                Save();
            }
        }

        public static string ConfPDName
        {
            get
            {
                return GetValueOrDefault<string>(ConfNamePDName, ConfDefaultPDName);
            }
            set
            {
                AddOrUpdateValue(ConfNamePDName, value);
                Save();
            }
        }

        public static string ConfPDURL
        {
            get
            {
                return GetValueOrDefault<string>(ConfNamePDURL, ConfDefaultPDURL);
            }
            set
            {
                AddOrUpdateValue(ConfNamePDURL, value);
                Save();
            }
        }
        #endregion
    }
}
