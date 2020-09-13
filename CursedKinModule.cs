using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;

namespace CursedKinItems
{
    public class CursedKinModule : ETGModule
    {
        public override void Init()
        {
        }

        public override void Start()
        {
            FakePrefabHooks.Init();
            ItemBuilder.Init();
            CursedBandana.Init();
        }

        public override void Exit()
        {
        }
    }
}
