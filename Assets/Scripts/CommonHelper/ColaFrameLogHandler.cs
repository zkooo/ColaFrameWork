using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ColaFrameLogHandler : ILogHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LogFormat(LogType logType, Object context, string format, params object[] args)
    {
        Debug.LogError(context.ToString());
    }

    public void LogException(Exception exception, Object context)
    {
        throw new NotImplementedException();
    }
}
