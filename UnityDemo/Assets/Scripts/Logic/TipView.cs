using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Geek.Client
{
    public class TipView : MonoBehaviour
    {
        static TipView instance;
        public static TipView Ins
        {
            get
            {
                if (instance == null)
                {
                    var prefab = Resources.Load<GameObject>("TipPanel");
                    Instantiate(prefab);
                }
                return instance;
            }
        }

        public Text TipTxt;
        Coroutine cacheCoroutine;
        void Awake()
        {
            instance = this;
        }

        public void Notice(string tip)
        {
            TipTxt.text = tip;
            gameObject.SetActive(true);
            if (cacheCoroutine != null)
                StopCoroutine(cacheCoroutine);
            cacheCoroutine = StartCoroutine(delayHide());
        }

        IEnumerator delayHide()
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
        }
    }
}