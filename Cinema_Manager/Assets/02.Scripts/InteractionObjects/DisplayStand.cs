using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class DisplayStand : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _foodStack;
    private int _currentFoodCnt => _foodStack.Count;
    public int StackMaxCnt => _spawnTrmList.Count * _columnSpawnCnt;

    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private int _columnSpawnCnt;
    [SerializeField] private List<Transform> _spawnTrmList = new List<Transform>();
    [Range(0, 5)] [SerializeField] private float _spacingX;

    private bool _isEnterInteraction = false;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>(); // ³ªÁß¿¡ ½Ì±ÛÅæÀ¸·Î
        _foodStack = new Stack<ITakeable>();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        StartCoroutine(GetFoodRoutine());
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
    }

    private IEnumerator GetFoodRoutine()
    {
        while (_isEnterInteraction)
        {
            if (_playerController.CanGiveFood(_poolObjType) && _foodStack.Count < StackMaxCnt)
            {
                ITakeable food = _playerController.OnGiveFood?.Invoke();
                TakeFood(food);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void TakeFood(ITakeable food)
    {
        int x = _currentFoodCnt % _columnSpawnCnt;
        int y = _currentFoodCnt / _columnSpawnCnt;
        Debug.Log(x + " " + y);
        
        Vector3 foodPos = _spawnTrmList[y].position;

        foodPos.x = _spawnTrmList[y].position.x + (_spacingX * x);
        foodPos.y = _spawnTrmList[y].position.y;
        Debug.Log(foodPos);

        food.Take(_spawnTrmList[y].parent, foodPos, Vector3.zero);
        Debug.Log("-----------------");
        _foodStack.Push(food);
    }
}
