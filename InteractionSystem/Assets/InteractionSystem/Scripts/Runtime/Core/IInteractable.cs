namespace InteractionSystem.Runtime.Core
{
    public interface IInteractable
    {
        void Interact();
        string InteractionPrompt { get; }
        float HoldDuration { get; }
    }
}