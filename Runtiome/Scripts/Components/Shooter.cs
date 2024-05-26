using UnityEngine;

namespace LCHFramework.Components
{
    public class Shooter : MonoBehaviour
    {
        // ?? 앵글이 45도인데 왜 45도로 셋팅해서 하는지는 모르겠다.
        public float Angle = 45.0f;
        // 기본 파워값 
        public float Power = 0.01f;


        private Transform makePos;
        private bool m_bStartCheck = false;
        //스타트에서 중력값을 계산하려고 한번 받아놓는 듯
        private float Timedir = Time.deltaTime;
        //중력값
        private float Gravity;
        private Vector3 v1;



        void Start()
        {
            makePos = GameObject.Find("BulletManager").transform;

            //고정 중력값
            //리얼한 중력값을 구하려고 이런식으로 하는 거 같다. 중력 계산값은
            //0,98888로 하면 더 리얼한 중력값을 받게끔 할 수 있다.
            Gravity = -(1.0f * Timedir * Timedir / 2.0f);
        }

        void Update()
        {
            if (m_bStartCheck == false) return;
            Timedir += Time.deltaTime;
            v1.z = Power = Mathf.Cos(Angle * Mathf.PI / 180.0f) * Timedir;//핵심코드
            v1.y = Power = Mathf.Cos(Angle * Mathf.PI / 180.0f) * Timedir * Gravity;//핵심코드
            transform.Translate(v1);

            transform.Rotate(new Vector3(Mathf.Cos(Angle * Mathf.PI / 180.0f), 0, 0));//핵심코드
        }

        void OnTriggerEnter(Collider col)
        {
            Collider collider = col.GetComponent<Collider>();

            if (collider.tag == "Player")
            {
                transform.position = makePos.position;
                v1 = new Vector3(0, 0, 0);//충돌하였으니 초기화
                Power = 0.0f;//충돌하였으니 초기화
                m_bStartCheck = false;
            }
            else if (collider.tag != "Enemy")
            {
                transform.position = makePos.position;
                v1 = new Vector3(0, 0, 0);//충돌하였으니 초기화
                Power = 0.0f;//충돌하였으니 초기화
                m_bStartCheck = false;
                Debug.Log(collider.tag);
            }
        }



        public void SetArrowCheck(bool check, Vector3 pos, Quaternion q)
        {
            //화살을 불러오는 함수
            m_bStartCheck = check;
            transform.position = pos;
            transform.rotation = q;
        }

        public bool GetArrowCheck()
        {
            return m_bStartCheck;
        }
    }
}

