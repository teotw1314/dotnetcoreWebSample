using dotnetCoreWebSample.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreWebSample.Impl
{
    /// <summary>
    /// 实现示例
    /// </summary>
    public class MyDependency : IMyDependency
    {
        public string Hello()
        {
            return "Hello Dependency"; 
        }

    }
}
