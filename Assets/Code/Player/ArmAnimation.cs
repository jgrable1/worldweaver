using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmAnimation : MonoBehaviour
{
    private BasicMovement player;

    public float frequency = 1;
    public float degrees = 60;
    public float phase = 0;
    public bool right;
    private float deltaDegrees = 0, swingSpeed = 2;
    Quaternion baseRot;
    float start;
    private bool toolUse = false;
    // Start is called before the first frame update
    void Start()
    {
        baseRot = transform.localRotation;
        start = degrees * Mathf.Sin(phase * 2 * Mathf.PI);
        player = transform.parent.parent.gameObject.GetComponent<BasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!toolUse){
            if(player.IsWalking()){
                deltaDegrees += (phase + frequency * Time.deltaTime) * 2 * Mathf.PI;
                transform.localRotation = baseRot * Quaternion.Euler(
                    degrees * (Mathf.Sin(deltaDegrees*(right ? 1 : -1))/2) - start, 0, 0);
                //transform.localRotation = baseRot * Quaternion.Euler(
                //    degrees * (Mathf.Sin((phase + frequency * Time.time) * 2 * Mathf.PI*(right ? 1 : -1))/2) - start, 0, 0);
            } else {
                if(baseRot != transform.localRotation){
                    deltaDegrees += (phase + frequency * Time.deltaTime) * 4 * Mathf.PI;
                    if(Mathf.Abs(deltaDegrees%(2*Mathf.PI)) < 0.1){
                        transform.localRotation = baseRot;
                        deltaDegrees = 0;
                    } else {
                        transform.localRotation = baseRot * Quaternion.Euler(
                            degrees * (Mathf.Sin(deltaDegrees*(right ? 1 : -1))/2) - start, 0, 0);
                
                    }
                }
            }
        } else {
            deltaDegrees -= (phase + frequency * Time.deltaTime) * swingSpeed * Mathf.PI;
            transform.localRotation = baseRot * Quaternion.Euler(
            degrees * (Mathf.Sin(deltaDegrees*(right ? 1 : -1))/2) - start, 0, 0);
        }
        
    }

    public IEnumerator SwingTool() {
        deltaDegrees = 0;
        degrees *= 3;
        toolUse = true;
        yield return new WaitForSeconds(0.25f);
        swingSpeed = 4;
        yield return new WaitForSeconds(0.25f);
        swingSpeed = 2;
        yield return new WaitForSeconds(0.25f);
        degrees /= 3;
        toolUse = false;
        transform.parent.parent.GetComponent<BasicMovement>().worldObject.GetComponent<World>().EndSwing();
    }

}
