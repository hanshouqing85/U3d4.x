using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    //5个模型，从外部传入
    public GameObject car;
    public GameObject helicopter;
    public GameObject suv;
    public GameObject plane;
    public GameObject tank;
    //模型数组下标
    private int index;
    private GameObject[] models;
    private GameObject mCurrentGameObject;
    /******************************************/
    /*分割线之下的变量用于触摸手势镜头控制旋转和缩放*/
    /******************************************/

    //缩放系数
    float distance = 10.0f;
    //左右滑动移动速度
    float xSpeed = 250.0f;
    float ySpeed = 120.0f;
    //缩放限制系数
    int yMinLimit = -20;
    int yMaxLimit = 80;
    //摄像头的位置
    float x = 0.0f;
    float y = 0.0f;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

    void Start()
    {
        //初始化模型数组
        index = 0;
        models = new GameObject[5];
        models[0] = car;
        models[1] = helicopter;
        models[2] = suv;
        models[3] = plane;
        models[4] = tank;
        //克隆一个初始模型对象
        mCurrentGameObject = (GameObject)Instantiate(models[index], new Vector3(0, 0, 0), Quaternion.Euler(-20, 0, 0));
        //初始化镜头参数
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
    void Update()
    {
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            //触摸类型为移动触摸
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //根据触摸点计算X与Y位置
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            }
            //判断触摸数量为多点触摸
            if (Input.touchCount > 1)
            {
                //前两只手指触摸类型都为移动触摸
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    //计算出当前两点触摸点的位置
                    var tempPosition1 = Input.GetTouch(0).position;
                    var tempPosition2 = Input.GetTouch(1).position;
                    //函数返回真为放大，返回假为缩小
                    if (isEnlarge(oldPosition1, oldPosition2, tempPosition1, tempPosition2))
                    {
                        //放大系数超过3以后不允许继续放大
                        //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                        if (distance > 3)
                        {
                            distance -= 0.5f;
                        }
                    }
                    else
                    {
                        //缩小洗漱返回18.5后不允许继续缩小
                        //这里的数据是根据我项目中的模型而调节的，大家可以自己任意修改
                        if (distance < 18.5)
                        {
                            distance += 0.5f;
                        }
                    }
                    //备份上一次触摸点的位置，用于对比
                    oldPosition1 = tempPosition1;
                    oldPosition2 = tempPosition2;
                }
            }
        }
    }
    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            //放大手势
            return true;
        }
        else
        {
            //缩小手势
            return false;
        }
    }
    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
    void LateUpdate()
    {
        //mCurrentGameObject为我们当前模型对象，缩放旋转的参照物
        if (mCurrentGameObject.transform)
        {

            //重置摄像机的位置
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + mCurrentGameObject.transform.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    //当android中按下next，显示下一个模型
    void Next()
    {
        index = index + 1;
        if (index > models.Length - 1)
        {
            index = 0;
        }
        Debug.Log("next");
        // 摧毁当前对象
        Destroy(mCurrentGameObject);
        // 建立新的模型对象
        mCurrentGameObject = (GameObject)Instantiate(models[index]);
    }
    void Previous()
    {
        index = index - 1;
        if (index < 0)
        {
            index = models.Length - 1;
        }
        Debug.Log("previous");
        // 摧毁当前对象
        Destroy(mCurrentGameObject);
        // 建立新的模型对象
        mCurrentGameObject = Instantiate(models[index]) as GameObject;
    }
	
    void OnGUI()
    {
        //改变字体大小
        GUI.skin.label.fontSize =30;
        //定位显示(左边距x, 上边距y, 宽, 高)
//		GUI.Label(new Rect(10, 10, 900, 50), strTest);
//		GUI.Label(new Rect(10, 70, 900, 50), strTest1);
//		GUI.Label(new Rect(10, 130, 900, 50), strTest2);
//		GUI.Label(new Rect(10, 190, 900, 50), strTest3);
//		GUI.Label(new Rect(10, 250, 900, 50), strTest4);
//		GUI.Label(new Rect(10, 310, 900, 50), strTest5);
//		GUI.Label(new Rect(10, 370, 900, 50), strTest6);
//		GUI.Label(new Rect(10, 430, 900, 50), strTest7);
//		GUI.Label(new Rect(10, 490, 900, 50), strTest8);
		if (GUI.Button(new Rect(10, 100, 200, 100), "Quit"))
		{
			Application.Quit();
		}
    }
}
