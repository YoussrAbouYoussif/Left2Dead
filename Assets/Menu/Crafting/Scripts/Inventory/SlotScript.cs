using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    //A a reference to the slot's icon
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Sprite loadingIcon;

    [SerializeField]
    private Text stackSize;

    [SerializeField]
    private int craftID;
    private Sprite oldIcon;

    [SerializeField]
    private bool isCraft;

    [SerializeField]
    private int maxCount;

    private int currCount = 0;

    private bool craftIsClicked;

    public BagScript MyBag { get; set; }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }
    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public int MyCraftID
    {
        get
        {
            return craftID;
        }
    }
    public bool IsClickedCraft
    {
        get
        {
            return craftIsClicked;
        }
        set
        {
            IsClickedCraft = value;
        }
    }

    private int numberItems;

    public int MyCount
    {
        get {
            numberItems = MyItems.Count;
            return numberItems; }
    }
    public Text MyStackText
    {
        get
        {
           return stackSize;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }
    public GameObject tooltip;
    public GameObject MyTooltip
    {
        get
        {
            return tooltip;
        }

        set
        {
            tooltip = value;
        }
    }

    private GameObject tooltipCraft;
    private Text tooltipTextCraft;
    private Text tooltipText;
    private Text bombText;
    private void Awake()
    {   
        tooltipCraft = GameObject.FindWithTag("GameScreen").transform.GetChild(15).gameObject;
        tooltipTextCraft = tooltipCraft.GetComponentInChildren<Text>();

        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
        switch(craftID)
                {
                case 0:
                    bombText = GameObject.FindWithTag("MolotovText").GetComponent<Text>();
                    break;
                case 1:
                    bombText = GameObject.FindWithTag("StunText").GetComponent<Text>();
                    break;
                case 3:
                    bombText = GameObject.FindWithTag("PipeText").GetComponent<Text>();
                    break;
                case 4:
                    bombText = GameObject.FindWithTag("BileText").GetComponent<Text>();
                    break;
                default:
                    break;
                }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) //If we don't have something to move
            // {
            //     if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
            //     {
            //         if (MyItem is Bag)
            //         {
            //             // InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
            //         }
            //     }
            //     else
            //     {
            //         HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
            //         InventoryScript.MyInstance.FromSlot = this;
            //     }
        
            // }
            // else
            if (InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag))
            {   //Dequips a bag from the inventory
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                //Makes sure we cant dequip it into itself and that we have enough space for the items from the dequipped bag
                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0)
                {
                    AddItem(bag);
                    // bag.MyBagButton.RemoveBag();
                    HandScript.MyInstance.Drop();
                }

            }
            // else if (InventoryScript.MyInstance.FromSlot != null)//If we have something to move
            // {
            //     //We will try to do diffrent things to place the item back into the inventory
            //     if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
            //     {
            //         HandScript.MyInstance.Drop();
            //         InventoryScript.MyInstance.FromSlot = null;
            //     }
            // }
      
        }
        // if (eventData.button == PointerEventData.InputButton.Right)//If we rightclick on the slot
        // {
        //     UseItem();
        //     if(MyItems.Count>0)
        //         MyItems.Pop();      

        // }
        bool canCraft =false;
        if(isCraft){
            if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)//If we rightclick on the slot
            {
          
                craftIsClicked =true;
                int [] x = InventoryScript.MyInstance.MyIngredientsCount;
                int alcholeCount = x[0] ;
                int ragCount = x[1];
                int sugarCount = x[2];
                int gunpowderCount = x[3] ;
                int canistersCount = x[4] ;
                int bileCount = x[5];
                oldIcon = icon.sprite;
                icon.sprite = loadingIcon;
                icon.color = Color.white;
                canCraft =false;
                switch(craftID)
                {
                case 0:
                    if(alcholeCount >1 && ragCount>1)
                    {
                        if(maxCount>currCount){

                            InventoryScript.MyInstance.RemoveIngredients("Alcohol",2);
                            InventoryScript.MyInstance.RemoveIngredients("Rag",2);
                            currCount+=1;
                            JoelScript.MyInstance.AddBombByIdByBag(1,1);
                            canCraft =true;

                        }else{

                                tooltipCraft.SetActive(true);
                                tooltipTextCraft.fontSize = 20;
                                tooltipTextCraft.text = "Reached the maximum";
                        }


                    }
                    else
                    {
                        tooltipCraft.SetActive(true);
                        tooltipTextCraft.fontSize = 20;
                        tooltipTextCraft.text = "There are no enough Ingredients";
                    }
                    break;
                case 1:
                    if(sugarCount >0 && gunpowderCount>1)
                    {   
                        if(maxCount>currCount){
                            InventoryScript.MyInstance.RemoveIngredients("Sugar",1);
                            InventoryScript.MyInstance.RemoveIngredients("Gunpowder",2);
                            currCount+=1;
                            JoelScript.MyInstance.AddBombByIdByBag(3,1);
                            canCraft =true;


                        }else{
                                tooltipCraft.SetActive(true);
                                tooltipTextCraft.fontSize = 20;
                                tooltipTextCraft.text = "Reached the maximum";
                        }

                    }
                    else
                    {
                        tooltipCraft.SetActive(true);
                        tooltipTextCraft.fontSize = 20;
                        tooltipTextCraft.text = "There is no enough Ingredients";

                    }
                    break;
                case 2:
                    if(alcholeCount >1 && ragCount>1)
                    {
                        if(maxCount>currCount){
                            InventoryScript.MyInstance.RemoveIngredients("Alcohol",2);
                            InventoryScript.MyInstance.RemoveIngredients("Rag",2);
                            currCount+=1;
                            canCraft =true;
                            IncreaseHealth();
                        }else{
                                tooltipCraft.SetActive(true);
                                tooltipTextCraft.fontSize = 20;
                                tooltipTextCraft.text = "Reached the maximum";
                        }

                    }
                    else
                    {
                        tooltipCraft.SetActive(true);
                        tooltipTextCraft.fontSize = 20;
                        tooltipTextCraft.text = "There is no enough Ingredients";

                    }
                    break;
                case 3:
                    if(alcholeCount >0 &&canistersCount>0 &&gunpowderCount>0)
                    {
                        if(maxCount>currCount){
                            InventoryScript.MyInstance.RemoveIngredients("Gunpowder",1);
                            InventoryScript.MyInstance.RemoveIngredients("Alcohol",1);
                            InventoryScript.MyInstance.RemoveIngredients("Canisters",1);
                            currCount+=1;
                            JoelScript.MyInstance.AddBombByIdByBag(2,1);
                            canCraft =true;


                        }else{
                                tooltipCraft.SetActive(true);
                                tooltipTextCraft.fontSize = 20;
                                tooltipTextCraft.text = "Reached the maximum";

                        }
                    }
                    else
                    {
                        tooltipCraft.SetActive(true);
                        tooltipTextCraft.fontSize = 20;
                        tooltipTextCraft.text = "There is no enough Ingredients";
                    }
                    break;
                case 4:
                    if(bileCount >0 &&canistersCount>0 &&gunpowderCount>0)
                    {
                        if(maxCount>currCount){
                            InventoryScript.MyInstance.RemoveIngredients("Gunpowder",1);
                            InventoryScript.MyInstance.RemoveIngredients("Bile",1);
                            InventoryScript.MyInstance.RemoveIngredients("Canisters",1);
                            currCount+=1;
                            JoelScript.MyInstance.AddBombByIdByBag(0,1);
                            canCraft =true;

                        }else{
                                tooltipCraft.SetActive(true);
                                tooltipTextCraft.fontSize = 20;
                                tooltipTextCraft.text = "Reached the maximum";

                        }
                    }
                    else
                    {

                        tooltipCraft.SetActive(true);
                        tooltipTextCraft.fontSize = 20;
                        tooltipTextCraft.text = "There is no enough Ingredients";
                    }
                    break;
                default:
                    break;
                }
                StartCoroutine(revertImage(canCraft));
            }
        }
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
      
    }
    public void IncreaseHealth(){
       Image healthBarFill =(Image)GameObject.FindGameObjectsWithTag("HealthBar")[0].GetComponent<Image>();
        healthBarFill.fillAmount += 50/300f;
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //Copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            //Clear Slot a
            from.MyItems.Clear();
            //All items from slot b and copy them into A
            from.AddItems(MyItems);

            //Clear B
            MyItems.Clear();
            //Move the items from ACopy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //How many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        if (this.MyCount > 1) //If our slot has more than one item on it
        {
            this.MyStackText.text = this.MyCount.ToString();
            this.MyStackText.color = Color.white;
            this.MyIcon.color = Color.white;
        }
        else //If it only has 1 item on it
        {
            this.MyStackText.color = new Color(0, 0, 0, 0);
            this.MyIcon.color = Color.white;
        }
        if (this.MyCount == 0) //If the slot is empty, then we need to hide the icon
        {
            this.MyIcon.color = new Color(0, 0, 0, 0);
            this.MyStackText.color = new Color(0, 0, 0, 0);
        }

    }

    private void UpdateSlotCraft()
    {
        if (this.currCount > 0) 
        {
            this.MyStackText.text =  (int.Parse(this.MyStackText.text)+1).ToString();
            this.MyStackText.color = Color.white;
            this.MyIcon.color = Color.white;
            if(bombText != null)
                this.bombText.text = this.currCount.ToString();
        }

    }

    private  IEnumerator  revertImage(bool canCraft){
        yield return new WaitForSecondsRealtime(1.5f);
        icon.sprite = oldIcon;
        icon.color = Color.white;
        tooltipTextCraft.fontSize = 35;
        tooltipCraft.SetActive(false);
        if(canCraft){
            if(craftID!=2){
                UpdateSlotCraft();
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.ShowToolitip(transform.position, MyItem);
        }   
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryScript.MyInstance.HideTooltip();
    }
}
