using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngineInternal;


/// <summary>
/// 빌드 시 빈 함수로 대체되어 로그를 출력하지 않습니다.
/// </summary>
public static class Debug 
{
    public static bool isDebugBuild
    {
	get { return UnityEngine.Debug.isDebugBuild; }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log (object message)
    {   
        UnityEngine.Debug.Log (message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log (object message, UnityEngine.Object context)
    {   
        UnityEngine.Debug.Log (message, context);
    }
		
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError (object message)
    {   
        UnityEngine.Debug.LogError (message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]	
    public static void LogError (object message, UnityEngine.Object context)
    {   
        UnityEngine.Debug.LogError (message, context);
    }
 
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning (object message)
    {   
        UnityEngine.Debug.LogWarning (message.ToString ());
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")] 
    public static void LogWarning (object message, UnityEngine.Object context)
    {   
        UnityEngine.Debug.LogWarning (message.ToString (), context);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")] 
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
 	UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    } 
	
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
	UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }
 	
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
	if (!condition) throw new Exception();
    }
}
