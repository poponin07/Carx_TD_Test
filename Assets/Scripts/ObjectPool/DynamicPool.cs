using System.Collections.Generic;
using UnityEngine;


namespace DynamicPool 
{
    public class DynamicPool : MonoBehaviour
    {
        private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
        
        public void CreatePool(GameObject prefab, int size, Transform parent)
        {
            if (!poolDictionary.ContainsKey(prefab))
            {
                poolDictionary[prefab] = new Queue<GameObject>();

               
            }
            for (int i = 0; i < size; i++)
            {
                GameObject newObj = Instantiate(prefab, parent);
                newObj.SetActive(false);
                poolDictionary[prefab].Enqueue(newObj);
            }
        }
        
        public GameObject GetFromPool(GameObject prefab, Transform parent)
        {
            if (!poolDictionary.ContainsKey(prefab) || poolDictionary[prefab].Count == 0)
            {
                CreatePool(prefab, 1, parent);
            }

            GameObject objToReuse = poolDictionary[prefab].Dequeue();
            return objToReuse;
        }
        
        public void ReturnToPool(GameObject prefab, GameObject obj)
        {
            obj.SetActive(false);

            if (!poolDictionary.ContainsKey(prefab))
            {
                Debug.LogError("There is no pool for this object");
                return;
            }

            //obj.transform.position = new Vector3(50, 50, 50);
            poolDictionary[prefab].Enqueue(obj);
        }
    }
}