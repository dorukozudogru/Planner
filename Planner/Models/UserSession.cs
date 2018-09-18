using Planner.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Models
{
    public class UserSession
    {
        public static bool IsLoggedIn
        {
            get
            {
                return SessionWrapper.GetSessionItem<bool>("IsLoggedIn", false);
            }
            set
            {
                SessionWrapper.SetSessionItem<bool>("IsLoggedIn", value);
            }
        }

        public static string UserId
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserId");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserId", value);
            }
        }
        public static string UserEMail
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserEMail");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserEMail", value);
            }
        }

        public static string UserPassword
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserPassword");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserPassword", value);
            }
        }

        public static string UserName
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserName");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserName", value);
            }
        }
        public static string UserSurname
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserSurname");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserSurname", value);
            }
        }
        public static string UserCitizenshipNo
        {
            get
            {
                return SessionWrapper.GetSessionItem<string>("UserCitizenshipNo");
            }
            set
            {
                SessionWrapper.SetSessionItem<string>("UserCitizenshipNo", value);
            }
        }
        public static bool UserIsCvUploaded
        {
            get
            {
                return SessionWrapper.GetSessionItem<bool>("UserIsCvUploaded");
            }
            set
            {
                SessionWrapper.SetSessionItem<bool>("UserIsCvUploaded", value);
            }
        }

        public static int UserCvId
        {
            get
            {
                return SessionWrapper.GetSessionItem<int>("UserCvId");
            }
            set
            {
                SessionWrapper.SetSessionItem<int>("UserCvId", value);
            }
        }

        public static int UserIsApproved
        {
            get
            {
                return SessionWrapper.GetSessionItem<int>("UserIsApproved");
            }
            set
            {
                SessionWrapper.SetSessionItem<int>("UserIsApproved", value);
            }
        }

        public static int UserIsAdmin
        {
            get
            {
                return SessionWrapper.GetSessionItem<int>("UserIsAdmin");
            }
            set
            {
                SessionWrapper.SetSessionItem<int>("UserIsAdmin", value);
            }
        }

        public static bool IsAuthorized()
        {
            if (!UserSession.IsLoggedIn)
                return false;

            if (UserSession.IsLoggedIn)
                return true;

            return false;

            //if (roles != 0 && !roles.HasFlag(UserSession.Role))
            //    return false;
            //else
            //    return true;
        }
    }
}