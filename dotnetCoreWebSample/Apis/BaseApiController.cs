using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreWebSample.Apis
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {


    }
}
