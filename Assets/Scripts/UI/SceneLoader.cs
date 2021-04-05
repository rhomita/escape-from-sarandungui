using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        private Animator _animator;

        void Start()
        {
            _animator = transform.GetComponent<Animator>();
        }

        public void GoToScene(string scene)
        {
            Time.timeScale = 1;
            StartCoroutine(GoScene(scene));
        }

        IEnumerator GoScene(string scene)
        {
            _animator.SetTrigger("ChangeScene");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}