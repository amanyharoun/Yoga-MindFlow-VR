using UnityEngine;

namespace Core3lb
{
    public class DoDestroy : MonoBehaviour
    {
        [Tooltip("Starts Destory when the Object runs Start do not use 0 destory time for this")]
        public bool startDestoryOnStart = false;

        public float destroyTime = 0;

        public void Start()
        {
            if (startDestoryOnStart)
            {
                if (destroyTime == 0)
                {
                    Debug.LogError($"{name} has destory time 0 so I am gone instantly");
                }
                _DestroySelf();
            }
        }
        public virtual void _DestroySelf()
        {
            DestroyInternal(gameObject);
        }

        public virtual void _DestroyObject(GameObject go)
        {
            DestroyInternal(go);
        }

        protected void DestroyInternal(GameObject go)
        {
            Destroy(go,destroyTime);
        }
    }
}
