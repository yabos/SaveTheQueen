using UnityEngine;
using System;
using System.Collections;

namespace Lib.Event
{
    public interface INotify
    {
        uint MsgCode { get; }
    }
}