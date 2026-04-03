using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FIKIFOW_Image_Shuffle_Grid : MonoBehaviour
{
    // List untuk menyimpan urutan asli tombol saat game baru mulai
    private List<Transform> originalOrder = new List<Transform>();

    void Awake()
    {
        // Simpan urutan asli segera setelah game dijalankan
        SaveOriginalOrder();
    }

    private void SaveOriginalOrder()
    {
        originalOrder.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            originalOrder.Add(transform.GetChild(i));
        }
    }

    // Fungsi untuk mengacak posisi (Shuffle)
    public void StartRandomizer()
    {
        ShuffleButtons();
    }

    // Fungsi baru untuk mengembalikan ke urutan semula (Reset)
    public void ResetRandomizer()
    {
        if (originalOrder.Count == 0) return;

        // Kembalikan urutan berdasarkan list originalOrder yang disimpan tadi
        for (int i = 0; i < originalOrder.Count; i++)
        {
            originalOrder[i].SetAsLastSibling();
        }
    }

    private void ShuffleButtons()
    {
        int childCount = transform.childCount;
        List<Transform> children = new List<Transform>();

        for (int i = 0; i < childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }

        // Algoritma Fisher-Yates untuk mengacak
        for (int i = 0; i < children.Count; i++)
        {
            Transform temp = children[i];
            int randomIndex = Random.Range(i, children.Count);
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // Terapkan hasil acakan ke layar
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetAsLastSibling();
        }
    }
}