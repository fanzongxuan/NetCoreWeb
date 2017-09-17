﻿using Microsoft.AspNetCore.Mvc;
using DotNetCore.Framework.Mvc.Controllers;
using DotNetCore.Framework.Controllers.Attributes;

namespace DotNetCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class BaseAdminController : BaseController
    {
    }
}