using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour {
	public Sprite[] puzzles;
	public TextAsset[] descriptions;

	public GameObject descriptionWindow;
	public Image descriptionImage;
	public Text descriptionText;

	public Piece selectedPiece;
	public Slot selectedSlot;

	public Text scoreLabel;
	public Text completeLabel;

	public Canvas canvasRef;
	public Transform piecesParent;
	public Transform slotParent;

	public float scoreMultiplier;

	private int selectedPuzzle;
	private int completedPieces;
	private int score;

	void Start() {
		completeLabel.text = "";
		selectedPuzzle = Random.Range(0, puzzles.Length);

		foreach (Transform piece in piecesParent)
			piece.GetComponent<RawImage>().texture = puzzles[selectedPuzzle].texture;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape))
			ReturnToMenu();

		if (Input.GetKeyDown(KeyCode.C))
			GameComplete();

		if (Input.GetMouseButtonDown(0)) {
			// Esconde ou mostra os slots no momento necessário
			slotParent.gameObject.SetActive(true);

			if (selectedPiece != null)
				selectedPiece.selected = true;
		}

		if (Input.GetMouseButtonUp(0)) {
			CheckPieceSlot(selectedPiece, selectedSlot);

			if (selectedPiece != null) {
				selectedPiece.Unselect();
				selectedPiece = null;
			}

			slotParent.gameObject.SetActive(false);
			selectedSlot = null;
		}
	}

	public void CheckPieceSlot(Piece piece, Slot slot) {
		if (piece == null || slot == null)
			return;

		if (piece.id == slot.id)
			MatchToSlot(slot);
	}

	public void MatchToSlot(Slot slot) {
		selectedPiece.Lock(slot.rect.position);
		selectedPiece = null;

		// Calcula a pontuação (Função logarítimca decrescente)
		var log = Mathf.Log(5 + Time.timeSinceLevelLoad, 0.1f);
		score += (int)Mathf.Ceil((10 + log) * scoreMultiplier);
		scoreLabel.text = score.ToString("00000");

		completedPieces++;

		if (completedPieces >= piecesParent.childCount)
			GameComplete();
	}

	public void GameComplete() {
		descriptionImage.sprite = puzzles[selectedPuzzle];
		descriptionText.text = descriptions[selectedPuzzle].text;
		descriptionWindow.SetActive(true);
		completeLabel.text = "Concluído!";
	}

	public void OpenSite() {
		Application.OpenURL("http://vivendoparque.azurewebsites.net/");
	}

	public void ReturnToMenu() {
		SceneManager.LoadScene(0);
	}
}
