using System;
using MySql.Data.MySqlClient;

namespace TourneeFutee
{
    /// <summary>
    /// Service de persistance permettant de sauvegarder et charger
    /// des graphes et des tournées dans une base de données MySQL.
    /// </summary>
    public class ServicePersistance
    {
        // ─────────────────────────────────────────────────────────────────────
        // Attributs privés
        // ─────────────────────────────────────────────────────────────────────

        private readonly string _connectionString;
        private readonly MySqlConnection _connexion;

        // TODO : si vous avez besoin de maintenir une connexion ouverte,
        //        ajoutez un attribut MySqlConnection ici.

        // ─────────────────────────────────────────────────────────────────────
        // Constructeur
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Instancie un service de persistance et se connecte automatiquement
        /// à la base de données <paramref name="dbname"/> sur le serveur
        /// à l'adresse IP <paramref name="serverIp"/>.
        /// Les identifiants sont définis par <paramref name="user"/> (utilisateur)
        /// et <paramref name="pwd"/> (mot de passe).
        /// </summary>
        /// <param name="serverIp">Adresse IP du serveur MySQL.</param>
        /// <param name="dbname">Nom de la base de données.</param>
        /// <param name="user">Nom d'utilisateur.</param>
        /// <param name="pwd">Mot de passe.</param>
        /// <exception cref="Exception">Levée si la connexion échoue.</exception>
        public ServicePersistance(string serverIp, string dbname, string user, string pwd)
        {
          // TODO : initialiser et ouvrir la connexion à la base de données
        // Exemple :
            _connectionString = $"server={serverIp};database={dbname};uid={user};pwd={pwd};";
            _connexion = new MySqlConnection(_connectionString) ;

            // TODO : tester la connexion dès la construction
            //        (ouvrir puis fermer une connexion pour valider les paramètres)
            bool connexionReussie = false;
            try
            {
                _connexion.Open();
                connexionReussie = true;
            }
            catch (Exception) { }
            finally
            {
                _connexion.Close();
            }
            if (!connexionReussie) throw new NotImplementedException("Constructeur non implémenté.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes publiques
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Sauvegarde le graphe <paramref name="g"/> en base de données
        /// (sommets et arcs inclus) et renvoie son identifiant.
        /// </summary>
        /// <param name="g">Le graphe à sauvegarder.</param>
        /// <returns>Identifiant du graphe en base de données (AUTO_INCREMENT).</returns>
        public uint SaveGraph(Graph g)
        {
            // TODO : implémenter la sauvegarde du graphe
            //
            // Ordre recommandé :
            //   1. INSERT dans la table Graphe -> récupérer l'id avec LAST_INSERT_ID()
            //   2. Pour chaque sommet de g : INSERT dans Sommet (valeur + graphe_id)
            //      -> conserver la correspondance sommet C# <-> id BdD
            //   3. Pour chaque arc de la matrice d'adjacence (poids != +inf) :
            //      INSERT dans Arc (sommet_source_id, sommet_dest_id, poids, graphe_id)
            //
            // Exemple pour récupérer l'id généré :
            //   uint id = Convert.ToUInt32(cmd.ExecuteScalar());

            string creerGraphe = "INSERT INTO Graphe(est_oriente) VALUES (@directed);";
            string ajouterSommet="";
            string ajouterArc = "";
            uint id=int.MaxValue;
            float weight=0;
            try
            {
                _connexion.Open();
                using (MySqlCommand cmdCreerGraphe = new MySqlCommand(creerGraphe, _connexion))
                {
                    cmdCreerGraphe.Parameters.AddWithValue("@directed", g.Directed);
                    cmdCreerGraphe.ExecuteNonQuery();

                    id = Convert.ToUInt32(cmdCreerGraphe.LastInsertedId);
                }

                Dictionary<string, uint> vertexIds = new Dictionary<string, uint>();
                foreach (var sommet in g.VertexValues)
                {
                    ajouterSommet = "INSERT INTO Sommet(graphe_id,nom,valeur) VALUES (@id , @name , @value);";
                    using (MySqlCommand cmdAjtValeur = new MySqlCommand(ajouterSommet, _connexion))
                    {
                        cmdAjtValeur.Parameters.AddWithValue("@id", id);
                        cmdAjtValeur.Parameters.AddWithValue("@name", sommet.Key);
                        cmdAjtValeur.Parameters.AddWithValue("@value", sommet.Value);
                        cmdAjtValeur.ExecuteNonQuery();

                        uint sommetId = Convert.ToUInt32(cmdAjtValeur.LastInsertedId);

                        vertexIds[sommet.Key] = sommetId;
                    }
                }

                
                foreach (var sommet1 in g.VertexValues)
                {
                    foreach (var sommet2 in g.VertexValues)
                    {
                        try
                        {
                            weight = g.GetEdgeWeight(sommet1.Key, sommet2.Key);

                            ajouterArc = "INSERT INTO Arc(graphe_id,sommet_source,sommet_dest,poids) VALUES(@id, @source, @dest, @weight);";
                            using (MySqlCommand cmdAjtArc = new MySqlCommand(ajouterArc, _connexion))
                            {
                                cmdAjtArc.Parameters.AddWithValue("@id", id);
                                cmdAjtArc.Parameters.AddWithValue("@source", vertexIds[sommet1.Key]);
                                cmdAjtArc.Parameters.AddWithValue("@dest", vertexIds[sommet2.Key]);
                                cmdAjtArc.Parameters.AddWithValue("@weight", weight);
                                cmdAjtArc.ExecuteNonQuery();
                            }
                        }
                        catch (ArgumentException)
                        {
                            
                        }
                    }
                }
            }
            finally 
            { 
                _connexion.Close();
            }
            if (id != int.MaxValue) return id;
            throw new NotImplementedException("SaveGraph non implémenté.");
        }

        /// <summary>
        /// Charge depuis la base de données le graphe identifié par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Graph"/>.
        /// </summary>
        /// <param name="id">Identifiant du graphe à charger.</param>
        /// <returns>Instance de <see cref="Graph"/> reconstituée.</returns>
        public Graph LoadGraph(uint id)
        {
            // TODO : implémenter le chargement du graphe
            //
            // Ordre recommandé :
            //   1. SELECT dans Graphe WHERE id = @id -> récupérer IsOriented, etc.
            //   2. SELECT dans Sommet WHERE graphe_id = @id -> reconstruire les sommets
            //      (respecter l'ordre d'insertion pour que les indices de la matrice
            //       correspondent à ceux sauvegardés)
            //   3. SELECT dans Arc WHERE graphe_id = @id -> reconstruire la matrice
            //      d'adjacence en utilisant les correspondances sommet_id <-> indice

            Graph g;
            try
            {
                _connexion.Open();

                // initialiser le graphe
                string getDirected = "SELECT est_oriente FROM Graphe WHERE id = @id";

                bool directed;

                using (MySqlCommand cmd = new MySqlCommand(getDirected, _connexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    object result = cmd.ExecuteScalar();
                    if (result == null) throw new Exception("Graphe introuvable");
                    directed = Convert.ToBoolean(result);
                }
                g = new Graph(directed);

                // ajouter les sommets
                Dictionary<uint, string> vertexNames = new Dictionary<uint, string>();
                string getVertices = "SELECT id, nom, valeur FROM Sommet WHERE graphe_id = @id ORDER BY id";
                using (MySqlCommand cmd = new MySqlCommand(getVertices, _connexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            uint vertexId =Convert.ToUInt32(reader["id"]);
                            string nom =Convert.ToString(reader["nom"]);
                            float valeur = 0;
                            if (reader["valeur"] != DBNull.Value)
                            {
                                valeur = Convert.ToSingle(reader["valeur"]);
                            }
                            g.AddVertex(nom, valeur);
                            vertexNames[vertexId] = nom;
                        }
                    }
                }

                // ajouter les arcs
                string getEdges = @"SELECT sommet_source,sommet_dest,poids FROM Arc WHERE graphe_id = @id";

                using (MySqlCommand cmd = new MySqlCommand(getEdges, _connexion))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            uint sourceId = Convert.ToUInt32(reader["sommet_source"]);
                            uint destId = Convert.ToUInt32(reader["sommet_dest"]);
                            float poids = Convert.ToSingle(reader["poids"]);
                            if (g.Directed || destId<sourceId)
                            {
                                string sourceName = vertexNames[sourceId];
                                string destName = vertexNames[destId];
                                g.AddEdge(sourceName, destName, poids);
                            }
                        }
                    }
                }
                
                return g;
            }
            finally
            {
                _connexion.Close();
            }
            throw new NotImplementedException("LoadGraph non implémenté.");
        }
                

