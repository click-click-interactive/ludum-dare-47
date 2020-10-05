using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulse : MonoBehaviour
{
         private Renderer renderer;
         public Color colourEnd;
         public Color colourStart;
         private float i;
         public float rate;
      void Start() {
          colourEnd = new Color(Random.value, Random.value, Random.value);
          colourStart = colourEnd;
          colourEnd.a = 0;
          renderer = GetComponent<Renderer>();
      }
      void Update () {
          // Blend towards the current target colour
          i += Time.deltaTime*rate;
          renderer.material.color = Color.Lerp (colourStart, colourEnd, Mathf.PingPong(i * 2, 1) );
          // If we've got to the current target colour, choose a new one
          if(i >= 1) {
              i = 0;
              colourEnd = new Color(Random.value, Random.value, Random.value);
              colourStart = colourEnd;
              colourEnd.a = 0;
          }
      }
}
