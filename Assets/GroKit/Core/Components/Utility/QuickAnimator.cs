using UnityEngine;

namespace Core3lb
{
    public class QuickAnimator : MonoBehaviour
    {
        

        [SerializeField] float framesPerSecond = 12.0f;

        public bool animate = true;
        public bool isSpriteRenderer;
        [CoreHideIf("isSpriteRenderer")]
        public Texture2D[] textureList;
        Renderer myRender;
        [CoreShowIf("Use SpriteRenderer")]
        SpriteRenderer mySpriteRenderer;
        [CoreShowIf("isSpriteRenderer")]
        public Sprite[] spriteList;
        private void Awake()
        {
            myRender = GetComponent<Renderer>();
            if (isSpriteRenderer)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                return;
            }
        }

        public void _Animate(bool chg)
        {
            animate = chg;
        }

        private void FixedUpdate()
        {
            if(animate)
            {
                // Calculate index
                int index = (int)(Time.time * framesPerSecond);               
                if (isSpriteRenderer)
                {
                    index = index % spriteList.Length;
                    mySpriteRenderer.sprite = spriteList[index];
                }
                else
                {
                    index = index % textureList.Length;
                    myRender.material.mainTexture = textureList[index];
                }
            }
           
        }
    }
}
