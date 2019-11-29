public interface ILevelController
{
    void CorrectOption(bool status);

    void AddLife();

    int GetScore();

    bool _enabled { get; set; }

    void Enable(bool status);

    void Restart();
}
