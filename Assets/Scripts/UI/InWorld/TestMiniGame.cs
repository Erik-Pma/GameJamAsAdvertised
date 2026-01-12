using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.InWorld
{
    public class TestMiniGame : Interactable
    {
        private InputAction _closeAction;
        private bool _closeAmt;
        
        protected override void Interact()
        {
            if (interationAmt)
            {
                actionAsset.FindActionMap("Movement").Disable();
                actionAsset.FindActionMap("MiniGames").Enable();
                SceneManager.LoadScene("MiniGame",LoadSceneMode.Additive);
                _closeAction = actionAsset.FindAction("Close");
                game.SetActive(true);
            }

            if (_closeAction != null && actionAsset.FindActionMap("MiniGames").enabled)
            {
                _closeAmt = _closeAction.IsPressed();
                //Cursor.lockState = CursorLockMode.Confined;
                //Cursor.visible = true;
                if (_closeAmt)
                {
                    CloseMiniGame();
                }
            }

        }
        
        protected void CloseMiniGame()
        {
            actionAsset.FindActionMap("Movement").Enable();
            actionAsset.FindActionMap("MiniGames").Disable();
            SceneManager.UnloadSceneAsync("MiniGame");
            game.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
}