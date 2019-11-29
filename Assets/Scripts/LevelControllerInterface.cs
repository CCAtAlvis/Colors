using UnityEngine;

public interface ILevelController
{
    void CorrectOption();

    void IncorrectOption();

    void AddLife();

    int GetScore();

    bool _enabled { get; set; }

    void Enable();

    void Disable();
}
