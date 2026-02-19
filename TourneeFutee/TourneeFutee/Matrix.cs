namespace TourneeFutee
{
    public class Matrix
    {
        // TODO : ajouter tous les attributs que vous jugerez pertinents 


        /* Crée une matrice de dimensions `nbRows` x `nbColums`.
         * Toutes les cases de cette matrice sont remplies avec `defaultValue`.
         * Lève une ArgumentOutOfRangeException si une des dimensions est négative
         */
        private float[,] data;
        private float defaultValue;

        public Matrix(int nbRows = 0, int nbColumns = 0, float defaultValue = 0)
        {
            // TODO : implémenter
            if (nbRows < 0 || nbColumns < 0)
                throw new ArgumentOutOfRangeException();

            this.defaultValue = defaultValue;
            data = new float[nbRows, nbColumns];

            for (int i = 0; i < nbRows; i++)
                for (int j = 0; j < nbColumns; j++)
                    data[i, j] = defaultValue;
        }
        public float DefaultValue => defaultValue;

        public int NbRows => data.GetLength(0);

        public int NbColumns => data.GetLength(1);

        // Propriété : valeur par défaut utilisée pour remplir les nouvelles cases
        // Lecture seule



        // Propriété : nombre de lignes
        // Lecture seule


        // Propriété : nombre de colonnes
        // Lecture seule


        /* Insère une ligne à l'indice `i`. Décale les lignes suivantes vers le bas.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `i` = NbRows, insère une ligne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
         */
        public void AddRow(int i)
        {
            if (i < 0 || i > NbRows)
                throw new ArgumentOutOfRangeException();

            float[,] newData = new float[NbRows + 1, NbColumns];

            for (int r = 0; r < NbRows + 1; r++)
            {
                for (int c = 0; c < NbColumns; c++)
                {
                    if (r < i)
                        newData[r, c] = data[r, c];
                    else if (r == i)
                        newData[r, c] = defaultValue;
                    else
                        newData[r, c] = data[r - 1, c];
                }
            }

            data = newData;
            // TODO : implémenter
        }

        /* Insère une colonne à l'indice `j`. Décale les colonnes suivantes vers la droite.
         * Toutes les cases de la nouvelle ligne contiennent DefaultValue.
         * Si `j` = NbColums, insère une colonne en fin de matrice
         * Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
         */
        public void AddColumn(int j)
        {
            if (j < 0 || j > NbColumns)
                throw new ArgumentOutOfRangeException();

            float[,] newData = new float[NbRows, NbColumns + 1];

            for (int r = 0; r < NbRows; r++)
            {
                for (int c = 0; c < NbColumns + 1; c++)
                {
                    if (c < j)
                        newData[r, c] = data[r, c];
                    else if (c == j)
                        newData[r, c] = defaultValue;
                    else
                        newData[r, c] = data[r, c - 1];
                }
            }

            data = newData;
            // TODO : implémenter
        }

        // Supprime la ligne à l'indice `i`. Décale les lignes suivantes vers le haut.
        // Lève une ArgumentOutOfRangeException si `i` est en dehors des indices valides
        public void RemoveRow(int i)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbRows)
                throw new ArgumentOutOfRangeException();

            float[,] newData = new float[NbRows - 1, NbColumns];

            for (int r = 0; r < NbRows; r++)
            {
                if (r == i) continue;

                for (int c = 0; c < NbColumns; c++)
                {
                    if (r < i)
                        newData[r, c] = data[r, c];
                    else
                        newData[r - 1, c] = data[r, c];
                }
            }

            data = newData;
        }

        // Supprime la colonne à l'indice `j`. Décale les colonnes suivantes vers la gauche.
        // Lève une ArgumentOutOfRangeException si `j` est en dehors des indices valides
        public void RemoveColumn(int j)
        {
            // TODO : implémenter
            if (j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException();

            float[,] newData = new float[NbRows, NbColumns - 1];

            for (int r = 0; r < NbRows; r++)
            {
                for (int c = 0; c < NbColumns; c++)
                {
                    if (c == j) continue;

                    if (c < j)
                        newData[r, c] = data[r, c];
                    else
                        newData[r, c - 1] = data[r, c];
                }
            }

            data = newData;
        }

        // Renvoie la valeur à la ligne `i` et colonne `j`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public float GetValue(int i, int j)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbRows || j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException();

            return data[i, j];

        }

        // Affecte la valeur à la ligne `i` et colonne `j` à `v`
        // Lève une ArgumentOutOfRangeException si `i` ou `j` est en dehors des indices valides
        public void SetValue(int i, int j, float v)
        {
            // TODO : implémenter
            if (i < 0 || i >= NbRows || j < 0 || j >= NbColumns)
                throw new ArgumentOutOfRangeException();

            data[i, j] = v;
        }

        // Affiche la matrice
        public void Print()
        {
            // TODO : implémenter
        }


        // TODO : ajouter toutes les méthodes que vous jugerez pertinentes 

    }


}
