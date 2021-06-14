using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public delegate void ItemCountChanged(Item item);

    public class InventoryScript : MonoBehaviour
    {
        public event ItemCountChanged itemCountChangedEvent;

        private static InventoryScript instance;

        public static InventoryScript MyInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<InventoryScript>();
                }

                return instance;
            }
        }
        
        private int[] ingredientsCount = { 0, 0, 0, 0, 0, 0}; 
        public int[] MyIngredientsCount
        {
            get 
            {   
                Decription();
                return ingredientsCount; 
            }
        }
        private SlotScript fromSlot;

        private List<Bag> bags = new List<Bag>();

        [SerializeField]
        private BagButton[] bagButtons;


        //For debugging
        [SerializeField]
        private Item[] items;

        [SerializeField]
        private GameObject tooltip;
        private Text tooltipText;

        public bool CanAddBag
        {
            get { return bags.Count < 1; }
        }

        public int MyEmptySlotCount
        {
            get
            {
                int count = 0;
                count += bags[0].MyBagScript.MyEmptySlotCount;

                return count;
            }
        }

        public int MyTotalSlotCount
        {
            get
            {
                int count = 0;
                count += bags[0].MyBagScript.MySlots.Count;
                return count;
            }
        }

        public int MyFullSlotCount
        {
            get
            {
                return MyTotalSlotCount - MyEmptySlotCount;
            }
        }

        public SlotScript FromSlot
        {
            get
            {
                return fromSlot;
            }

            set
            {
                fromSlot = value;

                if (value != null)
                {
                    fromSlot.MyIcon.color = Color.grey;
                }
            }
        }

        private void Awake()
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(30);
            bag.Use();
            bag.MyBagScript.OpenClose();
            tooltip = Instantiate(tooltip, gameObject.transform);
            tooltipText = tooltip.GetComponentInChildren<Text>();
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.L))
            // {
            //     HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            //     AddItem(potion);
            // }
            //  if (Input.GetKeyDown(KeyCode.U))
            // {
            //     Ingredients alcohol = (Ingredients)Instantiate(items[2]);
            //     AddItem(alcohol);
            // }
            // //    if (Input.GetKeyDown(KeyCode.W))
            // // {
            // //     Ingredients canisters = (Ingredients)Instantiate(items[5]);
            // //     AddItem(canisters);
            // // }
            // //    if (Input.GetKeyDown(KeyCode.E))
            // // {
            //     Ingredients gunpowder = (Ingredients)Instantiate(items[4]);
            //     AddItem(gunpowder);
            // }
               
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     Ingredients rag = (Ingredients)Instantiate(items[3]);
            //     AddItem(rag);
            //     Ingredients alcohol = (Ingredients)Instantiate(items[2]);
            //     AddItem(alcohol);
            //     Ingredients sugar = (Ingredients)Instantiate(items[6]);
            //     AddItem(sugar);
            //     Ingredients gunpowder = (Ingredients)Instantiate(items[4]);
            //     AddItem(gunpowder);

            //     Ingredients canisters = (Ingredients)Instantiate(items[5]);
            //     AddItem(canisters);
  
            // }
            //    if (Input.GetKeyDown(KeyCode.T))
            // {
            //     Ingredients sugar = (Ingredients)Instantiate(items[6]);
            //     AddItem(sugar);
            // }
           

        }

        /// <summary>
        /// Equips a bag to the inventory
        /// </summary>
        /// <param name="bag"></param>
        public void AddBag(Bag bag)
        {
            foreach (BagButton bagButton in bagButtons)
            {
                if (bagButton.MyBag == null)
                {
                    bagButton.MyBag = bag;
                    bags.Add(bag);
                    bag.MyBagButton = bagButton;
                    break;
                }
            }
        }

        public void AddBag(Bag bag, BagButton bagButton)
        {
            bags.Add(bag);
            bagButton.MyBag = bag;
        }

        /// <summary>
        /// Removes the bag from the inventory
        /// </summary>
        /// <param name="bag"></param>
        public void RemoveBag(Bag bag)
        {
            bags.Remove(bag);
            Destroy(bag.MyBagScript.gameObject);
        }


        /// <summary>
        /// Adds an item to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        public void AddItem(Item item)
        {
            if (item.MyStackSize > 0)
            {
                if (PlaceInStack(item))
                {
                    return;
                }
            }

            PlaceInEmpty(item);
        }

        /// <summary>
        /// Places an item on an empty slot in the game
        /// </summary>
        /// <param name="item">Item we are trying to add</param>
        private void PlaceInEmpty(Item item)
        {
            
                if (bags[0].MyBagScript.AddItem(item)) //Tries to add the item
                {
                    OnItemCountChanged(item);
                    return; //It was possible to add the item
                }
            //  we should return here if it's posibale or not 
        }
        public bool RemoveIngredients(string title,int count){
            int remaining = count;
            foreach (SlotScript slots in bags[0].MyBagScript.MySlots) //Checks all the slots on the current bag
                {

                    while(slots.MyCount>0 && remaining > 0 && slots.MyItems.Peek().MyTitel == title)
                    {
                        slots.MyItems.Pop();
                        remaining -=1;
                    } 
                    if(remaining == 0)
                        return true;       

                }
                return false;
            }
         
        private void Decription()
        {

                ingredientsCount[0] =0;
                ingredientsCount[1] =0;
                ingredientsCount[2] =0;
                ingredientsCount[3] =0;
                ingredientsCount[4] =0; 
                ingredientsCount[5] =0;         
                foreach (SlotScript slots in bags[0].MyBagScript.MySlots) //Checks all the slots on the current bag
                {

                    if(slots.MyCount>0){
                    switch (slots.MyItems.Peek().MyTitel)
                    {
                        case "Alcohol":
                            ingredientsCount[0] +=slots.MyCount;
                            break;
                        case "Rag":
                            ingredientsCount[1] +=slots.MyCount;
                            break;
                        case "Sugar":
                            ingredientsCount[2] +=slots.MyCount;
                            break;
                        case "Gunpowder":
                            ingredientsCount[3] +=slots.MyCount;
                            break;
                        case "Canisters":
                            ingredientsCount[4] +=slots.MyCount;
                            break;
                        case "Bile":
                            ingredientsCount[5] +=slots.MyCount;
                            break;
                        default:
                            break;
                    }                       

                }
            }
            // return ingredientsCount;
        }

        /// <summary>
        /// Tries to stack an item on anothe
        /// </summary>
        /// <param name="item">Item we try to stack</param>
        /// <returns></returns>
        private bool PlaceInStack(Item item)
        {
                foreach (SlotScript slots in bags[0].MyBagScript.MySlots) //Checks all the slots on the current bag
                {
                    if (slots.StackItem(item)) //Tries to stack the item
                    {
                        OnItemCountChanged(item);
                        return true; //It was possible to stack the item
                    }
                }

            return false; 
        }

        public void OpenClose()
        {
            //Checks if any bags are closed
            bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);

                if (bags[0].MyBagScript.IsOpen != closedBag)
                {
                    bags[0].MyBagScript.OpenClose();
                }
        }

        public Stack<IUseable> GetUseables(IUseable type)
        {
            Stack<IUseable> useables = new Stack<IUseable>();
                foreach (SlotScript slot in bags[0].MyBagScript.MySlots)
                {
                    if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                    {
                        foreach (Item item in slot.MyItems)
                        {
                            useables.Push(item as IUseable);
                        }
                    }
                }

            return useables;
        }

        public void OnItemCountChanged(Item item)
        {
            if (itemCountChangedEvent != null)
            {
                itemCountChangedEvent.Invoke(item);
            }
        }
    /// <summary>
    /// Shows the tooltip
    /// </summary>
    public void ShowToolitip(Vector3 position, IDescribable description)
    {
        tooltip.SetActive(true);
        tooltip.transform.position = new Vector3(position.x, position.y+80.0f, position.z);
        tooltipText.text = description.GetDescription();
    }

    /// <summary>
    /// Hides the tooltip
    /// </summary>
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }


}
