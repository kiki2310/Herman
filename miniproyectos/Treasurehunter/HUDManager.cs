using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text hpText;
    public TMP_Text energyText;
    public TMP_Text treasureText;
    public TMP_Text multiplierText;
    public TMP_Text pastiText;
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text levelText;    
    public TMP_Text keyText;   // ← NUEVO

    [Header("Font Materials")]
    public Material normalMaterial;
    public Material goldMaterial;

    [Header("Gold Flash")]
    public float goldDuration = 1.5f;
    private float goldUntil = 0f;

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null) return;

        // Textos base (lo que ya tenías)
        hpText.text = $"Fuerza de voluntad: {gm.HP}";
        energyText.text = $"Energía: {gm.Energy}/{gm.MaxEnergy}";
        treasureText.text = $"Tesoros: {gm.TreasuresCollected}/5";

        int mul = gm.TreasuresCollected switch { 0 => 1, 1 => 2, 2 => 4, 3 => 5, 4 => 6, _ => 7 };
        multiplierText.text = $"x{mul}";

        pastiText.text = $"Pastis: {gm.Pasti}";
        scoreText.text = $"Score: {gm.Score:n0}";

        int t = Mathf.CeilToInt(gm.TimeLeft);
        if (t < 0) t = 0;
        int m = t / 60, s = t % 60;
        timeText.text = $"{m:00}:{s:00}";

        levelText.text = $"Level: {gm.LevelIndex + 1}/5";
        int curLevel = gm.LevelIndex;
    if (gm.grid != null) // si GameManager expone su GridManager
    {
        curLevel = gm.grid.currentLevel; // preferimos el nivel real del grid si existe
    }
    if (keyText)
    {
        bool hasKey = gm.IsKeyCollected(curLevel);
        keyText.text = hasKey ? "Llave: X" : "Llave: -";
    }

        // Material activo (normal vs dorado)
        ApplyMaterial(Time.time > goldUntil ? normalMaterial : goldMaterial);
    }

    public void FlashGold(float duration = -1f)
    {
        if (duration <= 0f) duration = goldDuration;
        goldUntil = Time.time + duration;
        ApplyMaterial(goldMaterial);
    }

    private void ApplyMaterial(Material mat)
    {
        if (!mat) return;
        if (hpText) hpText.fontSharedMaterial = mat;
        if (energyText) energyText.fontSharedMaterial = mat;
        if (treasureText) treasureText.fontSharedMaterial = mat;
        if (multiplierText) multiplierText.fontSharedMaterial = mat;
        if (pastiText) pastiText.fontSharedMaterial = mat;
        if (scoreText) scoreText.fontSharedMaterial = mat;
        if (timeText) timeText.fontSharedMaterial = mat;
        if (levelText) levelText.fontSharedMaterial = mat;
        if (keyText) keyText.fontSharedMaterial = mat; // ← NUEVO
    }
}