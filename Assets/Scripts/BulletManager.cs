using NUnit.Framework;
using UnityEngine;

using System.Collections.Generic;
public class BulletManager : MonoBehaviour
{
    struct BulletData
    {
        public int IDplayer;
        public GameObject bullet;
    }
    List<BulletData> listBullets = new List<BulletData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void setNewBullet(GameObject bullet, int IDplayer)
    {
        BulletData data = new BulletData();

        data.IDplayer = IDplayer;
        data.bullet = bullet;

        listBullets.Add(data);
    }

    public int getIDplayer(GameObject bullet)
    {
        for (int i = listBullets.Count - 1; i > listBullets.Count; i--)
        {
            if (listBullets[i].bullet == bullet)
            {
                listBullets.Remove(listBullets[i]);
                return listBullets[i].IDplayer;
            }
        }
        return 0;
    }
}
