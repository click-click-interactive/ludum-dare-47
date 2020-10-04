using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    public Vector2 speed;
    private Material material;
    private Vector2 uvOffset = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // uvOffset += new Vector2(0, - 2.0f * Time.deltaTime);
        // if (uvOffset.y > 1.0f)
        // {
        //     // keep the "V" value between 0 and 1 using Math.Truncate. same idea as using frac() in the shader code
        //     uvOffset.y -= (float) System.Math.Truncate(uvOffset.y);
        // }
        // material.SetTextureOffset("_MainTex", uvOffset);
        material.SetTextureOffset("_BaseMap", material.mainTextureOffset + speed * Time.deltaTime);
        // material.mainTextureOffset += speed;
    }
}
