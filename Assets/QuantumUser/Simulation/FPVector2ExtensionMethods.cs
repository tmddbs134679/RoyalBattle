using Photon.Deterministic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum
{
    public static class FPVector2ExtensionMethods 
    {
       public static FPVector2 Rotate(this FPVector2 vector, FP angle)
       {
            FP cos = FPMath.Cos(angle);
            FP sin = FPMath.Sin(angle);

            return new FPVector2(vector.X * cos - vector.Y * sin, vector.X * vector.Y * cos);
       }
    }
}
