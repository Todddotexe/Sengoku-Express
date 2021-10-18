using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class MonoScriptAttribute : PropertyAttribute {
    public System.Type type;
}

[AttributeUsage(AttributeTargets.Method)]
public class AI_Function_Attribute : Attribute {
    public AI_Function_Attribute() {}
}