using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planner.Enums
{
    public enum RoleFunctionEnum
    {
        AdminUserMenu = 1,
        AllProjects = 2,
        ApprovedProjects = 4,
        SupportedProjects = 8,
        WaitingToApprove = 16,
        WaitingForSupport = 32,
        UserIndex = 64,
        ApprovedUsers = 128,
        UserCVIndex = 256,
        UserMenu = 512,
        UserProjectIndex = 1024,
        UserRejectedProjectIndex = 2048,
        CreateProject = 4096,
        UserCV = 8192
    }
}