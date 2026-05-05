namespace TourneeFutee
{
    internal class Program
    {
        static void AfficheMatrice(List<List<float>> liste)
        {
            foreach (var item in liste)
            {
                foreach (var f in item)
                {
                    if (f == float.PositiveInfinity) Console.Write("+ ");
                    else Console.Write(f.ToString() + " ");
                }
                Console.WriteLine();
            }
            
        }
        static void AfficheListe(List<float> liste)
        {
            foreach(var f in liste)
            {
                if (f == float.PositiveInfinity) Console.Write("+ ");
                else Console.Write(f.ToString()+ " ");
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            // TODO : faire vos propres tests ici
            List<List<float>> list = new List<List<float>>();
            list.Add(new List<float>() { float.PositiveInfinity, 1.0f, 7.0f, 3.0f, 14.0f, 2.0f });
            list.Add(new List<float>() { 3.0f, float.PositiveInfinity, 6.0f, 9.0f, 1.0f, 24.0f });
            list.Add(new List<float>() { 6.0f, 14.0f, float.PositiveInfinity, 3.0f, 7.0f, 3.0f });
            list.Add(new List<float>() { 2.0f, 3.0f, 5.0f, float.PositiveInfinity, 9.0f, 11.0f });
            list.Add(new List<float>() { 15.0f, 7.0f, 11.0f, 2.0f, float.PositiveInfinity, 4.0f });
            list.Add(new List<float>() { 20.0f, 5.0f, 13.0f, 4.0f, 18.0f, float.PositiveInfinity });
            AfficheMatrice(list);
            Console.WriteLine();

            Matrix mat = new Matrix(list);

            AfficheListe(mat.GetLigne(0));
            AfficheListe(mat.GetColonne(0));
            Console.WriteLine(Matrix.MinListe(mat.GetLigne(0)));
            Console.WriteLine(Matrix.MinListe(mat.GetColonne(0)));

            mat.SoustraireLigne(0, 1.0f);
            mat.SoustraireColonne(0, 1.0f);

            Console.WriteLine();
            mat.Print();

            Graph villes = new Graph(true, mat);
            villes.VertexIndices.Add("Paris", 0);
            villes.VertexIndices.Add("Marseille", 1);
            villes.VertexIndices.Add("Lille", 2);
            villes.VertexIndices.Add("Nantes", 3);
            villes.VertexIndices.Add("Quimper", 4);
            villes.VertexIndices.Add("Woippy", 5);
            Console.WriteLine();

            Console.WriteLine(villes.AdjacencyMatrix.GetValue(5, 4));
            villes.AfficheSegment(5, 4);

            // mettre son propre identifiant et mdp pour le test
            ServicePersistance testbdd = new ServicePersistance("localhost", "tourneefutee", "root", " ");
        }
    }
}
