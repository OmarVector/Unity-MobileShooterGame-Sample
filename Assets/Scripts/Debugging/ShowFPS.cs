using UnityEngine;
using UnityEngine.UI;

namespace Debugging
{
    public class ShowFPS : MonoBehaviour
    {
        private float deltaTime = 0.0f;
        private GUIStyle newStyle;
        private float fps;
        
        
        
        private void Start()
        {
            newStyle = GUIStyle.none;
            newStyle.normal.textColor = Color.red;
            newStyle.fontSize = 40;
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            fps = 1.0f / deltaTime;
        }
        
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), fps.ToString(),newStyle);
        }
    }
}
