using System.Collections.Generic;

public class WeightCalculator
{
    //Creating a new reference is not good for performance.
    private Dictionary<int, GameAction> nodeCollection;
    private float[][] weights;

    public void setNodeCollection(Dictionary<int, GameAction> nodeCollection)
    {
        this.nodeCollection = nodeCollection;
        this.weights = new float[nodeCollection.Count][];
        PrepareArray();
    }

    public float[][] GetWeights()
    {
        return weights;
    }

    public void CalculateWeight(GameAction previousNode, GameAction currentNode, GenericVector accumulatedValues,
        GenericVector npcPersonalityVectors)
    {
        accumulatedValues.Sum(currentNode.PersonalityModifiers.ToGenericVector());
        float angle = GenericVector.GetAngle(accumulatedValues, npcPersonalityVectors);

        if (previousNode.Id != currentNode.Id)
            weights[previousNode.Id][currentNode.Id] = 1 / angle;

        foreach (var i in currentNode.NeighbourIds)
        {
            if (weights[currentNode.Id][nodeCollection[i].Id] == 0.0f)
            {
                CalculateWeight(currentNode, nodeCollection[i], accumulatedValues, npcPersonalityVectors);
            }
        }
    }

    private void PrepareArray()
    {
        for (int i = 0; i < nodeCollection.Count; i++)
        {
            weights[i] = new float[nodeCollection.Count];
        }
    }
}
                                                                                                                                                                                                                                 