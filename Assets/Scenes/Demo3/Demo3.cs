using UnityEngine;
using UnityEditor;

public class Demo3 : MonoBehaviour
{
    /// <summary>
    /// 螢幕中心點座標
    /// </summary>
    private Vector2 m_ScrrenCenter = Vector2.zero;

    /// <summary>
    /// 基向量
    /// </summary>
    private Vector3 m_BaseVector = Vector3.zero;

    /// <summary>
    /// 原點
    /// </summary>
    public Vector3 m_OriVector = Vector3.zero;

    /// <summary>
    /// 與原點間的移動距離
    /// </summary>
    public Vector3 m_MoveVector = Vector3.zero;

    /// <summary>
    /// 起點(直角座標系)
    /// </summary>
    private Vector3 m_StartPos = Vector3.zero;

    /// <summary>
    /// 點擊時滑鼠座標,也是終點(直角座標系)
    /// </summary>
    private Vector3 m_MousePos = Vector3.zero;

    /// <summary>
    /// 起點與原點所形成的夾角
    /// </summary>
    private float m_OS_Deg = 0f;

    /// <summary>
    /// 終點與原點所形成的夾角
    /// </summary>
    private float m_OM_Deg = 0f;

    private bool m_bPlay = false;

    private float m_CurTicks = 0f;

    /// <summary>
    /// 動畫演譯的時間
    /// </summary>
    public float m_TotalTicks = 90f;

    void Start()
    {
        this.m_BaseVector.x = 1f;

        this.m_ScrrenCenter.x = Screen.width * 0.5f;
        this.m_ScrrenCenter.y = Screen.height * 0.5f;
    }

    void Update()
    {
    }

    private void OnGUI()
    {
        this.m_StartPos = this.m_OriVector + this.m_MoveVector; //起點為原點加上移動的距離

        if ( Input.GetMouseButton(0) )
        {
            //第一步先求出起點與原點所形成的直角座標系夾角
            this.m_OS_Deg = Mathf.Atan2( this.m_StartPos.y - this.m_OriVector.y, this.m_StartPos.x - this.m_OriVector.x ) * Mathf.Rad2Deg;
            if ( this.m_OS_Deg < 0f )
                this.m_OS_Deg += 360f;

            //第二步將滑鼠座標轉換為直角座標系
            this.m_MousePos = Input.mousePosition;
            this.m_MousePos.x -= this.m_ScrrenCenter.x;
            this.m_MousePos.y -= this.m_ScrrenCenter.y;

            //第三步求出滑鼠座標與原點所形成的直角座標系夾角
            this.m_OM_Deg = Mathf.Atan2( this.m_MousePos.y - this.m_OriVector.y, this.m_MousePos.x - this.m_OriVector.x ) * Mathf.Rad2Deg;
            if ( this.m_OM_Deg < 0f )
                this.m_OM_Deg += 360f;

            if ( Mathf.Abs( this.m_OM_Deg - this.m_OS_Deg ) > 180f )
            {
                if ( this.m_OM_Deg > this.m_OS_Deg )
                    this.m_OS_Deg += 360f;
                else
                    this.m_OM_Deg += 360f;
            }
            this.m_bPlay = true;
            this.m_CurTicks = 1f;
        }

        Matrix4x4 DrawMatrix = Matrix4x4.identity; //Handles繪圖時的座標系

        //繪圖座標系矩陣(SetTRS與下方單獨設定矩陣元素結果相同)
        //DrawMatrix.SetTRS( new Vector3( this.m_ScrrenCenter.x, this.m_ScrrenCenter.y, 0f ), Quaternion.Euler( 0f, 0f, 0f ), new Vector3( 1f, -1f, 1f ) );
        DrawMatrix.m03 = this.m_ScrrenCenter.x;
        DrawMatrix.m11 = -1f;
        DrawMatrix.m13 = this.m_ScrrenCenter.y;

        Handles.matrix = Matrix4x4.identity;
        Handles.matrix = DrawMatrix;
        Handles.color = Color.white;
        Handles.DrawLine( this.m_OriVector, this.m_StartPos ); //畫出起點線段

        if ( this.m_bPlay ) //開始進行動畫演譯
        {
            //第一步先計算目前演譯中的旋轉角度(使用插值運算)
            float CurDeg = Mathf.Lerp( this.m_OS_Deg, this.m_OM_Deg, this.m_CurTicks / this.m_TotalTicks );

            //UnityEngine.Debug.Log( this.m_OS_Deg.ToString() + " " + this.m_OM_Deg.ToString() + " " + ( this.m_OM_Deg - this.m_OS_Deg ).ToString() + " " + CurDeg.ToString() );

            float CosVal = Mathf.Cos( CurDeg * Mathf.Deg2Rad ); //計算餘弦值
            float SinVal = Mathf.Sin( CurDeg * Mathf.Deg2Rad ); //計算正弦值

            //設定變換矩陣並轉換為繪畫座標系(先縮放、再旋轉、最後平移)
            Matrix4x4 TRSMatrix = Matrix4x4.identity;
            TRSMatrix.SetTRS( new Vector3( this.m_ScrrenCenter.x + this.m_OriVector.x, this.m_ScrrenCenter.y - this.m_OriVector.y, 0f ), Quaternion.Euler( 0f, 0f, -CurDeg ), new Vector3( 1f, -1f, 1f ) );

            Handles.matrix = Matrix4x4.identity; //先恢復繪圖矩陣
            Handles.matrix = TRSMatrix; //再設定為變換矩陣
            Handles.color = Color.green;

            //求出起點的線段長度
            float LineLength = Mathf.Sqrt( Mathf.Pow( this.m_StartPos.x - this.m_OriVector.x, 2 ) + Mathf.Pow( this.m_StartPos.y - this.m_OriVector.y, 2 ) );

            Vector3 NewPos = Vector3.zero;
            NewPos.x = LineLength;
            Handles.DrawLine( Vector3.zero, NewPos );

            this.m_CurTicks += 1f;
            if ( this.m_CurTicks > this.m_TotalTicks )
                this.m_bPlay = false;
        }
    }
}