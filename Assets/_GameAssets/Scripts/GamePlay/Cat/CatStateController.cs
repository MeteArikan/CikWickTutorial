using UnityEngine;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private CatState _currentCatState = CatState.Walking;

    private void Start() {
        _currentCatState = CatState.Walking;
    }

    public void ChangeState(CatState newState)
    {
        if (_currentCatState == newState) return;
        _currentCatState = newState;
    }
    public CatState GetCatState()
    {
        return _currentCatState;
    }
}
