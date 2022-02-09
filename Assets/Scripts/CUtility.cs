using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

    public static class CUtility 
    {
        private static CSerializedDictionary<int, Random> RandomStack;

        public static void Initalize()
        {
            RandomStack = new CSerializedDictionary<int, Random> ();
        }
        public static int RandomInt(this MonoBehaviour P_MBObject, int P_InclusiveMin, int P_InclusiveMax)
        {
            int instanceID = P_MBObject.GetInstanceID();

            if(RandomStack.ContainsKey(instanceID))
            {
                RandomStack[instanceID] = null; // Freeing the previous random seed.
                RandomStack[instanceID] = new Random();
                return RandomStack[instanceID].Next(P_InclusiveMin, P_InclusiveMax + 1);
            } 

            Random random = new Random();
            RandomStack.Add(instanceID, random);
            return random.Next(P_InclusiveMin, P_InclusiveMax + 1);
        }
        public static float RandomFloat(this MonoBehaviour P_MBObject, float P_Min, float P_Max)
        {
            double sample = double.MaxValue;
            int instanceID = P_MBObject.GetInstanceID();
            if(RandomStack.ContainsKey(instanceID))
            {
                RandomStack[instanceID] = null; // Freeing the previous random seed.
                RandomStack[instanceID] = new Random();
            } 
            else RandomStack.Add(instanceID, new Random());
            
            sample = RandomStack[instanceID].NextDouble();
            return (float)(sample * P_Max + P_Min);
        }
        public static Vector3 RandomVector3(this MonoBehaviour P_MBObject, Vector3 P_Min, Vector3 P_Max)
        {
            float x = RandomFloat(P_MBObject, P_Min.x, P_Max.x); 
            float y = RandomFloat(P_MBObject, P_Min.y, P_Max.y); 
            float z = RandomFloat(P_MBObject, P_Min.z, P_Max.z); 

            return new Vector3(x, y, z);
        }
    }
    
