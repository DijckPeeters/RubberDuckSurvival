using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // Singleton patroon om de controller makkelijk bereikbaar te maken voor andere scripts.
    public static InputController instance;

    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Initialiseert het nieuwe Input System van Unity en activeert de speler-acties.
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public bool Jump()
    {
        // Leest de waarde van de 'SouthButton' (bijv. de A-knop op Xbox of X op PlayStation).
        float jumpFloat = playerInputActions.Player.SouthButton.ReadValue<float>();

        // Controleert of de knop volledig is ingedrukt (waarde 1).
        if (jumpFloat >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}