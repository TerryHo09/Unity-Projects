using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UI;

public class Lecture1 : MonoBehaviour
{
    public Vector3[] m_TranglePoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        Handles.DrawLine( this.m_TranglePoint[0], this.m_TranglePoint[1] );
        Handles.DrawLine( this.m_TranglePoint[1], this.m_TranglePoint[2] );
        Handles.DrawLine( this.m_TranglePoint[2], this.m_TranglePoint[0] );

        Vector3 MousePos = Input.mousePosition;
        MousePos.y = 720f - MousePos.y;

        if ( this.IsPointInTrangle(MousePos, this.m_TranglePoint[0], this.m_TranglePoint[1], this.m_TranglePoint[2] ) )
        {
            Handles.color = Color.green;
        }
        else
            Handles.color = Color.white;

    }

    bool IsPointInTrangle( Vector3 CurPos, Vector3 P1, Vector3 P2, Vector3 P3 )
    {
        float iHat = P1.x * (P3.y - P1.y) + (CurPos.y - P1.y) * (P3.x - P1.x) - CurPos.x * (P3.y - P1.y);
        iHat /= ((P2.y - P1.y) * (P3.x - P1.x) - (P2.x - P1.x) * (P3.y - P1.y));

        float jHat = CurPos.y - P1.y - iHat * (P2.y - P1.y);
        jHat /= (P3.y - P1.y);

        if (iHat >= 0 && jHat >= 0 && ((iHat + jHat) <= 1f))
            return true;

        return false;
    }
}
