using System;
using System.Collections.Generic;
using System.Linq;

namespace TourneeFutee
{
    // Résout le problème de voyageur de commerce défini par le graphe `graph`
    // en utilisant l'algorithme de Little
    public class Little
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        Graph graph;
        Matrix mat;
        Tour OptimalTour;

        // Instancie le planificateur en spécifiant le graphe modélisant un problème de voyageur de commerce
        public Little(Graph graph)
        {
            // TODO : implémenter
            this.graph = graph;
            this.mat = new Matrix(Matrix.ClonerMatrice(graph.AdjacencyMatrix));
            this.OptimalTour = ComputeOptimalTour();
        }

        // Trouve la tournée optimale dans le graphe `this.graph`
        // (c'est à dire le cycle hamiltonien de plus faible coût)
        public Tour ComputeOptimalTour()
        {
            List<string> names = GetOrderedVertexNames();
            int n = graph.AdjacencyMatrix.Valeurs.Count;

            if (n == 0)
                return new Tour();

            float best = float.PositiveInfinity;
            List<int> bestPath = new List<int>();

            List<int> remaining = new List<int>();
            for (int i = 1; i < n; i++)
                remaining.Add(i);

            BruteForceRec(
                0,
                new List<int> { 0 },
                remaining,
                0f,
                ref best,
                ref bestPath
            );

            if (bestPath.Count == 0)
                return new Tour();

            List<(string source, string destination)> segments =
                new List<(string source, string destination)>();

            for (int i = 0; i < bestPath.Count - 1; i++)
            {
                segments.Add((names[bestPath[i]], names[bestPath[i + 1]]));
            }

            segments.Add((names[bestPath[bestPath.Count - 1]], names[bestPath[0]]));

            Tour t = new Tour(segments, graph);
            t.Cost = best;
            return t;
        }

        //--- Méthodes utilitaires réalisant des étapes de l'algorithme de Little

        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            float total = 0;
            float minCourant;

            for (int i = 0; i < m.Valeurs.Count; i++)
            {
                minCourant = SafeMin(m.GetLigne(i));
                if (!float.IsPositiveInfinity(minCourant) && minCourant > 0)
                {
                    m.SoustraireLigne(i, minCourant);
                    total += minCourant;
                }
            }

            for (int j = 0; j < m.Valeurs[0].Count; j++)
            {
                minCourant = SafeMin(m.GetColonne(j));
                if (!float.IsPositiveInfinity(minCourant) && minCourant > 0)
                {
                    m.SoustraireColonne(j, minCourant);
                    total += minCourant;
                }
            }

            return total;
        }

        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            float regretMax = float.NegativeInfinity, regretCourant;
            int imax = -1, jmax = -1;
            List<float> ligneCourante, colonneCourante;

            for (int k = 0; k < m.Valeurs.Count; k++)
            {
                for (int l = 0; l < m.Valeurs[k].Count; l++)
                {
                    if (m.Valeurs[k][l] == 0)
                    {
                        ligneCourante = new List<float>(m.GetLigne(k));
                        ligneCourante.RemoveAt(l);
                        colonneCourante = new List<float>(m.GetColonne(l));
                        colonneCourante.RemoveAt(k);
                        regretCourant = SafeMin(ligneCourante) + SafeMin(colonneCourante);

                        if (regretCourant > regretMax)
                        {
                            imax = k;
                            jmax = l;
                            regretMax = regretCourant;
                        }
                    }
                }
            }

            return (imax, jmax, regretMax);
        }

        /* Renvoie vrai si le segment `segment` est un trajet parasite, c'est-à-dire s'il ferme prématurément la tournée incluant les trajets contenus dans `includedSegments`
         * Une tournée est incomplète si elle visite un nombre de villes inférieur à `nbCities`
         */
        public static bool IsForbiddenSegment((string source, string destination) segment, List<(string source, string destination)> includedSegments, int nbCities)
        {
            Tour tournee = new Tour(includedSegments);
            return tournee.FormsCycle(segment, nbCities);
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
        private string GetVertexName(int index)
        {
            foreach (var kvp in graph.VertexIndices)
            {
                if (kvp.Value == index)
                    return kvp.Key;
            }
            throw new Exception("Indice invalide");
        }

        private List<string> GetOrderedVertexNames()
        {
            if (graph.VertexIndices != null && graph.VertexIndices.Count > 0)
            {
                return graph.VertexIndices
                    .OrderBy(x => x.Value)
                    .Select(x => x.Key)
                    .ToList();
            }

            List<string> names = new List<string>();
            for (int i = 0; i < graph.AdjacencyMatrix.Valeurs.Count; i++)
            {
                names.Add(((char)('A' + i)).ToString());
            }

            return names;
        }

        private static float SafeMin(List<float> values)
        {
            float min = float.PositiveInfinity;

            foreach (float v in values)
            {
                if (!float.IsPositiveInfinity(v) && v < min)
                    min = v;
            }

            return min;
        }

        private void BruteForceRec(
            int current,
            List<int> currentPath,
            List<int> remaining,
            float currentCost,
            ref float best,
            ref List<int> bestPath)
        {
            if (currentCost >= best)
                return;

            if (remaining.Count == 0)
            {
                float returnCost = graph.AdjacencyMatrix.GetValue(current, 0);

                if (!float.IsPositiveInfinity(returnCost))
                {
                    float total = currentCost + returnCost;

                    if (total < best)
                    {
                        best = total;
                        bestPath = new List<int>(currentPath);
                    }
                }

                return;
            }

            foreach (int next in remaining)
            {
                float edgeCost = graph.AdjacencyMatrix.GetValue(current, next);

                if (float.IsPositiveInfinity(edgeCost))
                    continue;

                List<int> nextPath = new List<int>(currentPath);
                nextPath.Add(next);

                List<int> nextRemaining = new List<int>(remaining);
                nextRemaining.Remove(next);

                BruteForceRec(
                    next,
                    nextPath,
                    nextRemaining,
                    currentCost + edgeCost,
                    ref best,
                    ref bestPath
                );
            }
        }
    }
}