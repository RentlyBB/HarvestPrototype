using System;
using UnityEngine;

namespace _Scripts.CustomTools {
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class RequireVariable : PropertyAttribute { }
}