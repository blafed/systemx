using System.Collections.Generic;
using System.IO;
using System;
namespace Nord
{
    [System.Serializable]
    public class Neural
    {

        private int[] layers;//layers    
        private float[][] neurons;//neurons    
        private float[][] biases;//biasses    
        private float[][][] weights;//weights    
        private int[] activations;//layers
        public float fitness = 0;//fitness
        public Neural(int[] layers)
        {
            this.layers = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                this.layers[i] = layers[i];
            }
            InitNeurons();
            InitBiases();
            InitWeights();
        }
        //create empty storage array for the neurons in the network
        private void InitBiases()
        {
            List<float[]> biasList = new List<float[]>();
            for (int i = 0; i < layers.Length; i++)
            {
                float[] bias = new float[layers[i]];
                for (int j = 0; j < layers[i]; j++)
                {
                    bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                biasList.Add(bias);
            }
            biases = biasList.ToArray();
        }
        private void InitNeurons()
        {
            List<float[]> neuronsList = new List<float[]>(layers.Length);
            for (int i = 0; i < layers.Length; i++)
            {
                neuronsList.Add(new float[layers[i]]);
            }
            neurons = neuronsList.ToArray();
        }
        private void InitWeights()
        {
            List<float[][]> weightsList = new List<float[][]>();
            for (int i = 1; i < layers.Length; i++)
            {
                List<float[]> layerWeightsList = new List<float[]>();
                int neuronsInPreviousLayer = layers[i - 1];
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float[] neuronWeights = new float[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = UnityEngine.Random.Range(minInclusive: -0.5f, 0.5f);
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            weights = weightsList.ToArray();
        }
        public float activate(float value)
        {
            if (value > 0)
                return value;
            return 0;
            //return (float)Math.Tanh(value);
        }
        public void writeOutputs(float[] target)
        {
            for (int i = 0; i < target.Length; i++)
            {
                target[i] = neurons[layers.Length - 1][i];
            }
        }
        public float[] FeedForward(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                neurons[0][i] = inputs[i];
            }
            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = 0f;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    }
                    neurons[i][j] = activate(value + biases[i][j]);
                }
            }
            return neurons[neurons.Length - 1];
        }
        public int CompareTo(Neural other)
        {
            if (other == null)
                return 1;
            if (fitness > other.fitness)
                return 1;
            else if (fitness < other.fitness)
                return -1;
            else
                return 0;
        }
        //this loads the biases and weights from within a file into the neural network.
        public void Load(string path)
        {
            TextReader tr = new StreamReader(path);
            int NumberOfLines = (int)new FileInfo(path).Length;
            string[] ListLines = new string[NumberOfLines];
            int index = 1;
            for (int i = 1; i < NumberOfLines; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
            tr.Close();
            if (new FileInfo(path).Length > 0)
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = float.Parse(ListLines[index]);
                        index++;
                    }
                }
                for (int i = 0; i < weights.Length; i++)
                {
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            weights[i][j][k] = float.Parse(ListLines[index]);
                            index++;
                        }
                    }
                }
            }
        }
        public void Mutate(int chance, float val)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 50) ? biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 50) ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) : weights[i][j][k];

                    }
                }
            }
        }
        //Luckily Unity provides a built in function for Tanh
    }


    public interface IMotive
    {
        void write(IList<float> array, int startIndex);
    }



    [System.Serializable]
    public class Brain
    {

        public IMotive sensor;
        public IMotive motor;
        public virtual void collectSense()
        {

        }

    }

    public class FloatBuffer
    {
        public void push(float f) { }
    }
}