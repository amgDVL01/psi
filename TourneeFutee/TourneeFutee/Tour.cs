using System.Text.Json.Serialization.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TourneeFutee
{
    // Modélise une tournée dans le cadre du problème du voyageur de commerce
    public class Tour
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        Dictionary<string, string> segments;
        Graph? graphe;
        float cost;
        int nbSegments;

        public Tour(List<(string source, string destination)> segments, Graph graphe)
        {
            this.nbSegments = 0;
            this.cost = 0;
            this.graphe = graphe;
            this.segments = new Dictionary<string, string>();
            foreach (var s in segments) AddSegment(s);
        }
        public Tour(List<(string source, string destination)> segments)
        {
            this.nbSegments = 0;
            this.cost = 0;
            this.graphe = null;
            this.segments = new Dictionary<string, string>();
            foreach (var s in segments) AddSegment(s);
        }
        public Tour(Graph graphe)
        {
            this.segments = new Dictionary<string, string>();
            this.graphe = graphe;
            this.nbSegments = 0;
            this.cost = 0;
        }
        public Tour()
        {
            this.segments = new Dictionary<string, string>();
            this.graphe = null;
            this.nbSegments = 0;
            this.cost = 0;
        }
        public Tour(List<string> sommets, float cout)
        {
            this.segments = new Dictionary<string, string>();
            this.graphe = null;
            this.nbSegments = 0;
            this.cost = cout;
            for (int i = 0; i < sommets.Count - 1; i++)
            {
                segments.Add(sommets[i], sommets[i + 1]);
                nbSegments++;
            }
        }
        // propriétés
        public Dictionary<string, string> Segments
        {
            get { return segments; }
            set
            {
                segments = value ?? new Dictionary<string, string>();
                nbSegments = segments.Count;
                cost = 0;

                if (graphe != null)
                {
                    foreach (var s in segments)
                    {
                        try
                        {
                            cost += graphe.GetEdgeWeight(s.Key, s.Value);
                        }
                        catch { }
                    }
                }
            }
        }
        // Coût total de la tournée
        public float Cost
        {
            get { return cost; }
            set { cost = value; }// TODO : implémenter
        }

        // Nombre de trajets dans la tournée
        public int NbSegments
        {
            get { return nbSegments; }    // TODO : implémenter
        }

        // Renvoie vrai si la tournée contient le trajet `source`->`destination`
        public bool ContainsSegment((string source, string destination) segment)
        {
            foreach (var s in segments)
            {
                if (segment.source == s.Key && segment.destination == s.Value) return true;
            }
            return false;   // TODO : implémenter 
        }

        // Affiche les informations sur la tournée : coût total et trajets
        public void Print()
        {
            // TODO : implémenter 
            Console.WriteLine("Tour : ");
            foreach (var s in segments) Console.WriteLine(s.Key + " --> " + s.Value);
            Console.WriteLine("coût total : " + cost);
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
        public bool FormsCycle((string source, string destination) segment, int nbCities)
        {
            if (segments.ContainsKey(segment.source)) return true;
            if (segments.Values.Contains(segment.destination)) return true;

            string depart = segment.source;
            string courant = segment.destination;
            int compteurVilles = 1;

            while (segments.ContainsKey(courant) && compteurVilles <= nbCities)
            {
                courant = segments[courant];
                compteurVilles++;

                if (courant == depart)
                    return compteurVilles < nbCities;
            }

            return false;
        }

        public void AddSegment((string source, string destination) segment)
        {
            if (segments.ContainsKey(segment.source)) return;
            if (segments.Values.Contains(segment.destination)) return;

            segments[segment.source] = segment.destination;
            nbSegments++;

            if (graphe != null)
            {
                try
                {
                    cost += graphe.GetEdgeWeight(segment.source, segment.destination);
                }
                catch { }
            }
        }

        public void RemoveSegment((string source, string destination) segment)
        {
            if (!ContainsSegment(segment)) return;

            if (graphe != null)
            {
                try
                {
                    cost -= graphe.GetEdgeWeight(segment.source, segment.destination);
                }
                catch { }
            }

            segments.Remove(segment.source);
            nbSegments--;
        }
        public IList<string> vertices()
        {
            var verts= new List<string>();
            foreach (var segment in segments) verts.Add(segment.Key);
            return verts.AsReadOnly();
        }
        public IList<string> Vertices { get { return vertices(); } }
    }
}