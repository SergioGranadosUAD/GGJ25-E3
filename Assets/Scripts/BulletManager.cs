using NUnit.Framework;
using UnityEngine;

using System.Collections.Generic;
public class BulletManager : MonoBehaviour
{
    struct BulletData
    {
        public int playerID;
        public GameObject bullet;
    }
    List<BulletData> bulletList = new List<BulletData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void addNewBullet(GameObject bullet, int playerID)
    {
        BulletData data = new BulletData();

        data.playerID = playerID;
        data.bullet = bullet;

        bulletList.Add(data);
    }

    public int getPlayerID(GameObject bullet)
    {
        for (int i = bulletList.Count - 1; i > bulletList.Count; i--)
        {
            if (bulletList[i].bullet == bullet)
            {
                bulletList.Remove(bulletList[i]);
                return bulletList[i].playerID;
            }
        }
        return 0;
    }
}
