using UnityEngine;

namespace Game.Scripts {
    [RequireComponent(typeof(CrabController))]
    public class PlayerController : MonoBehaviour {

        private CrabController controller;
        
        // Start is called before the first frame update
        private void Awake() {
            controller = GetComponent<CrabController>();
        }

        // Update is called once per frame
        void Update() {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            var verticalInput = Input.GetAxisRaw("Vertical");
            
            controller.Handle(horizontalInput, verticalInput);
        }      
    }
}