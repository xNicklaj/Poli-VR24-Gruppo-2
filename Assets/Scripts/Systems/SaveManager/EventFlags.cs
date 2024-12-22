using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFlags
{
    public bool TestVar_HasCrossedPlane { get; set; }

    public EventFlags()
    {
        TestVar_HasCrossedPlane = false;
    }

    public EventFlags(EventFlags eventFlags)
    {
        TestVar_HasCrossedPlane = eventFlags.TestVar_HasCrossedPlane;
    }
}
