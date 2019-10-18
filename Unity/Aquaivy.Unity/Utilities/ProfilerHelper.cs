﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Profiling;

namespace Aquaivy.Unity.Utilities
{
    /// <summary>
    /// Unity Profiler调试工具，可加入自定义监测项目
    /// </summary>
    public class ProfilerHelper
    {
        public static bool EnableProfilerSample = true;

        /// <summary>
        /// 是否允许BeginSample的代码段名字使用格式化字符串（格式化字符串本身会带来内存开销）
        /// </summary>
        public static bool EnableFormatStringOutput = true;

        public static void BeginSample(string name)
        {
#if ENABLE_PROFILER
            if (EnableProfilerSample)
            {
                Profiler.BeginSample(name);
            }
#endif
        }

        public static void BeginSample(string formatName, params object[] args)
        {
#if ENABLE_PROFILER
            if (EnableProfilerSample)
            {
                // 必要时很有用，但string.Format本身会产生GC Alloc，需要慎用
                if (EnableFormatStringOutput)
                    Profiler.BeginSample(string.Format(formatName, args));
                else
                    Profiler.BeginSample(formatName);
            }
#endif
        }

        public static void EndSample()
        {
#if ENABLE_PROFILER
            if (EnableProfilerSample)
            {
                Profiler.EndSample();
            }
#endif
        }
    }
}
