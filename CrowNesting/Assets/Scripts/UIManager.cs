using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text cottonText;
	public Text wireText;
	public Text branchText;

	// c‚è”‚ğ‚»‚Ì‚Ü‚Ü•\¦
	public void UpdateUI(int cotton, int wire, int branch)
	{
		cottonText.text = cotton.ToString();
		wireText.text = wire.ToString();
		branchText.text = branch.ToString();
	}
}
