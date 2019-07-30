using dotnetCoreWebSample.Dtos;
using dotnetCoreWebSample.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreWebSample.Apis
{
    /// <summary>
    /// 示例控制器
    /// </summary>
    public class SampleController : BaseApiController
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="myDependency"></param>
        public SampleController(IMyDependency myDependency)
        {
            _myDependency = myDependency;
        }
        private readonly IMyDependency _myDependency;

        /// <summary>
        /// Get请求
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var data = _myDependency.Hello();
            return Json(new { data = data });
        }

        /// <summary>
        /// Get请求2  根据Id取信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Json(new { data = $"hello world, Id = {id}" });
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(SampleDto dto)   //Post默认在Body取参数出来,需要在Url或FormData传参,需要指定 [FromQuery]
        {
            return Json(new { data = $"id:{dto.Id}, name :{dto.Name}" });
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(SampleDto dto)   //Put默认在Body取参数出来,需要在Url或FormData传参,需要指定 [FromQuery]
        {
            return Json(new { data = $"id:{dto.Id}, name :{dto.Name}" });
        }


        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)   //默认从url取参数
        {
            return Json(new { data = $"id:{id})" });
        }

        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(SampleDto dto)   //默认从Body取参数
        {
            return Json(new { data = $"id:{dto.Id}, name :{dto.Name}" });
        }








    }
}
