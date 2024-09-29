﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class GeneticAlgorithm
    {
        private List<Route> population;
        private double[,] distanceMatrix;
        private int populationSize;
        private int generations;

        public GeneticAlgorithm(double[,] distanceMatrix, int populationSize, int generations)
        {
            this.distanceMatrix = distanceMatrix;
            this.populationSize = populationSize;
            this.generations = generations;
            population = new List<Route>();

            for (int i = 0; i < populationSize; i++)
            {
                population.Add(new Route(distanceMatrix));
            }
        }

        public Route Run()
        {
            for (int generation = 0; generation < generations; generation++)
            {
                Random rand = new Random();
                population = population.OrderBy(r => r.Length).ToList();
                List<Route> newPopulation = new List<Route>();

                for (int i = 0; i < populationSize/2; i++)
                {
                    newPopulation.Add(population[i]);
                }

                while (newPopulation.Count < populationSize)
                {
                    Route parent1 = population[rand.Next(populationSize/2)];
                    Route parent2 = population[rand.Next(populationSize/2)];

                    Route child = Cross(parent1, parent2);
                    child.Mutate();
                    newPopulation.Add(child);
                    
                }

                population = newPopulation;
                Route bestRoute = population.OrderBy(r => r.Length).First();
                Console.WriteLine($"Generation: {generation}, Best Length: {bestRoute.Length}");
                Console.WriteLine($"Best Route: {string.Join(", ", bestRoute.Cities)}");
            }

            return population.OrderBy(r => r.Length).First();
        }

    private Route Cross(Route parent1, Route parent2)
    {
            int length = parent1.Cities.Count;
            int[] childCities = new int[length];

            Random rand = new Random();

            int crossoverPoint = rand.Next(1, length - 1);

            for (int i = 0; i < crossoverPoint; i++)
            {
                childCities[i] = parent1.Cities[i];
            }

            int currentIndex = crossoverPoint;

            for (int i = 0; i < length; i++)
            {
                int city = parent2.Cities[i];
                if (!childCities.Take(crossoverPoint).Contains(city))
                {
                    childCities[currentIndex] = city;
                    currentIndex++;
                    if (currentIndex >= length) break;
                }
            }

            return new Route(childCities.ToList(), distanceMatrix);
        }
}

}