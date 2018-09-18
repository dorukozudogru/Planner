using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Enums
{
    public enum ProjectTypeChangeEnum
    {
        NotChanged = 0,
        ChangeAsApproved = 1,
        ChangeAsNotApproved = 2,
        ChangeAsSupported = 3,
        ChangeAsNotSupported = 4
    }
}