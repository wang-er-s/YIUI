using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(Camera))]
    public class Camera2DFitScreen : MonoBehaviour
    {
        [SerializeField]
        private int DefaultWidth = 720;
        [SerializeField]
        private int DefaultHeight = 1280;
        // orthographicSize表示相机视野高度的一半
        [SerializeField]
        private float DefaultCameraSize = 5;
        public float CameraViewWidth { get; private set; }
        public float CameraViewHeight { get; private set; }
        public Camera Camera { get; private set; }
        
        private void Awake()
        {
            this.Camera = this.GetComponent<Camera>();
            this.CameraViewWidth = 2 * DefaultCameraSize * (DefaultWidth * 1.0f / DefaultHeight);
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
#if UNITY_EDITOR
            screenSize = UnityEditor.Handles.GetMainGameViewSize();
#endif
            this.CameraViewHeight = this.CameraViewWidth * (screenSize.y * 1.0f / screenSize.x);
            print($"Width:{this.CameraViewWidth} Height:{this.CameraViewHeight}");
            var size = (screenSize.y * 1.0f / screenSize.x) * this.CameraViewWidth / 2;
            this.Camera.orthographicSize = size;
        }
    }
}