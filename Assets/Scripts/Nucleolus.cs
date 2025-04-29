using UnityEngine;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Collections;


public class Nucleolus  
{

    public float ribosomeProductionRate = 1f;
    private Coroutine _delayRoutine;
    

    public void ProduceRibosomes(int amount, Ribosome ribosomeTemplate, Transform[] positions, Nucleus nRef)
    {
        for(int i=0; i < amount; i++)
        {
            Ribosome r = GameObject.Instantiate(ribosomeTemplate);
            int randomIndex = Random.Range(0, positions.Length);
            r.transform.position = nRef.transform.position;
            r.transform.DOMove(positions[randomIndex].position,1);
            r.transform.SetParent(positions[randomIndex].parent);
        }
    }

    /*
     * First, RNA has to be made. Then they have to slide through the ribosomes. Then they produce the slicers, which glide to random spots around the cell and wait. 
     * 
     * */
    internal void ProduceSlicers(int amount, SlicerEnzyme slicerTemplate, RNA rnaTemplate, Transform[] positions, Nucleus nRef)
    {
        //produce RNA 
        rnaTemplate.templateProtein = slicerTemplate.gameObject;
        List<RNA> rnaList = prepRNA(rnaTemplate, amount, positions, nRef);
        nRef.StartCoroutine(delayThenProduce(2, rnaList));        
       /*for(int i=0; i < rnaList.Count; i++)
        {
            SlicerEnzyme slicer = GameObject.Instantiate(slicerTemplate);
            slicer.transform.position = rnaList[i].transform.position;
            slicer.transform.parent = rnaList[i].transform.parent;
            slicer.transform.DOBlendableLocalMoveBy(Vector3.one * Random.Range(-3, 3), 2);
        }*/
         
        //make the slicers 

       
    }

    public void HijackAndProduceInjector(Nucleus nRef, InjectorVirus injector, int count = 1)
    {
        List<RNA> rnaList = prepRNA(injector.rnaPrefab, count, new Transform[] {injector.transform}, nRef);
        //need to wait 2 seconds
        _delayRoutine = nRef.StartCoroutine(delayThenProduce(2, rnaList));
    }

    public IEnumerator delayThenProduce(float delay, List<RNA> rnaList)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < rnaList.Count; i++)
        {
         
            GameObject newProtein = GameObject.Instantiate(rnaList[i].templateProtein);
            newProtein.transform.parent = rnaList[i].transform.parent;
            newProtein.transform.position = rnaList[i].transform.position;
            
        }
    }

    private List<RNA> prepRNA(RNA rnaTemplate, int count, Transform[] positions, Nucleus nRef)
    {
        List<RNA> rnaList = produceRNA(rnaTemplate, count, positions);
        sendRNAToRibosomes(rnaList);
        for (int i = 0; i < rnaList.Count; i++)
        {
            rnaList[i].Descend();
        }

        return rnaList;
    }

    public List<RNA> produceRNA(RNA rnaTemplate, int count, Transform[] positions)
    {
        List<RNA> rnaList = new List<RNA>();
        for (int i = 0; i < count; i++)
        {
            RNA rna = GameObject.Instantiate(rnaTemplate);
            rna.transform.position = positions[Random.Range(0, positions.Length)].transform.position;
            rnaList.Add(rna);
            rna.transform.SetParent(positions[0].transform.parent);
        }

        return rnaList;
    }

    private void sendRNAToRibosomes(List<RNA> rnaList)
    {
        Ribosome[] ribosomes = GameObject.FindObjectsByType<Ribosome>(FindObjectsSortMode.None);
        if (ribosomes.Length < rnaList.Count)
        {
            Debug.LogError("Not enough ribosomes!");
            return;
        }
        for(int i=0; i < rnaList.Count; i++)
            rnaList[i].transform.DOMove(ribosomes[Random.Range(0,ribosomes.Length)].transform.position, 1).SetDelay(1);
    }


}
