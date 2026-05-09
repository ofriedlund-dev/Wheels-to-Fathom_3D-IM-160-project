/*****************************************************************************
// File Name : Shermann.cs
// Author : Owen M. Friedlund
// Creation Date : April 16, 2026
//
// Brief Description : This is a document that holds the code for the Shermann easter egg in the game
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class Shermann : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip1;
    [SerializeField] private AudioClip audioClip2;
    /// <summary>
    /// This is the start function for the Shermann easter egg
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GravityFlip());
    }
    /// <summary>
    /// This is the coroutine that flips gravity every 2.5 seconds and plays a sound effect when it does so
    /// </summary>
    /// <returns></returns>
    private IEnumerator GravityFlip()
    {
        while (true)
        {
            GetComponent<Rigidbody>().AddForce(0, -2500, 0);
            audioSource.PlayOneShot(audioClip1);
            yield return new WaitForSeconds(2.5f);
            GetComponent<Rigidbody>().AddForce(0, 2500, 0);
            audioSource.PlayOneShot(audioClip2);
            yield return new WaitForSeconds(2.5f);
        }
    }
}