        /// <summary>
        /// Sauvegarde la tournée <paramref name="t"/> (effectuée dans le graphe
        /// identifié par <paramref name="graphId"/>) en base de données
        /// et renvoie son identifiant.
        /// </summary>
        /// <param name="graphId">Identifiant BdD du graphe dans lequel la tournée a été calculée.</param>
        /// <param name="t">La tournée à sauvegarder.</param>
        /// <returns>Identifiant de la tournée en base de données (AUTO_INCREMENT).</returns>
        public uint SaveTour(uint graphId, Tour t)
        {
            // TODO : implémenter la sauvegarde de la tournée
            //
            // Ordre recommandé :
            //   1. INSERT dans Tournee (cout_total, graphe_id) -> récupérer l'id
            //   2. Pour chaque sommet de la séquence (avec son numéro d'ordre) :
            //      INSERT dans EtapeTournee (tournee_id, numero_ordre, sommet_id)
            //
            // Attention : conserver l'ordre des étapes est essentiel pour
            //             pouvoir reconstruire la tournée fidèlement au chargement.
            string addTour = "INSERT INTO Tournee(graphe_id, cout_total) VALUES (@id, @cost);";
            using (MySqlCommand cmdAddTour = new MySqlCommand(addTour, _connexion))
            {
                cmdAddTour.Parameters.AddWithValue("@id", graphId);
                cmdAddTour.Parameters.AddWithValue("@cost", t.Cost);

            }
            throw new NotImplementedException("SaveTour non implémenté.");
        }

