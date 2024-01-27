using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour 
{
    public ISpawnerRecipe recipe;
    // if -1, no limit to spawning. Configure at your own risk!
    public int max_spawn_count = 3;
    public float spawn_cooldown = 2;

    private float remaining_cooldown = 0;
    private List<GameObject> spawned_objects = new List<GameObject>();

    protected GameObject playerRef;
    protected GameObject mainCameraRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindWithTag("Player");
        mainCameraRef = GameObject.FindWithTag("MainCamera");
        recipe = GetComponent<ISpawnerRecipe>();
    }

    // Update is called once per frame
    void Update()
    {
        if (remaining_cooldown > 0)
        {
            remaining_cooldown -= Time.deltaTime;
        }
        else if (CanSpawn())
        {
            Spawn();
        }      
    }

    protected virtual bool CanSpawn()
    {
        return (max_spawn_count == -1 || (max_spawn_count > -1 && spawned_objects.Count < max_spawn_count))
            && remaining_cooldown <= 0;
    }

    private void Spawn()
    {
        var newlySpawnedObj = Instantiate(recipe.objectToSpawn, transform.position, Quaternion.identity);
        recipe.InitializeSpawnableComponent(newlySpawnedObj);
        spawned_objects.Add(newlySpawnedObj);
        remaining_cooldown += spawn_cooldown;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) { 
            if (CanSpawn())
            {
                Gizmos.color = Color.green;
            }
            else if (remaining_cooldown >= 0)
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.gray;
            }
        } else
        {
            Gizmos.color = Color.gray;
        }
        Gizmos.DrawSphere(transform.position, .25f);
        ChildDrawGizmos();
    }

    protected virtual void ChildDrawGizmos() {}
}