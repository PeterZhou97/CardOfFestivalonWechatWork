﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BirthdayCard
{
    // <summary>
    /// 辅助类，用于保存IP索引信息
    /// </summary>
    ///
    public class CZ_INDEX_INFO
    {
        public UInt32 IpSet;
        public UInt32 IpEnd;
        public UInt32 Offset;

        public CZ_INDEX_INFO()
        {
            IpSet = 0;
            IpEnd = 0;
            Offset = 0;
        }
    }

}