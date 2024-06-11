using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Inventory inventory;

    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;

    private void Awake()
    {
        controls = new Controls();
        UpDown();
        //Up and down
    }
    private void UpDown()
    {
        Ladder lad = GameManager.Get.GetLadderAtLocation(this.transform.position);
        if(lad != null )
        {
            if (lad.Up == true)
            {
                MapManager.Get.MoveUp();
            }
            if (lad.Up == false)
            {
                MapManager.Get.MoveDown();
            }
        }
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();
        inventory.MaxItems = 8;
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        if (context.performed)
        {
            if(inventoryIsOpen == true && direction.y > 0)
            {
                UIManager.Get.Inventory.SelectNextItem();
            }
            if(inventoryIsOpen == true && direction.y < 0)
            {
                UIManager.Get.Inventory.SelectPreviousItem();
            }
            Move();
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen == true)
            {
                UIManager.Get.Inventory.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Consumable item = GameManager.Get.GetItemAtLocation(gameObject.transform.position);
            if(item == null)
            {
                UIManager.Get.AddMessage("There's no item here, dummy", Color.cyan);
            }
            else if (inventory.Items.Count >= inventory.MaxItems)
            {
                UIManager.Get.AddMessage("Your inventory isn't infinite, idiot", Color.cyan);
            }
            else
            {
                inventory.AddItem(item);
                item.gameObject.SetActive(false);
                GameManager.Get.RemoveItem(item);
                UIManager.Get.AddMessage("You picked up an item... fuck that", Color.cyan);
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(inventoryIsOpen == false)
            {
                UIManager.Get.Inventory.Show(inventory.Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen == true)
            {
                Consumable item = inventory.Items[UIManager.Get.Inventory.Selected];
                inventory.DropItem(item);
                if(Input.GetKeyDown(KeyCode.D))
                {
                    item.gameObject.transform.position = gameObject.transform.position;
                    GameManager.Get.AddItem(item);
                    item.gameObject.active = true;
                    UIManager.Get.Inventory.Hide();
                    inventoryIsOpen = false;
                    droppingItem = false;
                    usingItem = false;
                }
                else if (Input.GetKeyDown(KeyCode.U))
                {
                    UseItem(item);
                    Destroy(item.gameObject);
                    UIManager.Get.Inventory.Hide();
                    inventoryIsOpen = false;
                    droppingItem = false;
                    usingItem = false;
                }
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen == false)
            {
                UIManager.Get.Inventory.Show(inventory.Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    private void UseItem(Consumable item)
    {
        Consumable Item = inventory.Items[UIManager.Get.Inventory.Selected];
        if(Item.name == "HealthPotion")
        {
            gameObject.GetComponent<Actor>().Heal(5);
        }
        if (Item.name == "Fireball")
        {
            List<Actor> enemies = GameManager.Get.GetNearbyEnemies(transform.position);
            foreach(Actor enemy in enemies)
            {
                enemy.DoDamage(10, GetComponent<Actor>());
            }
            UIManager.Get.AddMessage("I HOPE YOU DIE IN A FIRE", Color.green);
        }
        if (Item.name == "ScrollOfConfusion")
        {
            List<Actor> enemies = GameManager.Get.GetNearbyEnemies(transform.position);
            foreach (Actor enemy in enemies)
            {
                enemy.GetComponent<Enemy>().Confuse();
            }
            UIManager.Get.AddMessage("CONFUSION!!!!", Color.green);
        }
    }
}
