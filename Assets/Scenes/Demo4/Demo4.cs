using UnityEngine;
using UnityEditor;

public class Demo4 : MonoBehaviour
{
    /// <summary>
    /// 螢幕中心點座標
    /// </summary>
    private Vector2 m_ScrrenCenter = Vector2.zero;

    /// <summary>
    /// 原點
    /// </summary>
    public Vector3 m_OriVector = Vector3.zero;

    /// <summary>
    /// 三角形3個點的座標(相對於OriVector的位移量)
    /// </summary>
    public Vector3[] m_TrianglePos;

    /// <summary>
    /// 螢幕座標系轉直角座標系
    /// </summary>
    private Matrix4x4 mScreenToCartesianCoordMat = Matrix4x4.identity;

    /// <summary>
    /// 直角座標系轉螢幕座標系
    /// </summary>
    private Matrix4x4 mCartesianCoordToScreenMat = Matrix4x4.identity;

    /// <summary>
    /// 繪圖座標系轉直角座標系
    /// </summary>
    private Matrix4x4 mDrawToCartesianCoordMat = Matrix4x4.identity;

    /// <summary>
    /// 直角座標系轉繪圖座標系
    /// </summary>
    private Matrix4x4 mCartesianCoordToDrawMat = Matrix4x4.identity;

    void Start()
    {
        this.m_ScrrenCenter.x = Screen.width * 0.5f;
        this.m_ScrrenCenter.y = Screen.height * 0.5f;

        this.mScreenToCartesianCoordMat.m03 = -this.m_ScrrenCenter.x;
        this.mScreenToCartesianCoordMat.m13 = -this.m_ScrrenCenter.y;

        this.mCartesianCoordToScreenMat.m03 = this.m_ScrrenCenter.x;
        this.mCartesianCoordToScreenMat.m13 = this.m_ScrrenCenter.y;

        this.mDrawToCartesianCoordMat.m03 = -this.m_ScrrenCenter.x;
        this.mDrawToCartesianCoordMat.m11 = -1f;
        this.mDrawToCartesianCoordMat.m13 = this.m_ScrrenCenter.y;

        this.mCartesianCoordToDrawMat.m03 = this.m_ScrrenCenter.x;
        this.mCartesianCoordToDrawMat.m11 = -1f;
        this.mCartesianCoordToDrawMat.m13 = this.m_ScrrenCenter.y;
    }

    private void Update()
    {
        
    }

    void OnGUI()
    {
        if ( this.m_TrianglePos.Length != 3 )
            return;

        Handles.matrix = Matrix4x4.identity;
        Handles.matrix = this.mCartesianCoordToDrawMat;

        Vector3[] TriPos = new Vector3[3];
        for ( int i = 0; i < this.m_TrianglePos.Length; i++ )
             TriPos[i] = this.m_OriVector + this.m_TrianglePos[i];

        Vector3 MousePos = this.mScreenToCartesianCoordMat.MultiplyPoint( Input.mousePosition ); //將螢幕座標系下的滑鼠座標轉為直角座標系

        if ( IsPtInTriangle( MousePos, TriPos ) )
            Handles.color = Color.green;
        else
            Handles.color = Color.white;

        Handles.DrawLine( TriPos[0], TriPos[1] );
        Handles.DrawLine( TriPos[1], TriPos[2] );
        Handles.DrawLine( TriPos[2], TriPos[0] );
    }

    float CalcuDeterminant( Vector3 P1, Vector3 P2, Vector3 P3 )
    {
        return ( ( P1.x - P3.x ) * ( P2.y - P3.y ) ) - ( ( P2.x - P3.x ) * ( P1.y - P3.y ) );
    }

    bool IsPtInTriangle( Vector3 Pt, Vector3[] TrianglePt )
    {
        float A1 = CalcuDeterminant( Pt, TrianglePt[0], TrianglePt[1] );
        float A2 = CalcuDeterminant( Pt, TrianglePt[1], TrianglePt[2] );
        float A3 = CalcuDeterminant( Pt, TrianglePt[2], TrianglePt[0] );

        if ( A1 > 0 && A2 > 0 && A3 > 0 )
            return true;

        if ( A1 < 0 && A2 < 0 && A3 < 0 )
            return true;

        return false;
    }
}