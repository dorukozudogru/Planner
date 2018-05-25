using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Enums
{
    public enum UserApproveEnum
    {
        NotApproved = 0, /* Kullanılmıyor */
        WaitingToApprove = 1,
        Approved = 2,
        Blocked = 3,
        ApproveAfterBlock = 4
    }
}