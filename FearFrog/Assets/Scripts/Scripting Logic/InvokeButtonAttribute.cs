using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EditorInvokeButton
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class InvokeButtonAttribute : Attribute
    {
        public readonly string Label;
        public InvokeButtonAttribute(string label = null) => Label = label;
    }
}