        /// <summary>
        /// Charge depuis la base de données la tournée identifiée par <paramref name="id"/>
        /// et renvoie une instance de la classe <see cref="Tour"/>.
        /// </summary>
        /// <param name="id">Identifiant de la tournée à charger.</param>
        /// <returns>Instance de <see cref="Tour"/> reconstituée.</returns>
        public Tour LoadTour(uint id)
        {
            // TODO : implémenter le chargement de la tournée
            //
            // Ordre recommandé :
            //   1. SELECT dans Tournee WHERE id = @id -> récupérer cout_total et graphe_id
            //   2. SELECT dans EtapeTournee JOIN Sommet WHERE tournee_id = @id
            //      ORDER BY numero_ordre -> reconstruire la séquence ordonnée de sommets
            //   3. Construire et retourner l'instance Tour

            throw new NotImplementedException("LoadTour non implémenté.");
        }

        // ─────────────────────────────────────────────────────────────────────
        // Méthodes utilitaires privées (à compléter selon vos besoins)
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Crée et retourne une nouvelle connexion MySQL ouverte.
        /// Encadrez toujours l'appel dans un bloc using pour garantir la fermeture.
        /// </summary>
        private MySqlConnection OpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public uint GetVertexId(string name, uint graphId)
        {
            string getName = "SELECT id FROM Sommet WHERE nom=@name AND graphe_id=@id;";
            uint resultat;
            using (MySqlCommand cmdGetName = new MySqlCommand(getName, _connexion))
            {
                cmdGetName.Parameters.AddWithValue("@name", name);
                cmdGetName.Parameters.AddWithValue("@id", graphId);
                resultat = Convert.ToUInt32(cmdGetName.ExecuteScalar());
            }
            return resultat;
        }
    }
}
