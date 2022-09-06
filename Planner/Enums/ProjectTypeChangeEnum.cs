using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Enums
{
    public enum ProjectTypeChangeEnum
    {
        NotChanged = 0,
        ChangedAsApproved = 1,
        ChangedAsNotApproved = 2,
        ChangedAsSupported = 3,
        ChangedAsNotSupported = 4,
        ChangedAsClosed = 5
    }
}