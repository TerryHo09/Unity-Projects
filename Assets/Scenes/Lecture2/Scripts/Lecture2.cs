using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Lecture2 : MonoBehaviour
{
    private float m_CurAngle = 0f;
    private Vector3 m_ViewHalfSize = Vector3.zero;

    public bool m_bUseTRS = false;

    public Vector3 m_Line1 = Vector3.zero;
    public Vector3 m_Line2 = Vector3.zero;

    public Matrix4x4 m_Mat = Matrix4x4.identity;

    public Matrix4x4 m_aa = Matrix4x4.identity;

    // Start is called before the first frame update
    void Start()
    {
        this.m_ViewHalfSize.x = Screen.width / 2f;
        this.m_ViewHalfSize.y = Screen.height / 2f;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    void OnGUI()
    {
        this.m_CurAngle += 1f;
        if ( this.m_CurAngle >= 360f )
            this.m_CurAngle = 0f;

        float CosVal = Mathf.Cos( this.m_CurAngle * Mathf.PI / 180f ); //Mathf.Deg2Rad
        float SinVal = Mathf.Sin( this.m_CurAngle * Mathf.PI / 180f );

        this.m_Mat = Matrix4x4.identity;

        if ( this.m_bUseTRS == false )
        {
            this.m_Mat.m00 = CosVal;
            this.m_Mat.m01 = -SinVal;
            this.m_Mat.m03 = this.m_ViewHalfSize.x;

            this.m_Mat.m10 = SinVal;
            this.m_Mat.m11 = CosVal;
            this.m_Mat.m13 = this.m_ViewHalfSize.y;
        }
        else
        {
            this.m_Mat.SetTRS( this.m_ViewHalfSize, Quaternion.Euler( 0f, 0f, this.m_CurAngle ), Vector3.one );
        }

        Handles.matrix = this.m_Mat;

        Handles.DrawLine( this.m_Line1, this.m_Line2 );
    }
}
