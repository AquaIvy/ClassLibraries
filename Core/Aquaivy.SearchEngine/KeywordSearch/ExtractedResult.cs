﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeywordSearchEngine
{
    public class ExtractedResult
    {
        /// <summary>
        /// The String value of the result
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The corresponding score compared to the query string
        /// </summary>
        public int Score { get; set; }
    }
}
