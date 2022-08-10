using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThunderManager : MonoBehaviour
{
    [SerializeField] private Image m_Thunder0;

    public void DoThunder()
    {
        StartCoroutine(DoThunderProgess());
        // m_Thunder0.gameObject.SetActive(true);
        // Invoke("SetActiveFalse", 0.25f);
        // Invoke("SetActiveTrue", 0.25f);
        // Invoke("SetActiveFalse", 0.25f);
    }

    private IEnumerator DoThunderProgess()
    {
        m_Thunder0.color = new Color(0.9215686f,0.9215686f,0.9215686f, 0.9215686f);
        m_Thunder0.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        m_Thunder0.color = new Color(0.9215686f,0.9215686f,0.9215686f, 0.3f);
        // m_Thunder0.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        m_Thunder0.color = new Color(0.9215686f,0.9215686f,0.9215686f, 0.875f);
        m_Thunder0.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        m_Thunder0.gameObject.SetActive(false);
    }

    // private void SetActiveFalse()
    // {
    //     m_Thunder0.gameObject.SetActive(false);
    // }

    // private void SetActiveTrue()
    // {
    //     m_Thunder0.gameObject.SetActive(true);
    // }

}
