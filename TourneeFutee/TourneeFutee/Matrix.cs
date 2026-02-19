namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 
        List<List<float>> valeurs;
        int nbRows;
        int nbColumns;
        float defaultValue;

        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        public Matrix(int nbRows = 0, int nbColumns = 0, float defaultValue = 0)
        {
            // TODO : implémenter
            if (nbRows < 0)
                throw new ArgumentOutOfRangeException(nameof(nbRows));

            if (nbColumns < 0)
                throw new ArgumentOutOfRangeException(nameof(nbColumns));

            this.nbRows = nbRows;
            this.nbColumns = nbColumns;
            this.defaultValue = defaultValue;

            valeurs = new List<List<float>>(nbRows);

            for (int i = 0; i < nbRows; i++)
            {
                var row = new List<float>(nbColumns);
                for (int j = 0; j < nbColumns; j++)
                {
                    row.Add(defaultValue);
                }
                valeurs.Add(row);
            }
        }

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule
        public float DefaultValue
        {
            get { return defaultValue; } // TODO : implémenter
                 // pas de set
        }

        // Propriété : nombre de lignes
        // Lecture seule
        public int NbRows
        {
            get { return nbRows; } // TODO : implémenter
                 // pas de set
        }

        // Propriété : nombre de colonnes
        // Lecture seule
        public int NbColumns
        {
            get { return nbColumns; } // TODO : implémenter
                 // pas de set
        }

        public List<List<float>> Valeurs
        {
            get { return valeurs; }
        }

        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            // TODO : implémenter
            if (i < 0 || i > nbRows) throw new ArgumentOutOfRangeException(nameof(i));

            var newRow = new List<float>(nbColumns);
            for (int j = 0; j < nbColumns; j++)
            {
                newRow.Add(defaultValue);
            }

            valeurs.Insert(i, newRow);

            nbRows++;
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            // TODO : implémenter
            if (j < 0 || j > nbColumns) throw new ArgumentOutOfRangeException(nameof(j));

            foreach (var row in valeurs)
            {
                row.Insert(j, defaultValue);
            }

            nbColumns++;
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            // TODO : implémenter
            if (i < 0 || i >= nbRows) throw new ArgumentOutOfRangeException(nameof(i));

            valeurs.RemoveAt(i);

            nbRows--;
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            // TODO : implémenter
            if (j < 0 || j >= nbColumns) throw new ArgumentOutOfRangeException(nameof(j));

            foreach (var row in valeurs)
            {
                row.RemoveAt(j);
            }

            nbColumns--;
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            // TODO : implémenter
            if (i < 0 || i >= nbRows) throw new ArgumentOutOfRangeException(nameof(i));

            if (j < 0 || j >= nbColumns) throw new ArgumentOutOfRangeException(nameof(j));

            return valeurs[i][j];
        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
            if (i < 0 || i >= nbRows) throw new ArgumentOutOfRangeException(nameof(i));

            if (j < 0 || j >= nbColumns) throw new ArgumentOutOfRangeException(nameof(j));

            valeurs[i][j] = v;
        }

        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
            for (int i = 0; i < nbRows; i++)
            {
                for (int j = 0; j < nbColumns; j++)
                {
                    Console.Write(valeurs[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }

        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 
    }
}