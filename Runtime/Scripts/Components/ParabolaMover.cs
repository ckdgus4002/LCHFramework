using System;
using System.Collections;
using LCHFramework.Utilities;
using UnityEngine;

namespace LCHFramework.Components
{
    public class ParabolaMover : LCHMonoBehaviour
    {
        public float max_height;


        private float gravity = 9.8f;
        private Transform bullet;   // 포물체
        private float tx, ty, tz, v, elapsed_time, t, dat;
        private Vector3 start_pos, end_pos;



        /// <summary>
        /// 높이보다 시작지점, 끝지점이 낮을경우는 동작 하지 않습니다.
        /// </summary>
        /// <remarks>
        /// 출처: https://smilejsu.tistory.com/1036 [{ 일등하이 :Unity3D }]
        /// </remarks>
        private Coroutine _move;
        public void Move(Transform bullet, Vector3 startPos, Vector3 endPos, float g, float max_height, Action onComplete)
        {
            start_pos = startPos;
            end_pos = endPos;
            gravity = g;
            this.max_height = max_height;
            this.bullet = bullet;
            this.bullet.position = start_pos;

            var dh = endPos.y - startPos.y;
            var mh = max_height - startPos.y;

            ty = Mathf.Sqrt(2 * this.gravity * mh);

            float a = this.gravity;
            float b = -2 * ty;
            float c = 2 * dh;

            dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            tx = -(startPos.x - endPos.x) / dat;
            tz = -(startPos.z - endPos.z) / dat;
            elapsed_time = 0;

            _move = RestartCoroutine(_move, Coroutine());
            IEnumerator Coroutine()
            {
                while (true)
                {
                    elapsed_time += Time.deltaTime;

                    var tx = start_pos.x + this.tx * elapsed_time;
                    var ty = start_pos.y + this.ty * elapsed_time - 0.5f * gravity * elapsed_time * elapsed_time;
                    var tz = start_pos.z + this.tz * elapsed_time;
                    var tpos = new Vector3(tx, ty, tz);

                    bullet.transform.LookAt(tpos);
                    bullet.transform.position = tpos;

                    if (dat <= elapsed_time) break;
                    else yield return null;
                }

                onComplete();
            }
        }
    }
}