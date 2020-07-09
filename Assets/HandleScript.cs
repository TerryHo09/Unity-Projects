using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Lecture1))]
public class HandleScript : Editor
{
    private void OnSceneGUI()
    {
        Lecture1 Obj = target as Lecture1;
        if ( Obj.m_TranglePoint == null || Obj.m_TranglePoint.Length != 3 )
            return;

        Handles.DrawLine( Obj.m_TranglePoint[0], Obj.m_TranglePoint[1] );
        Handles.DrawLine( Obj.m_TranglePoint[1], Obj.m_TranglePoint[2] );
        Handles.DrawLine( Obj.m_TranglePoint[2], Obj.m_TranglePoint[0] );
    }

}
