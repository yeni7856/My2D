using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My2D
{
    //발사체 발사 (화살)
    public class ProjectileLauncher : MonoBehaviour
    {
        #region Variables
        public GameObject projectilePrefab;
        public Transform firePoint;
        #endregion
        public void Fire()
        {
            Debug.Log("화살 발사");
            //화살 포지션
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            //화살 킬 
            Destroy(projectile,2f);

            //로컬 스케일에 방향 결정
            Vector3 originScale = projectile.transform.localScale;
            
            //로컬스케일에 반전주기 
            projectile.transform.localScale = 
                new Vector3(originScale.x * transform.localScale.x > 0 ? 1 : -1 , 
                originScale.y, originScale.z);
        }
    }
}
