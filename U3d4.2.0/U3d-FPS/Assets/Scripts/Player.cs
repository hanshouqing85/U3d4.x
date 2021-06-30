using UnityEngine;
using System.Collections;

[AddComponentMenu("Game/Player")]
public class Player : MonoBehaviour {

    // ���
    public Transform m_transform;
    CharacterController m_ch;


    // ��ɫ�ƶ��ٶ�
    float m_movSpeed = 3.0f;

    // ����
    float m_gravity = 2.0f;


    // �����
    Transform m_camTransform;

    // �������ת�Ƕ�
    Vector3 m_camRot;

    // ������߶�
    float m_camHeight = 1.4f;

    // ����ֵ
    public int m_life = 5;

    //ǹ��transform
    Transform m_muzzlepoint;

    // ���ʱ���������䵽����ײ��
    public LayerMask m_layer;

    // ����Ŀ��������Ч��
    public Transform m_fx;

    // �����Ч
    public AudioClip m_audio;

    // ������ʱ���ʱ��
    float m_shootTimer = 0;


	// Use this for initialization
	void Start () {

        // ��ȡ���
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();

        // ��ȡ�����
        m_camTransform = Camera.main.transform;

        // �����������ʼλ��
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;

        m_camRot = m_camTransform.eulerAngles;

        Screen.lockCursor = true;

        // ����muzzlepoint
        m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;
	
	}
	
	// Update is called once per frame
	void Update () {

        // �������Ϊ0��ʲôҲ����
        if (m_life <= 0)
            return;

        Control();
	}

    void Control()
    {
       
        //��ȡ����ƶ�����
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");

        // ��ת�����
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTransform.eulerAngles = m_camRot;

        // ʹ���ǵ��������������һ��
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;

        
        float xm = 0, ym = 0, zm = 0;

        // �����˶�
        ym -= m_gravity*Time.deltaTime;

        // ���������˶�
        if (Input.GetKey(KeyCode.W)){
            zm += m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S)){
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A)){
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D)){
            xm += m_movSpeed * Time.deltaTime;
        }

        // ����������ʱ��
        m_shootTimer -= Time.deltaTime;

        // ���������
        if (Input.GetMouseButton(0) && m_shootTimer<=0)
        {
            m_shootTimer = 0.1f;

            this.audio.PlayOneShot(m_audio);

            // ���ٵ�ҩ�����µ�ҩUI
            GameManager.Instance.SetAmmo(1);

            // RaycastHit�����������ߵ�̽����
            RaycastHit info;

            // ��muzzlepoint��λ�ã����������������������һ������
            // ����ֻ����m_layer��ָ���Ĳ���ײ
            bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if (hit)
            {
                // ���������tagΪenemy����Ϸ��
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();

                    // ���˼�������
                    enemy.OnDamage(1);
                }

                // �����еĵط��ͷ�һ������Ч��
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }


        //�ƶ�
        m_ch.Move( m_transform.TransformDirection(new Vector3(xm, ym, zm)) );

        // ʹ�������λ��������һ��
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;


        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(this.transform.position, "Spawn.tif");
    }

    // �˺�
    public void OnDamage(int damage)
    {
        m_life -= damage;

        // ����UI
        GameManager.Instance.SetLife(m_life);

        // �������Ϊ0�����������ʾ
        if (m_life <= 0)
            Screen.lockCursor = false;
    }


}
