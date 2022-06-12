using System;
using System.Data;

namespace BIT.Core.Extensions.Util
{
    public delegate void MMProgressType(DataRow dr, Boolean success);

    public enum emailFormat
    {
        text = 0,
        html = 1
    }
}