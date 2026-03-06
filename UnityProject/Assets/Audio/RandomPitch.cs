using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitcher : MonoBehaviour
{
   private AudioSource source;

   [Header("MinMax")]
   public float minPitch = -1.5f; 
   public float maxPitch = 2f;  

   void Awake()
   {
      
      source = GetComponent<AudioSource>();
   }

   
   public void PlayRandomHover()
   {
      if (source != null)
      {
         
         float randomPitch = Random.Range(minPitch, maxPitch);

         
         if (randomPitch == 0) randomPitch = 1.0f;

         
         source.pitch = randomPitch;

        
         source.PlayOneShot(source.clip);

         
      }
   }
}