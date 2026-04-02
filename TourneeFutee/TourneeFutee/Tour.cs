using System.Text.Json.Serialization.Metadata;

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

        public Tour (List<(string source,string destination)> segments, Graph graphe)
        {
            this.nbSegments = 0;
            this.cost = 0;
            this.graphe = graphe;
            this.segments=new Dictionary<string, string>();
            foreach (var s in segments) AddSegment(s);
        }
        public Tour(List<(string source, string destination)> segments)
        {
            this.nbSegments = 0;
            this.cost = 0;
            this.graphe = null ;
            this.segments = new Dictionary<string, string>();
            foreach (var s in segments) AddSegment(s);
        }
        public Tour(Graph graphe)
        {
            this.segments = new Dictionary<string, string>();
            this.graphe= graphe;
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
        // propriétés
        public Dictionary<string,string> Segments 
        { 
            get { return segments; } 
            set { segments = value; } 
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
            AddSegment(segment);
            string depart = segment.source;
            string courant = segment.destination;
            int compteurVilles = 1;
            while (segments.ContainsKey(courant) && compteurVilles<=nbCities)
            {
                courant = segments[courant];
                compteurVilles++;
                if (courant == depart)
                {
                    RemoveSegment(segment);
                    return compteurVilles < nbCities;
                }
            }
            RemoveSegment(segment);
            return false;
        }
        public void AddSegment((string source, string destination) segment)
        {
            if (!segments.ContainsKey(segment.source))
            {
                segments[segment.source] = segment.destination;
                nbSegments++;
            }
        }
        public void RemoveSegment((string source, string destination) segment)
        {
            if (ContainsSegment(segment)) segments.Remove(segment.source);
        }

    }
}
