//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemySpawner : Spawner
//{
//    public override void SetSpawningInterval(float _duration)
//    {
        
//    }

//    public override IEnumerator SpawningRoutine()
//    {
//        while(true)
//        {
//            if (repeatSpawning)
//                SetSpawningInterval(repeatDuration);
//            else
//                SpawnObj();

//            yield return new WaitForEndOfFrame();
//        }
//    }

//    public override void SpawnObj(bool _onlyOnce = true)
//    {
//        if (ableToSpawn && (objectToSpawn is ISpawnable))
//        {
//            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
//            objectToSpawn.OnSpawn();
//            FindTotalObjectsSpawned();
//            if (_onlyOnce) ableToSpawn = false;
//        }
//    }

//    public override int FindTotalObjectsSpawned() => FindObjectsOfType<CapsuleTargetPawn>().Length;
//}
