
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainsManager : MonoBehaviour
{
    public GameObject terrainPrefab;
    public List<GameObject> terrains = new List<GameObject>();
    public GameObject currentTerrain;
    public bool allowInst;

    public void OnCenterEnter(GameObject target)
    {


        foreach (var terrain in terrains.ToList())
        {
            if (terrain != target)
            {
                terrains.Remove(terrain);
                Destroy(terrain);
            }
        }
        allowInst = true;
        currentTerrain = target;

    }

    public void OnCenterExit(int side, Transform curTerrian)
    {

        if (curTerrian.gameObject == currentTerrain)
        {
            allowInst = true;
        }
        else
        {
            currentTerrain = curTerrian.gameObject;
        }



        if (allowInst)
        {
            var pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z);

            if (side < 4)
            {
                switch (side)
                {
                    case 0:
                        pos = new Vector3(curTerrian.position.x - 400, curTerrian.position.y, curTerrian.position.z);
                        break;

                    case 1:
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z + 400);
                        break;

                    case 2:
                        pos = new Vector3(curTerrian.position.x + 400, curTerrian.position.y, curTerrian.position.z);
                        break;

                    case 3:
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z - 400);
                        break;
                }

                Debug.Log(curTerrian.position);
                Debug.Log(pos);


                var ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                terrains.Add(ter);
            }
            else
            {
                switch (side)
                {
                    case 4:
                        pos = new Vector3(curTerrian.position.x - 400, curTerrian.position.y, curTerrian.position.z);
                        var ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x - 400, curTerrian.position.y, curTerrian.position.z+400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z + 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        break;

                    case 5:
                        pos = new Vector3(curTerrian.position.x + 400, curTerrian.position.y, curTerrian.position.z);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x + 400, curTerrian.position.y, curTerrian.position.z + 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z + 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        break;

                    case 6:
                        pos = new Vector3(curTerrian.position.x + 400, curTerrian.position.y, curTerrian.position.z);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x + 400, curTerrian.position.y, curTerrian.position.z - 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z - 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        break;

                    case 7:
                        pos = new Vector3(curTerrian.position.x - 400, curTerrian.position.y, curTerrian.position.z);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x - 400, curTerrian.position.y, curTerrian.position.z - 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        pos = new Vector3(curTerrian.position.x, curTerrian.position.y, curTerrian.position.z - 400);
                        ter = Instantiate(terrainPrefab, pos, Quaternion.identity, this.transform);
                        terrains.Add(ter);
                        break;
                }
            }
            allowInst = false;
        }
    }


}
