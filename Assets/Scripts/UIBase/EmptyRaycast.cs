using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public class EmptyRaycast : MaskableGraphic
    {
        public void EnableRayCast(bool enable) {
            this.raycastTarget = enable;
        }
        protected EmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}