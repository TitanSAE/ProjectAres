using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MarsMessageManager : MonoBehaviour {

	public List<MarsMessage> l_messages = new List<MarsMessage>();
    public Image imMarsMessagesPanel;
    public Text tTally;
    public GameObject goMessage;
    public Color colWhiteRegular;
    private Color colBlackRegular = new Color(0, 0, 0, 255);
    public int iCurrentMessage = 0;
    private bool bIsPanelOpen;
    private Vector3 vMessagePos;
    private Vector3 vMessageSize;
    private Color colTransparent = new Color(255, 255, 255, 0);

    [Header("Add Goals here")]
    public string sTitle;
    public string sBody;

    public int iUnread {
		get {
			if (l_messages.Count == 0) {
				return 0;
			}
			else {
				int i = 0;
				foreach (MarsMessage msg in l_messages) {
					if (!msg.bRead) {
						i++;
					}
				}

				return i;
			}
		}
	}

	void Start() {
        vMessagePos = goMessage.GetComponent<RectTransform>().position;
        vMessageSize = goMessage.GetComponent<RectTransform>().localScale;

		AddMessage("Misc Objective", "Gather minerals from rocks");
	}

	void Update() {

        UpdateUI();

        if (Input.GetButtonDown("OpenMessages"))
        {
            if(bIsPanelOpen)
            {
                imMarsMessagesPanel.color = colTransparent;
                tTally.color = colTransparent;
                bIsPanelOpen = false;
                l_messages[iCurrentMessage].gameObject.SetActive(false);
            }
            else
            {
                imMarsMessagesPanel.color = colWhiteRegular;
                tTally.color = colBlackRegular;
                bIsPanelOpen = true;
                l_messages[iCurrentMessage].gameObject.SetActive(true);
                l_messages[iCurrentMessage].bRead = true;
            }
        }

        if (l_messages.Count >= 1)
        {
			if (Input.GetButtonDown("NextMessage"))
            {
                if (iCurrentMessage == (l_messages.Count - 1))
                {
                    iCurrentMessage = 0;
                    l_messages[(l_messages.Count - 1)].gameObject.SetActive(false);
                }
                else
                {
                    iCurrentMessage++;
                    l_messages[(iCurrentMessage - 1)].gameObject.SetActive(false);
                }

                if (bIsPanelOpen)
                {
                    l_messages[iCurrentMessage].gameObject.SetActive(true);
                    l_messages[iCurrentMessage].bRead = true;
                }
                else
                {
                    l_messages[iCurrentMessage].gameObject.SetActive(false);
                }
            }

			if (Input.GetButtonDown("PrevMessage"))
            {
                if (iCurrentMessage == 0)
                {
                    iCurrentMessage = (l_messages.Count - 1);
                    l_messages[0].gameObject.SetActive(false);
                }
                else
                {
                    iCurrentMessage--;
                    l_messages[(iCurrentMessage + 1)].gameObject.SetActive(false);
                }

                if (bIsPanelOpen)
                {
                    l_messages[iCurrentMessage].gameObject.SetActive(true);
                    l_messages[iCurrentMessage].bRead = true;
                }
                else
                {
                    l_messages[iCurrentMessage].gameObject.SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            string TestTitle = "Misc Objective";
            string TestBody = "Drill into surface to draw out water";
            AddMessage(TestTitle, TestBody);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            string TestTitle = "Main Objective";
            string TestBody = "Find a suitable area to place your base";
            AddMessage(TestTitle, TestBody);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            string TestTitle = "Misc Objective";
            string TestBody = "Gather minerals from rocks";
            AddMessage(TestTitle, TestBody);
        }
    }

    public void AddMessageWithoutAddingToList()
    {
        MarsMessage newMessage = Instantiate(goMessage.GetComponent<MarsMessage>()) as MarsMessage;
        newMessage.GetComponent<RectTransform>().SetParent(GameObject.Find("MessagesPanel").transform);
        newMessage.GetComponent<RectTransform>().localPosition = vMessagePos;
        newMessage.GetComponent<RectTransform>().rotation = new Quaternion(0, 0, 0, 0);
        newMessage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        newMessage.tTitle.text = sTitle;
        newMessage.tBody.text = sBody;

        if (bIsPanelOpen && l_messages.Count == 1)
        {
            newMessage.gameObject.SetActive(true);
        }
        else
        {
            newMessage.gameObject.SetActive(false);
        }

        sTitle = "";
        sBody = "";
    }

    void AddMessage(string sTitle, string sBody) {
        MarsMessage newMessage = Instantiate(goMessage.GetComponent<MarsMessage>()) as MarsMessage;
        newMessage.GetComponent<RectTransform>().SetParent(GameObject.Find("MessagesPanel").transform);
        newMessage.GetComponent<RectTransform>().localPosition = vMessagePos;
        newMessage.GetComponent<RectTransform>().rotation = new Quaternion (0, 0, 0, 0);
        newMessage.GetComponent<RectTransform>().localScale = vMessageSize;
        newMessage.tTitle.text = sTitle;
        newMessage.tBody.text = sBody;
        l_messages.Add(newMessage);

        if(bIsPanelOpen && l_messages.Count == 1)
        {
            newMessage.gameObject.SetActive(true);
        }
        else
        {
            newMessage.gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if(l_messages.Count == 0)
        {
            tTally.text = "" + (iCurrentMessage) + "/" + (l_messages.Count);
        }
        else
        {
            tTally.text = "" + (iCurrentMessage + 1) + "/" + (l_messages.Count);
        }
    }
}
