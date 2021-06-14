using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// A reference to the bag item
    /// </summary>
    /// 
/*    BagButton.MyInstance.closeIFOpen(false);
*/
    private static BagButton instance;

    public static BagButton MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BagButton>();
            }

            return instance;
        }
    }
    private Bag bag;
    [SerializeField]
    private GameObject itemsTOCraft;
    [SerializeField]
    private GameObject ButtonBar;
    [SerializeField]
    private GameObject pauseMenu;

    public static bool craftIsOpen;
    /// <summary>
    /// A property for accessing the specific bag
    /// </summary>
    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            bag = value;
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T) && !pauseMenu.activeInHierarchy)
        {
/*            craftIsOpen = !craftIsOpen;
*/            closeIFOpen();

        }
    }
    public void closeIFOpen()
    {
        if (bag != null)//If we have a bag equipped
        {
            //Open or close the bag
            itemsTOCraft.SetActive(!itemsTOCraft.activeInHierarchy);
            ButtonBar.SetActive(!ButtonBar.activeInHierarchy);
            bag.MyBagScript.OpenClose();
            if (ButtonBar.activeInHierarchy)
            {

                    Cursor.lockState = CursorLockMode.None;
                    GameMenu.pause = true;
                    Time.timeScale = 0;
                

            }
            else
            {

                    GameMenu.pause = false;
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                
                
            }
        }
    }
    /// <summary>
    /// if we click the specific bag button
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {

            if (bag != null)//If we have a bag equipped
            {
                //Open or close the bag
                itemsTOCraft.SetActive(!itemsTOCraft.activeInHierarchy);
                ButtonBar.SetActive(!ButtonBar.activeInHierarchy);
                bag.MyBagScript.OpenClose();
            }

        }


    }

}
