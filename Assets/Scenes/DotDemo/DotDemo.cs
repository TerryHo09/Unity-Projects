using UnityEngine;
using UnityEditor;

public class DotDemo : MonoBehaviour
{
    public Vector3 m_OriPos = Vector3.zero; //直角座標系

    [SerializeField]
    private float m_Fov;
    public float Fov //視野
    {
        get { return this.m_Fov; }

        set
        {
            if ( value > 180f )
                value = 180f;
            this.m_Fov = value;
        }
    }

    public float m_Far = 100f; //可看見的距離

    private Vector2 m_HalfScreenSize = Vector2.zero;

    private Vector3 m_CurPos = Vector3.zero; //Ori座標系

    private Matrix4x4 m_DrawToCartMat = Matrix4x4.identity; //繪圖轉直角座標矩陣
    private Matrix4x4 m_ScreenToCartMat = Matrix4x4.identity; //螢幕轉直角座標矩陣

    private Matrix4x4 m_CartToDrawMat = Matrix4x4.identity; //直角轉繪圖座標矩陣
    private Matrix4x4 m_CartToScreenMat = Matrix4x4.identity; //直角轉螢幕座標矩陣

    private float m_CurRotDeg = 0f; //目前旋轉角度

    void Start()
    {
        this.m_HalfScreenSize.x = Screen.width * 0.5f;
        this.m_HalfScreenSize.y = Screen.height * 0.5f;

        this.m_DrawToCartMat.SetTRS( new Vector3( -this.m_HalfScreenSize.x, this.m_HalfScreenSize.y, 0f ), Quaternion.Euler( Vector3.zero ), new Vector3( 1f, -1f, 1f ) );
        this.m_ScreenToCartMat.SetTRS( new Vector3( -this.m_HalfScreenSize.x, -this.m_HalfScreenSize.y, 0f ), Quaternion.Euler( Vector3.zero ), Vector3.one );

        //this.m_CartToDrawMat.SetTRS( new Vector3( this.m_HalfScreenSize.x, this.m_HalfScreenSize.y, 0f ), Quaternion.Euler( Vector3.zero ), new Vector3( 1f, -1f, 1f ) );
        //this.m_CartToScreenMat.SetTRS(new Vector3( this.m_HalfScreenSize.x, this.m_HalfScreenSize.y, 0f ), Quaternion.Euler( Vector3.zero ), Vector3.one );

        this.m_CartToDrawMat = this.m_DrawToCartMat.inverse;
        this.m_CartToScreenMat = this.m_ScreenToCartMat.inverse;
    }

    void OnGUI()
    {
        this.DrawRadar();
    }

    void DrawRadar() //畫雷達
    {
        if ( !IsInAreaByDot() ) //不在雷達掃描範圍內則不累加角度
        {
            this.m_CurRotDeg += 1f;
            if ( this.m_CurRotDeg >= 360f )
                this.m_CurRotDeg = 0f;
        }

        Vector3 NewPos = this.m_OriPos;
        NewPos.x += this.m_Far;

        Matrix4x4 RotMat = Matrix4x4.identity;
        Matrix4x4 MoveBackMat = Matrix4x4.identity;

        float HalfFov = this.m_Fov / 2;

        float Min = this.m_CurRotDeg - HalfFov;
        float Max = this.m_CurRotDeg + HalfFov;

        float Cur = Min;
        while ( Cur < Max )
        {
             RotMat.SetTRS( -this.m_OriPos, Quaternion.Euler( Vector3.zero ), Vector3.one );
             MoveBackMat.SetTRS( this.m_OriPos, Quaternion.Euler( 0f, 0f, Cur ), Vector3.one );
             this.m_CurPos = ( MoveBackMat * RotMat ).MultiplyPoint( NewPos );

             Handles.matrix = this.m_CartToDrawMat;
             Handles.DrawLine( this.m_OriPos, this.m_CurPos );

             Cur += 0.1f;
        }
    }

    bool IsInAreaByDot()
    {
        Vector3 V1 = new Vector3( this.m_Far, 0f, 0f );

        Matrix4x4 RotMat = Matrix4x4.identity;
        RotMat.SetTRS( Vector3.zero, Quaternion.Euler( 0f, 0f, this.m_CurRotDeg ), Vector3.one );
        V1 = RotMat.MultiplyPoint( V1 );

        Vector3 V2 = this.m_ScreenToCartMat.MultiplyPoint( Input.mousePosition );

        V2 = V2 - this.m_OriPos;

        float DotVal = Vector3.Dot( V1.normalized, V2.normalized );

        if ( DotVal >= 0f )
        {
            float CosVal = DotVal / ( V1.normalized.magnitude * V2.normalized.magnitude );
            float CosHalfVal = Mathf.Cos( ( this.m_Fov / 2 ) * Mathf.Deg2Rad );

            if ( CosVal >= CosHalfVal && CosVal <= 1f )
            {
                if ( V2.magnitude <= this.m_Far )
                    return true;
            }
        }
        return false;
    }
}