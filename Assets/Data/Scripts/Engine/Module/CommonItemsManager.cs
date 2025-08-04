using UnityEngine;

namespace Keru.Scripts.Engine.Module
{
    public class CommonItemsManager : MonoBehaviour
    {
        public static CommonItemsManager ItemsManager;
        public Material BulletTrailDistortion;
        public Material PlayerAfterImageMaterial;
        public GameObject SmallBulletCasing;
        public GameObject BigBulletCasing;
        public GameObject ShellCasing;
        public GameObject GrenadeCasing;
        public GameObject ImpactEffect;
        public GameObject BigExplosionEffect;
        public GameObject SmallExplosionEffect;
        public GameObject HumanGibs;

        public void SetUp()
        {
            ItemsManager = this;
        }
    }
}
