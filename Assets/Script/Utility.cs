using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Utility
{
    public static void ObliqueProjectileMotion(ref Vector3 startPos,ref Vector3 destPos, float angle, float speed, float time)
    {
        destPos.x = startPos.x + speed * time * Mathf.Cos(angle * Mathf.Deg2Rad);
        destPos.y = startPos.y + speed * Mathf.Sin(angle * Mathf.Deg2Rad) * time - 9.8f *(float)Mathf.Pow(time,2)/2;
    }


    //Find angle, LMax, Vector2 Maxforce.
    public static void ObliqueProjectileMotion(Vector2 startPos, Vector2 curPos, Vector2 desPos,
                                        ref Vector2 maxVectorForce, float FixedTimeV0, 
                                        float HMax, 
                                        ref float angle,
                                        ref bool isMaxHeigh,
                                        Rigidbody2D rgb2D)
    {
        if(isMaxHeigh) return;
        
        float coefficient                 = 2;
        float deltaTime                   = Time.fixedDeltaTime;
        
        float gravity                     = Physics.gravity.y;
        float gravityScale                = rgb2D.gravityScale;
        
        // Calculate angle for ObliqueProjectileMotion
        if(angle == Mathf.Epsilon)
        {
            float LMax  = Vector2.Distance(startPos, desPos);
            angle       = Mathf.Atan(4 * HMax / LMax) * Mathf.Rad2Deg;
            // Debug.Log("L: " + LMax + " HMax: " + HMax);
        }

        if(maxVectorForce == Vector2.zero)
        {
            float V0            = Mathf.Sqrt(4 * HMax * (-Physics.gravity.y) * gravityScale / (coefficient * Mathf.Pow(Mathf.Sin(angle * Mathf.Deg2Rad), 2)));

            maxVectorForce.x    = V0 * Mathf.Cos(angle * Mathf.Deg2Rad);
            maxVectorForce.y    = V0 * Mathf.Sin(angle * Mathf.Deg2Rad) + gravity * gravityScale / coefficient;
        }
        
        // With upY
        if(!isMaxHeigh)
        {
            
            if(curPos.y - startPos.y < HMax)
            {
                // if((Mathf.Abs(m_rgb2D.velocity.x) < MathF.Abs(maxVectorForce.x)) && 
                //     (Mathf.Abs(m_rgb2D.velocity.y) < MathF.Abs(maxVectorForce.y)))
                if(rgb2D.velocity.magnitude < maxVectorForce.magnitude)
                {
                    
                    float distance          = HMax - (curPos.y - startPos.y);
                    

                    float Vector0ByDistance = Mathf.Sqrt(4 * (-gravity) * gravityScale * distance / (coefficient * Mathf.Pow(Mathf.Sin(angle * Mathf.Deg2Rad), 2)));
                    

                    Vector2 distanceVector  = new Vector2 ( Vector0ByDistance * Mathf.Cos(angle * Mathf.Deg2Rad), 
                                                            Vector0ByDistance * Mathf.Sin(angle * Mathf.Deg2Rad) /*+ gravity * gravityScale / coefficient*/);


                    Vector2 maxVelocityForFixedUpdate = new Vector2(FixedTimeV0 * Mathf.Cos(angle * Mathf.Deg2Rad), 
                                                                    FixedTimeV0 * Mathf.Sin(angle * Mathf.Deg2Rad) /*+ gravity * gravityScale / coefficient*/);

                                                            

                    Vector2 availableVector = Vector2.ClampMagnitude(distanceVector, maxVelocityForFixedUpdate.magnitude);
                    availableVector         *= new Vector2(Mathf.Sign(desPos.x - curPos.x), 1);

                    availableVector         = availableVector * rgb2D.mass / deltaTime;
                    
                    rgb2D.AddForce(availableVector); 
                    
                    // Debug.Log("m_rgb2D: " +  rgb2D.velocity);
                }
            }
            else 
            {
                Debug.Log("ObliqueProjectileMotion detect Max height");
                isMaxHeigh = true;
            }
        }
    }
}

