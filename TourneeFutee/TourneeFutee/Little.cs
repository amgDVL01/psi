using System.Collections.Generic;

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

            return new Tour();
        }


         //--- Méthodes utilitaires réalisant des étapes de l'algorithme de Little


        // Réduit la matrice `m` et revoie la valeur totale de la réduction
        // Après appel à cette méthode, la matrice `m` est *modifiée*.
        public static float ReduceMatrix(Matrix m)
        {
            // TODO : implémenter
            float total = 0;
            float minCourant;
            for(int i=0;i<m.Valeurs.Count;i++)
            {
                minCourant = Matrix.MinListe(m.GetLigne(i));
                m.SoustraireLigne(i, minCourant);
                total+= minCourant;
            }
            for (int j=0; j < m.Valeurs[0].Count;j++)
            {
                minCourant = Matrix.MinListe(m.GetColonne(j));
                m.SoustraireColonne(j, minCourant);
                total+= minCourant;
            }
            return total;
        }
        // Renvoie le regret de valeur maximale dans la matrice de coûts `m` sous la forme d'un tuple `(int i, int j, float value)`
        // où `i`, `j`, et `value` contiennent respectivement la ligne, la colonne et la valeur du regret maximale
        public static (int i, int j, float value) GetMaxRegret(Matrix m)
        {
            // TODO : implémenter
            float regretMax = float.NegativeInfinity, regretCourant;
            int imax=0, jmax=0;
            List<float> ligneCourante, colonneCourante;
            for(int k=0; k<m.Valeurs.Count;k++)
            {
                for(int l = 0; l < m.Valeurs[k].Count;l++)
                {
                    if (m.Valeurs[k][l]==0)
                    {
                        ligneCourante = new List<float>(m.GetLigne(k));
                        ligneCourante.RemoveAt(l);
                        colonneCourante = new List<float>(m.GetColonne(l));
                        colonneCourante.RemoveAt(k);
                        regretCourant = Matrix.MinListe(ligneCourante) + Matrix.MinListe(colonneCourante);
                        if (regretCourant>regretMax)
                        {
                            imax=k; 
                            jmax=l;
                            regretMax=regretCourant;
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

            // TODO : implémenter
            Tour tournee = new Tour(includedSegments);
            return (tournee.FormsCycle(segment, nbCities));
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

    }
}
