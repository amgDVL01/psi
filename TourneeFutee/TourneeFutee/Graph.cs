namespace TourneeFutee
{
    public class Graph
    {

        // TODO : ajouter tous les attributs que vous jugerez pertinents 

        private Matrix adjacencyMatrix;
        private Dictionary<string, int> vertexIndices;
        private Dictionary<string, float> vertexValues;
        private bool directed;
        private float noEdgeValue;
        // --- Construction du graphe ---

        // Contruit un graphe (`directed`=true => orienté)
        // La valeur `noEdgeValue` est le poids modélisant l'absence d'un arc (0 par défaut)
        public Graph(bool directed, float noEdgeValue = 0)
        {
            // TODO : implémenter
            this.directed = directed;
            this.noEdgeValue = noEdgeValue;

            adjacencyMatrix = new Matrix(0, 0, noEdgeValue);
            vertexIndices = new Dictionary<string, int>();
            vertexValues = new Dictionary<string, float>();
        }


        // --- Propriétés ---

        // Propriété : ordre du graphe
        // Lecture seule
        // pas de set

        // Propriété : graphe orienté ou non
        // Lecture seule
        public int Order
        {
            get { return vertexIndices.Count; }
        }

        public bool Directed
        {
            get { return directed; }
        }


        // --- Gestion des sommets ---

        // Ajoute le sommet de nom `name` et de valeur `value` (0 par défaut) dans le graphe
        // Lève une ArgumentException s'il existe déjà un sommet avec le même nom dans le graphe
        public void AddVertex(string name, float value = 0)
        {
            // TODO : implémenter

            if (vertexIndices.ContainsKey(name)) throw new ArgumentException();

            int newIndex = Order;

            vertexIndices[name] = newIndex;
            vertexValues[name] = value;

            adjacencyMatrix.AddRow(newIndex);
            adjacencyMatrix.AddColumn(newIndex);
        }


        // Supprime le sommet de nom `name` du graphe (et tous les arcs associés)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void RemoveVertex(string name)
        {
            // TODO : implémenter
            if (!vertexIndices.ContainsKey(name)) throw new ArgumentException();

            int index = vertexIndices[name];

            adjacencyMatrix.RemoveRow(index);
            adjacencyMatrix.RemoveColumn(index);

            vertexIndices.Remove(name);
            vertexValues.Remove(name);

            // Réindexation
            var keys = new List<string>(vertexIndices.Keys);
            foreach (var key in keys)
            {
                if (vertexIndices[key] > index) vertexIndices[key]--;
            }
        }

        // Renvoie la valeur du sommet de nom `name`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public float GetVertexValue(string name)
        {
            // TODO : implémenter
            if (!vertexValues.ContainsKey(name)) throw new ArgumentException();

            return vertexValues[name];
        }

        // Affecte la valeur du sommet de nom `name` à `value`
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public void SetVertexValue(string name, float value)
        {
            // TODO : implémenter
            if (!vertexValues.ContainsKey(name)) throw new ArgumentException();

            vertexValues[name] = value;
        }


        // Renvoie la liste des noms des voisins du sommet de nom `vertexName`
        // (si ce sommet n'a pas de voisins, la liste sera vide)
        // Lève une ArgumentException si le sommet n'a pas été trouvé dans le graphe
        public List<string> GetNeighbors(string vertexName)
        {
            if (!vertexIndices.ContainsKey(vertexName)) throw new ArgumentException();

            List<string> neighbors = new List<string>();
            int index = vertexIndices[vertexName];

            foreach (var pair in vertexIndices)
            {
                int j = pair.Value;
                if (adjacencyMatrix.GetValue(index, j) != noEdgeValue)neighbors.Add(pair.Key);
            }

            return neighbors;
        }

        // --- Gestion des arcs ---

        /* Ajoute un arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`, avec le poids `weight` (1 par défaut)
         * Si le graphe n'est pas orienté, ajoute aussi l'arc inverse, avec le même poids
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - il existe déjà un arc avec ces extrémités
         */
        public void AddEdge(string sourceName, string destinationName, float weight = 1)
        {
            // TODO : implémenter
            if (!vertexIndices.ContainsKey(sourceName) || !vertexIndices.ContainsKey(destinationName)) throw new ArgumentException();

            int i = vertexIndices[sourceName];
            int j = vertexIndices[destinationName];

            if (adjacencyMatrix.GetValue(i, j) != noEdgeValue) throw new ArgumentException();

            adjacencyMatrix.SetValue(i, j, weight);

            if (!directed) adjacencyMatrix.SetValue(j, i, weight);
        }

        /* Supprime l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` du graphe
         * Si le graphe n'est pas orienté, supprime aussi l'arc inverse
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public void RemoveEdge(string sourceName, string destinationName)
        {
            // TODO : implémenter
            if (!vertexIndices.ContainsKey(sourceName) || !vertexIndices.ContainsKey(destinationName)) throw new ArgumentException();

            int i = vertexIndices[sourceName];
            int j = vertexIndices[destinationName];

            if (adjacencyMatrix.GetValue(i, j) == noEdgeValue) throw new ArgumentException();

            adjacencyMatrix.SetValue(i, j, noEdgeValue);

            if (!directed) adjacencyMatrix.SetValue(j, i, noEdgeValue);
        }

        /* Renvoie le poids de l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName`
         * Si le graphe n'est pas orienté, GetEdgeWeight(A, B) = GetEdgeWeight(B, A) 
         * Lève une ArgumentException dans les cas suivants :
         * - un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         * - l'arc n'existe pas
         */
        public float GetEdgeWeight(string sourceName, string destinationName)
        {
            // TODO : implémenter
            if (!vertexIndices.ContainsKey(sourceName) || !vertexIndices.ContainsKey(destinationName)) throw new ArgumentException();

            int i = vertexIndices[sourceName];
            int j = vertexIndices[destinationName];

            float weight = adjacencyMatrix.GetValue(i, j);

            if (weight == noEdgeValue) throw new ArgumentException();

            return weight;
        }

        /* Affecte le poids l'arc allant du sommet nommé `sourceName` au sommet nommé `destinationName` à `weight` 
         * Si le graphe n'est pas orienté, affecte le même poids à l'arc inverse
         * Lève une ArgumentException si un des sommets n'a pas été trouvé dans le graphe (source et/ou destination)
         */
        public void SetEdgeWeight(string sourceName, string destinationName, float weight)
        {
            // TODO : implémenter
            if (!vertexIndices.ContainsKey(sourceName) || !vertexIndices.ContainsKey(destinationName)) throw new ArgumentException();

            int i = vertexIndices[sourceName];
            int j = vertexIndices[destinationName];

            adjacencyMatrix.SetValue(i, j, weight);

            if (!directed) adjacencyMatrix.SetValue(j, i, weight);
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
