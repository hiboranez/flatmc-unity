using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class ArmorContent: MonoBehaviour
    {
        public float flashTimer;
        public PlayerThread playerThread;
        public List<GameObject> armorGridList;
        public MeshRenderer helmetMeshRenderer;
        public MeshRenderer chestMeshRenderer1;
        public MeshRenderer chestMeshRenderer2;
        public MeshRenderer chestMeshRenderer3;
        public MeshRenderer leggingsMeshRenderer1;
        public MeshRenderer leggingsMeshRenderer2;
        public MeshRenderer leggingsMeshRenderer3;
        public MeshRenderer bootsMeshRenderer1;
        public MeshRenderer bootsMeshRenderer2;
        public MeshRenderer modelHelmetMeshRenderer;
        public MeshRenderer modelChestMeshRenderer1;
        public MeshRenderer modelChestMeshRenderer2;
        public MeshRenderer modelChestMeshRenderer3;
        public MeshRenderer modelLeggingsMeshRenderer1;
        public MeshRenderer modelLeggingsMeshRenderer2;
        public MeshRenderer modelLeggingsMeshRenderer3;
        public MeshRenderer modelBootsMeshRenderer1;
        public MeshRenderer modelBootsMeshRenderer2;
        public GameObject model;

        private void Start()
        {
            armorGridList = new List<GameObject>();
        }

        private void OnEnable()
        {
            model.SetActive(true);
        }

        private void OnDisable()
        {
            model.SetActive(false);
        }

        public void UpdateArmorModel()
        {
            Material iron1 = Resources.Load<Material>("Materials/Armor Iron");
            Material iron2 = Resources.Load<Material>("Materials/Armor Iron2");
            Material gold1 = Resources.Load<Material>("Materials/Armor Gold");
            Material gold2 = Resources.Load<Material>("Materials/Armor Gold2");
            Material diamond1 = Resources.Load<Material>("Materials/Armor Diamond");
            Material diamond2 = Resources.Load<Material>("Materials/Armor Diamond2");
            
            if (playerThread.armorHelmet.Equals("null")) {
                helmetMeshRenderer.gameObject.SetActive(false);
                modelHelmetMeshRenderer.gameObject.SetActive(false);
            }else {
                if (playerThread.armorHelmet.Contains("Iron")) {
                    helmetMeshRenderer.material = iron1;
                    modelHelmetMeshRenderer.material = iron1;
                    
                }else if (playerThread.armorHelmet.Contains("Gold"))
                {
                    helmetMeshRenderer.material = gold1;
                    modelHelmetMeshRenderer.material = gold1;
                }
                else if (playerThread.armorHelmet.Contains("Diamond"))
                {
                    helmetMeshRenderer.material = diamond1;
                    modelHelmetMeshRenderer.material = diamond1;
                }
                helmetMeshRenderer.gameObject.SetActive(true);
                modelHelmetMeshRenderer.gameObject.SetActive(true);
            }
            
            if (playerThread.armorChest.Equals("null")) {
                chestMeshRenderer1.gameObject.SetActive(false);
                chestMeshRenderer2.gameObject.SetActive(false);
                chestMeshRenderer3.gameObject.SetActive(false);
                modelChestMeshRenderer1.gameObject.SetActive(false);
                modelChestMeshRenderer2.gameObject.SetActive(false);
                modelChestMeshRenderer3.gameObject.SetActive(false);
            }else {
                if (playerThread.armorChest.Contains("Iron")) {
                    chestMeshRenderer1.material = iron1;
                    chestMeshRenderer2.material = iron1;
                    chestMeshRenderer3.material = iron1;
                    modelChestMeshRenderer1.material = iron1;
                    modelChestMeshRenderer2.material = iron1;
                    modelChestMeshRenderer3.material = iron1;
                }else if (playerThread.armorChest.Contains("Gold"))
                {
                    chestMeshRenderer1.material = gold1;
                    chestMeshRenderer2.material = gold1;
                    chestMeshRenderer3.material = gold1;
                    modelChestMeshRenderer1.material = gold1;
                    modelChestMeshRenderer2.material = gold1;
                    modelChestMeshRenderer3.material = gold1;
                }
                else if (playerThread.armorChest.Contains("Diamond"))
                {
                    chestMeshRenderer1.material = diamond1;
                    chestMeshRenderer2.material = diamond1;
                    chestMeshRenderer3.material = diamond1;
                    modelChestMeshRenderer1.material = diamond1;
                    modelChestMeshRenderer2.material = diamond1;
                    modelChestMeshRenderer3.material = diamond1;
                }
                chestMeshRenderer1.gameObject.SetActive(true);
                chestMeshRenderer2.gameObject.SetActive(true);
                chestMeshRenderer3.gameObject.SetActive(true);
                modelChestMeshRenderer1.gameObject.SetActive(true);
                modelChestMeshRenderer2.gameObject.SetActive(true);
                modelChestMeshRenderer3.gameObject.SetActive(true);
            }
            
            if (playerThread.armorLeggings.Equals("null")) {
                leggingsMeshRenderer1.gameObject.SetActive(false);
                leggingsMeshRenderer2.gameObject.SetActive(false);
                leggingsMeshRenderer3.gameObject.SetActive(false);
                modelLeggingsMeshRenderer1.gameObject.SetActive(false);
                modelLeggingsMeshRenderer2.gameObject.SetActive(false);
                modelLeggingsMeshRenderer3.gameObject.SetActive(false);
            }else {
                if (playerThread.armorLeggings.Contains("Iron")) {
                    leggingsMeshRenderer1.material = iron2;
                    leggingsMeshRenderer2.material = iron2;
                    leggingsMeshRenderer3.material = iron2;
                    modelLeggingsMeshRenderer1.material = iron2;
                    modelLeggingsMeshRenderer2.material = iron2;
                    modelLeggingsMeshRenderer3.material = iron2;
                }else if (playerThread.armorLeggings.Contains("Gold"))
                {
                    leggingsMeshRenderer1.material = gold2;
                    leggingsMeshRenderer2.material = gold2;
                    leggingsMeshRenderer3.material = gold2;
                    modelLeggingsMeshRenderer1.material = gold2;
                    modelLeggingsMeshRenderer2.material = gold2;
                    modelLeggingsMeshRenderer3.material = gold2;
                }
                else if (playerThread.armorLeggings.Contains("Diamond"))
                {
                    leggingsMeshRenderer1.material = diamond2;
                    leggingsMeshRenderer2.material = diamond2;
                    leggingsMeshRenderer3.material = diamond2;
                    modelLeggingsMeshRenderer1.material = diamond2;
                    modelLeggingsMeshRenderer2.material = diamond2;
                    modelLeggingsMeshRenderer3.material = diamond2;
                }
                leggingsMeshRenderer1.gameObject.SetActive(true);
                leggingsMeshRenderer2.gameObject.SetActive(true);
                leggingsMeshRenderer3.gameObject.SetActive(true);
                modelLeggingsMeshRenderer1.gameObject.SetActive(true);
                modelLeggingsMeshRenderer2.gameObject.SetActive(true);
                modelLeggingsMeshRenderer3.gameObject.SetActive(true);
            }
            
            if (playerThread.armorBoots.Equals("null")) {
                bootsMeshRenderer1.gameObject.SetActive(false);
                bootsMeshRenderer2.gameObject.SetActive(false);
                modelBootsMeshRenderer1.gameObject.SetActive(false);
                modelBootsMeshRenderer2.gameObject.SetActive(false);
            }else {
                if (playerThread.armorBoots.Contains("Iron")) {
                    bootsMeshRenderer1.material = iron1;
                    bootsMeshRenderer2.material = iron1;
                    modelBootsMeshRenderer1.material = iron1;
                    modelBootsMeshRenderer2.material = iron1;
                }else if (playerThread.armorBoots.Contains("Gold"))
                {
                    bootsMeshRenderer1.material = gold1;
                    bootsMeshRenderer2.material = gold1;
                    modelBootsMeshRenderer1.material = gold1;
                    modelBootsMeshRenderer2.material = gold1;
                }
                else if (playerThread.armorBoots.Contains("Diamond"))
                {
                    bootsMeshRenderer1.material = diamond1;
                    bootsMeshRenderer2.material = diamond1;
                    modelBootsMeshRenderer1.material = diamond1;
                    modelBootsMeshRenderer2.material = diamond1;
                }
                bootsMeshRenderer1.gameObject.SetActive(true);
                bootsMeshRenderer2.gameObject.SetActive(true);
                modelBootsMeshRenderer1.gameObject.SetActive(true);
                modelBootsMeshRenderer2.gameObject.SetActive(true);
            }
        }
        
        public void StartFlash(Image whiteBackImage)
        {
            StartCoroutine(Flash(whiteBackImage));
        }
        
        public IEnumerator Flash(Image whiteBackImage) {
            flashTimer = 0.15f;
            while (flashTimer > 0)
            {
                if (flashTimer > 0.12f && flashTimer <= 0.16f) {
                    whiteBackImage.enabled = true;
                }else if (flashTimer > 0.08f && flashTimer <= 0.12f) {
                    whiteBackImage.enabled = false;
                }else if (flashTimer > 0.04f && flashTimer <= 0.08f) {
                    whiteBackImage.enabled = true;
                }else if (flashTimer > 0f && flashTimer <= 0.04f) {
                    whiteBackImage.enabled = false;
                }
                // 等待一段时间，例如0.1秒
                yield return new WaitForSeconds(Time.deltaTime);
                // 逐步减小flashTimer的值
                flashTimer -= Time.deltaTime;
            }
        }
    }
}