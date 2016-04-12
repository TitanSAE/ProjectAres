using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarsMessageManager : MonoBehaviour {

	public List<MarsMessage> l_messages = new List<MarsMessage>();

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
	
	}

	void Update() {
	
	}
}
