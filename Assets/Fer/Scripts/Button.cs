using UnityEngine.UI;
using UnityEngine;

public class Button : MonoBehaviour
{
    bool m_buttonHit;
    Image m_image;
    //m_color;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        m_image.alphaHitTestMinimumThreshold = 0.0001f;
    }
}
